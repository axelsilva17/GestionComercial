using System;
using System.Diagnostics;
using Microsoft.Data.Sqlite;

class Program
{
    static string DbPath = "StressTest.db";

    static void Main()
    {
        Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║    STRESS TEST COMPLETO — SQLite + GestionComercial     ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
        Console.WriteLine();

        if (System.IO.File.Exists(DbPath)) System.IO.File.Delete(DbPath);

        using var conn = new SqliteConnection($"Data Source={DbPath}");
        conn.Open();

        // Optimización SQLite
        Exec(conn, "PRAGMA journal_mode = WAL;");
        Exec(conn, "PRAGMA synchronous = NORMAL;");
        Exec(conn, "PRAGMA cache_size = -128000;");  // 128MB cache
        Exec(conn, "PRAGMA temp_store = MEMORY;");
        Exec(conn, "PRAGMA mmap_size = 536870912;"); // 512MB mmap
        Exec(conn, "PRAGMA page_size = 4096;");
        Exec(conn, "PRAGMA auto_vacuum = INCREMENTAL;");

        Console.WriteLine("SQLite: WAL, NORMAL sync, 128MB cache, 512MB mmap");
        Console.WriteLine();

        Console.Write("Creando schema completo con índices... ");
        var swTotal = Stopwatch.StartNew();
        CreateSchema(conn);
        Console.WriteLine($"OK ({swTotal.ElapsedMilliseconds} ms)");
        Console.WriteLine();

        // ════════════════════════════════════════════════════
        //  FASE 1: CARGA MASIVA DE TODAS LAS TABLAS
        // ════════════════════════════════════════════════════
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine("  FASE 1: CARGA MASIVA");
        Console.WriteLine("═══════════════════════════════════════════════════════════");

        // Productos: 100K
        Measure("100,000 Productos", () => InsertProductos(conn, 100_000));

        // Clientes: 50K
        Measure("50,000 Clientes", () => InsertClientes(conn, 50_000));

        // Proveedores: 5K
        Measure("5,000 Proveedores", () => InsertProveedores(conn, 5_000));

        // Usuarios: 100
        Measure("100 Usuarios", () => InsertUsuarios(conn, 100));

        // Compras: 100K (con detalles)
        Measure("100,000 Compras", () => InsertCompras(conn, 100_000));

        // Ventas: 500K (con detalles + pagos)
        Console.WriteLine();
        Measure("50,000 Ventas", () => InsertVentas(conn, 50_000));
        Measure("250,000 Ventas", () => InsertVentas(conn, 250_000));
        Measure("500,000 Ventas", () => InsertVentas(conn, 500_000));

        // Movimientos de stock: 200K
        Measure("200,000 Movimientos Stock", () => InsertMovimientosStock(conn, 200_000));

        // Movimientos de caja: 100K
        Measure("100,000 Movimientos Caja", () => InsertMovimientosCaja(conn, 100_000));

        // Auditoría: 500K
        Measure("500,000 Logs Auditoría", () => InsertAuditoria(conn, 500_000));

        var totalSize = new System.IO.FileInfo(DbPath).Length;
        Console.WriteLine();
        Console.WriteLine($"  Tamaño total DB: {totalSize / 1024.0 / 1024.0:N1} MB ({totalSize / 1024.0 / 1024.0 / 1024.0:N2} GB)");
        Console.WriteLine();

        // ════════════════════════════════════════════════════
        //  FASE 2: RESUMEN DE TABLAS
        // ════════════════════════════════════════════════════
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine("  RESUMEN TABLAS");
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        var tables = new[] {
            "Productos", "Clientes", "Proveedores", "Usuarios",
            "Ventas", "VentaDetalles", "Pagos",
            "Compras", "CompraDetalles",
            "MovimientosStock", "MovimientosCaja", "AuditoriaLogs"
        };
        long totalRows = 0;
        foreach (var t in tables)
        {
            var count = (long)ExecScalar(conn, $"SELECT COUNT(*) FROM {t}");
            totalRows += count;
            Console.WriteLine($"  {t,-25} {count,12:N0} filas");
        }
        Console.WriteLine($"  {"TOTAL",-25} {totalRows,12:N0} filas");
        Console.WriteLine();

        // ════════════════════════════════════════════════════
        //  FASE 3: CONSULTAS CON ÍNDICES
        // ════════════════════════════════════════════════════
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine("  FASE 3: CONSULTAS CON ÍNDICES");
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        BenchmarkConsultas(conn);

        // ════════════════════════════════════════════════════
        //  FASE 4: CONSULTAS SIN ÍNDICES (para comparar)
        // ════════════════════════════════════════════════════
        Console.WriteLine();
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine("  FASE 4: BENCHMARK CONCURRENCIA (5 threads × 10s)");
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        BenchmarkConcurrencia();

        // ════════════════════════════════════════════════════
        //  RESUMEN FINAL
        // ════════════════════════════════════════════════════
        Console.WriteLine();
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine("  RESUMEN FINAL");
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine($"  Total filas:         {totalRows:N0}");
        Console.WriteLine($"  Tamaño DB:           {totalSize / 1024.0 / 1024.0:N1} MB");
        Console.WriteLine($"  Tiempo total:        {swTotal.Elapsed.TotalSeconds:N1} s");
        Console.WriteLine();
        Console.WriteLine("  LÍMITES SQLITE:");
        Console.WriteLine($"  DB size:             {totalSize / 1024.0 / 1024.0:N1} MB / 281,474 GB (teórico)");
        Console.WriteLine($"  Filas:               {totalRows:N0} / 2,814,749,767,106,559 (teórico)");
        Console.WriteLine($"  % del límite filas:  {totalRows / 2_814_749_767_106_559.0 * 100:N10}%");
        Console.WriteLine("═══════════════════════════════════════════════════════════");

        conn.Close();
        TryDelete(DbPath);
        TryDelete(DbPath + "-wal");
        TryDelete(DbPath + "-shm");

        Console.WriteLine();
        Console.WriteLine("Presioná cualquier tecla para salir...");
        Console.ReadKey();
    }

    // ════════════════════════════════════════════════════════
    //  SCHEMA COMPLETO CON ÍNDICES
    // ════════════════════════════════════════════════════════
    static void CreateSchema(SqliteConnection conn)
    {
        Exec(conn, @"
            -- Empresas
            CREATE TABLE Empresas (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL, Cuit TEXT, Direccion TEXT, Telefono TEXT, Email TEXT
            );
            INSERT INTO Empresas (Nombre, Cuit) VALUES ('Empresa Stress Test', '30-71234567-9');

            -- Sucursales
            CREATE TABLE Sucursales (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL, Id_empresa INTEGER, Direccion TEXT, Activo INTEGER DEFAULT 1
            );
            INSERT INTO Sucursales (Nombre, Id_empresa) VALUES ('Sucursal Central', 1);

            -- Roles y Permisos
            CREATE TABLE Roles (Id INTEGER PRIMARY KEY AUTOINCREMENT, Nombre TEXT NOT NULL);
            CREATE TABLE Permisos (Id INTEGER PRIMARY KEY AUTOINCREMENT, Codigo TEXT NOT NULL, Descripcion TEXT);
            CREATE TABLE RolPermisos (Id_rol INTEGER, Id_permiso INTEGER);
            INSERT INTO Roles (Nombre) VALUES ('Admin'), ('Vendedor'), ('Gerente');

            -- Usuarios
            CREATE TABLE Usuarios (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL, Email TEXT, PasswordHash TEXT,
                Id_rol INTEGER, Id_empresa INTEGER, Id_sucursal INTEGER, Activo INTEGER DEFAULT 1
            );
            CREATE INDEX IX_Usuario_Empresa ON Usuarios(Id_empresa);
            CREATE INDEX IX_Usuario_Email ON Usuarios(Email);

            -- Categorías y Unidades
            CREATE TABLE Categorias (Id INTEGER PRIMARY KEY AUTOINCREMENT, Nombre TEXT NOT NULL);
            CREATE TABLE UnidadesMedida (Id INTEGER PRIMARY KEY AUTOINCREMENT, Nombre TEXT NOT NULL, Abreviatura TEXT);
            INSERT INTO Categorias (Nombre) VALUES ('Alimentos'),('Bebidas'),('Limpieza'),('Electronica'),('Ropa'),('Hogar'),('Deportes'),('Otros');
            INSERT INTO UnidadesMedida (Nombre, Abreviatura) VALUES ('Unidad','UN'),('Kilogramo','KG'),('Litro','L'),('Caja','CAJ'),('Paquete','PAQ');

            -- Productos
            CREATE TABLE Productos (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL, CodigoBarra TEXT,
                PrecioVenta REAL DEFAULT 0, PrecioCosto REAL DEFAULT 0,
                StockActual INTEGER DEFAULT 0, StockMinimo INTEGER DEFAULT 0,
                Id_categoria INTEGER, Id_empresa INTEGER, Id_unidadMedida INTEGER,
                Activo INTEGER DEFAULT 1
            );
            CREATE INDEX IX_Prod_Empresa ON Productos(Id_empresa);
            CREATE INDEX IX_Prod_Categoria ON Productos(Id_categoria);
            CREATE INDEX IX_Prod_CodigoBarra ON Productos(CodigoBarra);
            CREATE INDEX IX_Prod_Nombre ON Productos(Nombre);
            CREATE INDEX IX_Prod_StockBajo ON Productos(StockActual, StockMinimo);

            -- Clientes
            CREATE TABLE Clientes (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL, Email TEXT, Telefono TEXT, Direccion TEXT,
                Id_empresa INTEGER, Activo INTEGER DEFAULT 1
            );
            CREATE INDEX IX_Cliente_Empresa ON Clientes(Id_empresa);
            CREATE INDEX IX_Cliente_Nombre ON Clientes(Nombre);

            -- Proveedores
            CREATE TABLE Proveedores (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL, Email TEXT, Telefono TEXT, Direccion TEXT, Id_empresa INTEGER
            );
            CREATE INDEX IX_Prov_Empresa ON Proveedores(Id_empresa);

            -- Proveedor-Producto-Costo
            CREATE TABLE ProveedorProductoCostos (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Id_proveedor INTEGER, Id_producto INTEGER, PrecioCosto REAL DEFAULT 0
            );
            CREATE INDEX IX_ProvProd_Proveedor ON ProveedorProductoCostos(Id_proveedor);
            CREATE INDEX IX_ProvProd_Producto ON ProveedorProductoCostos(Id_producto);

            -- Métodos de Pago
            CREATE TABLE MetodosPago (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL, Categoria TEXT, CuentaContable TEXT, Activo INTEGER DEFAULT 1
            );
            INSERT INTO MetodosPago (Nombre, Categoria) VALUES
                ('Efectivo','Efectivo'),('Tarjeta Credito','Tarjeta'),('Tarjeta Debito','Tarjeta'),('Transferencia','Transferencia');

            -- Tipos documento
            CREATE TABLE TiposDocumento (Id INTEGER PRIMARY KEY AUTOINCREMENT, Nombre TEXT NOT NULL);
            INSERT INTO TiposDocumento (Nombre) VALUES ('DNI'),('CUIT'),('CUIL'),('Pasaporte');

            -- TipoMovimientoStock
            CREATE TABLE TiposMovimientoStock (Id INTEGER PRIMARY KEY AUTOINCREMENT, Nombre TEXT NOT NULL);
            INSERT INTO TiposMovimientoStock (Nombre) VALUES ('Entrada'),('Salida'),('Ajuste'),('Devolucion');

            -- TipoMovimientoCaja
            CREATE TABLE TipoMovimientoCaja (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL, Tipo TEXT
            );
            INSERT INTO TipoMovimientoCaja (Nombre, Tipo) VALUES ('Apertura','Ingreso'),('Cierre','Egreso'),('Ingreso','Ingreso'),('Egreso','Egreso');

            -- ═══ VENTAS ═══
            CREATE TABLE Ventas (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Fecha TEXT NOT NULL, Total REAL DEFAULT 0, DescuentoTotal REAL DEFAULT 0,
                Id_cliente INTEGER, Id_empresa INTEGER, Id_usuario INTEGER,
                Id_sucursal INTEGER, Estado TEXT DEFAULT 'Completada', Observaciones TEXT
            );
            CREATE INDEX IX_Venta_Fecha ON Ventas(Fecha);
            CREATE INDEX IX_Venta_Empresa ON Ventas(Id_empresa);
            CREATE INDEX IX_Venta_Cliente ON Ventas(Id_cliente);
            CREATE INDEX IX_Venta_Estado ON Ventas(Estado);
            CREATE INDEX IX_Venta_FechaEmpresa ON Ventas(Fecha, Id_empresa);

            CREATE TABLE VentaDetalles (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Id_venta INTEGER, Id_producto INTEGER,
                Cantidad INTEGER DEFAULT 1, PrecioUnitario REAL DEFAULT 0,
                Subtotal REAL DEFAULT 0, Descuento REAL DEFAULT 0
            );
            CREATE INDEX IX_VDet_Venta ON VentaDetalles(Id_venta);
            CREATE INDEX IX_VDet_Producto ON VentaDetalles(Id_producto);
            CREATE INDEX IX_VDet_VentaProd ON VentaDetalles(Id_venta, Id_producto);

            CREATE TABLE VentaDetalleDescuentos (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Id_ventaDetalle INTEGER, Porcentaje REAL DEFAULT 0, Monto REAL DEFAULT 0
            );

            CREATE TABLE Pagos (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Id_venta INTEGER, Monto REAL DEFAULT 0, Id_metodoPago INTEGER
            );
            CREATE INDEX IX_Pago_Venta ON Pagos(Id_venta);
            CREATE INDEX IX_Pago_Metodo ON Pagos(Id_metodoPago);

            -- ═══ COMPRAS ═══
            CREATE TABLE Compras (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Fecha TEXT NOT NULL, Total REAL DEFAULT 0,
                Id_proveedor INTEGER, Id_empresa INTEGER, Id_usuario INTEGER,
                Estado TEXT DEFAULT 'Completada', NumeroFactura TEXT
            );
            CREATE INDEX IX_Compra_Fecha ON Compras(Fecha);
            CREATE INDEX IX_Compra_Empresa ON Compras(Id_empresa);
            CREATE INDEX IX_Compra_Proveedor ON Compras(Id_proveedor);

            CREATE TABLE CompraDetalles (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Id_compra INTEGER, Id_producto INTEGER,
                Cantidad INTEGER DEFAULT 1, PrecioCosto REAL DEFAULT 0, Subtotal REAL DEFAULT 0
            );
            CREATE INDEX IX_CDet_Compra ON CompraDetalles(Id_compra);
            CREATE INDEX IX_CDet_Producto ON CompraDetalles(Id_producto);

            -- ═══ INVENTARIO ═══
            CREATE TABLE MovimientosStock (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Fecha TEXT NOT NULL, Id_producto INTEGER, Id_tipoMovimiento INTEGER,
                Cantidad INTEGER DEFAULT 0, StockAnterior INTEGER DEFAULT 0, StockNuevo INTEGER DEFAULT 0,
                Id_empresa INTEGER, Observaciones TEXT
            );
            CREATE INDEX IX_MovStock_Fecha ON MovimientosStock(Fecha);
            CREATE INDEX IX_MovStock_Producto ON MovimientosStock(Id_producto);
            CREATE INDEX IX_MovStock_Empresa ON MovimientosStock(Id_empresa);
            CREATE INDEX IX_MovStock_FechaProd ON MovimientosStock(Fecha, Id_producto);

            -- ═══ CAJA ═══
            CREATE TABLE Cajas (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FechaApertura TEXT, FechaCierre TEXT, SaldoInicial REAL DEFAULT 0,
                SaldoFinal REAL DEFAULT 0, Estado TEXT DEFAULT 'Abierta',
                Id_empresa INTEGER, Id_usuario INTEGER, Id_sucursal INTEGER
            );
            CREATE INDEX IX_Caja_Empresa ON Cajas(Id_empresa);
            CREATE INDEX IX_Caja_Fecha ON Cajas(FechaApertura);

            CREATE TABLE MovimientosCaja (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Id_caja INTEGER, Fecha TEXT NOT NULL, Monto REAL DEFAULT 0,
                Id_tipoMovimiento INTEGER, Descripcion TEXT, Id_venta INTEGER
            );
            CREATE INDEX IX_MCaja_Caja ON MovimientosCaja(Id_caja);
            CREATE INDEX IX_MCaja_Fecha ON MovimientosCaja(Fecha);

            -- ═══ AUDITORÍA ═══
            CREATE TABLE AuditoriaLogs (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Fecha TEXT NOT NULL, Tabla TEXT, Id_registro INTEGER,
                Accion TEXT, ValoresAnteriores TEXT, ValoresNuevos TEXT,
                Id_usuario INTEGER, IpAddress TEXT
            );
            CREATE INDEX IX_Audit_Fecha ON AuditoriaLogs(Fecha);
            CREATE INDEX IX_Audit_Tabla ON AuditoriaLogs(Tabla);
            CREATE INDEX IX_Audit_Usuario ON AuditoriaLogs(Id_usuario);
            CREATE INDEX IX_Audit_TablaFecha ON AuditoriaLogs(Tabla, Fecha);

            -- ═══ DESCUENTOS ═══
            CREATE TABLE DescuentoConfiguraciones (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL, Porcentaje REAL DEFAULT 0, MontoFijo REAL DEFAULT 0,
                Id_categoria INTEGER, Id_empresa INTEGER, Activo INTEGER DEFAULT 1,
                FechaDesde TEXT, FechaHasta TEXT
            );
            CREATE TABLE DescuentoMetodosPago (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Id_descuento INTEGER, Id_metodoPago INTEGER
            );

            -- ═══ CONFIGURACIÓN ═══
            CREATE TABLE TablaAuditadas (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT NOT NULL, Activa INTEGER DEFAULT 1
            );
        ");
    }

    // ════════════════════════════════════════════════════════
    //  INSERT MASIVOS
    // ════════════════════════════════════════════════════════
    static void InsertProductos(SqliteConnection conn, int hasta)
    {
        using var tx = conn.BeginTransaction();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Productos (Nombre, CodigoBarra, PrecioVenta, PrecioCosto, StockActual, StockMinimo, Id_categoria, Id_empresa, Id_unidadMedida, Activo) 
                            VALUES ($nom, $cod, $pv, $pc, $sa, $sm, $cat, 1, $um, 1)";
        var pNom = cmd.Parameters.Add("$nom", SqliteType.Text);
        var pCod = cmd.Parameters.Add("$cod", SqliteType.Text);
        var pPv = cmd.Parameters.Add("$pv", SqliteType.Real);
        var pPc = cmd.Parameters.Add("$pc", SqliteType.Real);
        var pSa = cmd.Parameters.Add("$sa", SqliteType.Integer);
        var pSm = cmd.Parameters.Add("$sm", SqliteType.Integer);
        var pCat = cmd.Parameters.Add("$cat", SqliteType.Integer);
        var pUm = cmd.Parameters.Add("$um", SqliteType.Integer);

        var rand = new Random(42);
        var cats = new[] { "Alimentos", "Bebidas", "Limpieza", "Electronica", "Ropa", "Hogar", "Deportes", "Otros" };

        for (int i = 1; i <= hasta; i++)
        {
            pNom.Value = $"Producto {i} - {cats[i % cats.Length]}";
            pCod.Value = $"789{i:D8}";
            pPv.Value = Math.Round((decimal)(rand.NextDouble() * 500 + 1), 2);
            pPc.Value = Math.Round((decimal)(rand.NextDouble() * 200 + 0.5), 2);
            pSa.Value = rand.Next(0, 500);
            pSm.Value = rand.Next(5, 20);
            pCat.Value = (i % cats.Length) + 1;
            pUm.Value = (i % 5) + 1;
            cmd.ExecuteNonQuery();
        }
        tx.Commit();
    }

    static void InsertClientes(SqliteConnection conn, int hasta)
    {
        using var tx = conn.BeginTransaction();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Clientes (Nombre, Email, Telefono, Id_empresa, Activo) VALUES ($nom, $email, $tel, 1, 1)";
        var pNom = cmd.Parameters.Add("$nom", SqliteType.Text);
        var pEmail = cmd.Parameters.Add("$email", SqliteType.Text);
        var pTel = cmd.Parameters.Add("$tel", SqliteType.Text);

        for (int i = 1; i <= hasta; i++)
        {
            pNom.Value = $"Cliente {i}";
            pEmail.Value = $"cliente{i}@test.com";
            pTel.Value = $"11{i:D8}";
            cmd.ExecuteNonQuery();
        }
        tx.Commit();
    }

    static void InsertProveedores(SqliteConnection conn, int hasta)
    {
        using var tx = conn.BeginTransaction();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Proveedores (Nombre, Email, Telefono, Id_empresa) VALUES ($nom, $email, $tel, 1)";
        var pNom = cmd.Parameters.Add("$nom", SqliteType.Text);
        var pEmail = cmd.Parameters.Add("$email", SqliteType.Text);
        var pTel = cmd.Parameters.Add("$tel", SqliteType.Text);

        for (int i = 1; i <= hasta; i++)
        {
            pNom.Value = $"Proveedor {i}";
            pEmail.Value = $"prov{i}@test.com";
            pTel.Value = $"11{i:D8}";
            cmd.ExecuteNonQuery();
        }
        tx.Commit();
    }

    static void InsertUsuarios(SqliteConnection conn, int hasta)
    {
        using var tx = conn.BeginTransaction();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO Usuarios (Nombre, Email, Id_rol, Id_empresa, Id_sucursal, Activo) VALUES ($nom, $email, $rol, 1, 1, 1)";
        var pNom = cmd.Parameters.Add("$nom", SqliteType.Text);
        var pEmail = cmd.Parameters.Add("$email", SqliteType.Text);
        var pRol = cmd.Parameters.Add("$rol", SqliteType.Integer);

        for (int i = 1; i <= hasta; i++)
        {
            pNom.Value = $"Usuario {i}";
            pEmail.Value = $"user{i}@test.com";
            pRol.Value = (i % 3) + 1;
            cmd.ExecuteNonQuery();
        }
        tx.Commit();
    }

    static void InsertCompras(SqliteConnection conn, int total)
    {
        var rand = new Random(42);
        var totalProd = (int)(long)ExecScalar(conn, "SELECT COUNT(*) FROM Productos");

        Exec(conn, "PRAGMA foreign_keys = OFF;");
        using var tx = conn.BeginTransaction();

        using var cmdC = conn.CreateCommand();
        cmdC.CommandText = @"INSERT INTO Compras (Fecha, Total, Id_proveedor, Id_empresa, Id_usuario, Estado, NumeroFactura) 
                             VALUES ($fec, $tot, $prov, 1, 1, 'Completada', $fac)";
        var pFec = cmdC.Parameters.Add("$fec", SqliteType.Text);
        var pTot = cmdC.Parameters.Add("$tot", SqliteType.Real);
        var pProv = cmdC.Parameters.Add("$prov", SqliteType.Integer);
        var pFac = cmdC.Parameters.Add("$fac", SqliteType.Text);

        using var cmdD = conn.CreateCommand();
        cmdD.CommandText = @"INSERT INTO CompraDetalles (Id_compra, Id_producto, Cantidad, PrecioCosto, Subtotal) 
                             VALUES ($cid, $pid, $cant, $pc, $sub)";
        var pCid = cmdD.Parameters.Add("$cid", SqliteType.Integer);
        var pPid = cmdD.Parameters.Add("$pid", SqliteType.Integer);
        var pCant = cmdD.Parameters.Add("$cant", SqliteType.Integer);
        var pPc = cmdD.Parameters.Add("$pc", SqliteType.Real);
        var pSub = cmdD.Parameters.Add("$sub", SqliteType.Real);

        for (int i = 1; i <= total; i++)
        {
            pFec.Value = DateTime.Now.AddDays(-rand.Next(0, 730)).ToString("yyyy-MM-dd HH:mm:ss");
            int numDet = rand.Next(1, 6);
            decimal totalC = 0;

            var dets = new (int pid, int cant, decimal pc)[numDet];
            for (int d = 0; d < numDet; d++)
            {
                int cant = rand.Next(10, 200);
                decimal pc = Math.Round((decimal)(rand.NextDouble() * 200 + 1), 2);
                totalC += cant * pc;
                dets[d] = (rand.Next(1, totalProd + 1), cant, pc);
            }

            pTot.Value = totalC;
            pProv.Value = rand.Next(1, 5001);
            pFac.Value = $"FC-{i:D8}";
            cmdC.ExecuteNonQuery();
            long cid = (long)ExecScalar(conn, "SELECT last_insert_rowid()");

            foreach (var d in dets)
            {
                pCid.Value = cid; pPid.Value = d.pid; pCant.Value = d.cant;
                pPc.Value = d.pc; pSub.Value = d.cant * d.pc;
                cmdD.ExecuteNonQuery();
            }
        }
        tx.Commit();
        Exec(conn, "PRAGMA foreign_keys = ON;");
    }

    static void InsertVentas(SqliteConnection conn, int totalVentas)
    {
        var existentes = (int)(long)ExecScalar(conn, "SELECT COUNT(*) FROM Ventas");
        if (totalVentas <= existentes) return;

        var rand = new Random(42);
        var totalProd = (int)(long)ExecScalar(conn, "SELECT COUNT(*) FROM Productos");
        var totalCli = (int)(long)ExecScalar(conn, "SELECT COUNT(*) FROM Clientes");

        Exec(conn, "PRAGMA foreign_keys = OFF;");
        using var tx = conn.BeginTransaction();

        using var cmdV = conn.CreateCommand();
        cmdV.CommandText = @"INSERT INTO Ventas (Fecha, Total, DescuentoTotal, Id_cliente, Id_empresa, Id_usuario, Estado) 
                             VALUES ($fec, $tot, 0, $cli, 1, 1, 'Completada')";
        var pFec = cmdV.Parameters.Add("$fec", SqliteType.Text);
        var pTot = cmdV.Parameters.Add("$tot", SqliteType.Real);
        var pCli = cmdV.Parameters.Add("$cli", SqliteType.Integer);

        using var cmdD = conn.CreateCommand();
        cmdD.CommandText = @"INSERT INTO VentaDetalles (Id_venta, Id_producto, Cantidad, PrecioUnitario, Subtotal, Descuento) 
                             VALUES ($vid, $pid, $cant, $pu, $sub, 0)";
        var pVid = cmdD.Parameters.Add("$vid", SqliteType.Integer);
        var pPid = cmdD.Parameters.Add("$pid", SqliteType.Integer);
        var pCant = cmdD.Parameters.Add("$cant", SqliteType.Integer);
        var pPu = cmdD.Parameters.Add("$pu", SqliteType.Real);
        var pSub = cmdD.Parameters.Add("$sub", SqliteType.Real);

        using var cmdP = conn.CreateCommand();
        cmdP.CommandText = @"INSERT INTO Pagos (Id_venta, Monto, Id_metodoPago) VALUES ($vid, $mont, $met)";
        var pVid2 = cmdP.Parameters.Add("$vid", SqliteType.Integer);
        var pMont = cmdP.Parameters.Add("$mont", SqliteType.Real);
        var pMet = cmdP.Parameters.Add("$met", SqliteType.Integer);

        for (int i = existentes + 1; i <= totalVentas; i++)
        {
            pFec.Value = DateTime.Now.AddDays(-rand.Next(0, 730)).ToString("yyyy-MM-dd HH:mm:ss");

            int numDet = rand.Next(1, 6);
            decimal totalVenta = 0;
            var dets = new (int pid, int cant, decimal pu)[numDet];
            for (int d = 0; d < numDet; d++)
            {
                int cant = rand.Next(1, 10);
                decimal pu = Math.Round((decimal)(rand.NextDouble() * 500 + 1), 2);
                totalVenta += cant * pu;
                dets[d] = (rand.Next(1, totalProd + 1), cant, pu);
            }

            pTot.Value = totalVenta;
            pCli.Value = totalCli > 0 ? rand.Next(1, totalCli + 1) : (object)DBNull.Value;
            cmdV.ExecuteNonQuery();
            long vid = (long)ExecScalar(conn, "SELECT last_insert_rowid()");

            foreach (var d in dets)
            {
                pVid.Value = vid; pPid.Value = d.pid; pCant.Value = d.cant;
                pPu.Value = d.pu; pSub.Value = d.cant * d.pu;
                cmdD.ExecuteNonQuery();
            }

            int numPagos = rand.Next(1, 4);
            decimal restante = totalVenta;
            for (int p = 0; p < numPagos; p++)
            {
                decimal monto = p == numPagos - 1 ? restante : Math.Round(restante / (numPagos - p), 2);
                pVid2.Value = vid; pMont.Value = monto; pMet.Value = rand.Next(1, 5);
                cmdP.ExecuteNonQuery();
                restante -= monto;
            }
        }
        tx.Commit();
        Exec(conn, "PRAGMA foreign_keys = ON;");
    }

    static void InsertMovimientosStock(SqliteConnection conn, int total)
    {
        var rand = new Random(42);
        var totalProd = (int)(long)ExecScalar(conn, "SELECT COUNT(*) FROM Productos");

        using var tx = conn.BeginTransaction();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO MovimientosStock (Fecha, Id_producto, Id_tipoMovimiento, Cantidad, StockAnterior, StockNuevo, Id_empresa, Observaciones) 
                            VALUES ($fec, $prod, $tipo, $cant, $sa, $sn, 1, $obs)";
        var pFec = cmd.Parameters.Add("$fec", SqliteType.Text);
        var pProd = cmd.Parameters.Add("$prod", SqliteType.Integer);
        var pTipo = cmd.Parameters.Add("$tipo", SqliteType.Integer);
        var pCant = cmd.Parameters.Add("$cant", SqliteType.Integer);
        var pSa = cmd.Parameters.Add("$sa", SqliteType.Integer);
        var pSn = cmd.Parameters.Add("$sn", SqliteType.Integer);
        var pObs = cmd.Parameters.Add("$obs", SqliteType.Text);

        for (int i = 1; i <= total; i++)
        {
            pFec.Value = DateTime.Now.AddDays(-rand.Next(0, 365)).ToString("yyyy-MM-dd HH:mm:ss");
            pProd.Value = rand.Next(1, totalProd + 1);
            pTipo.Value = rand.Next(1, 5);
            int cant = rand.Next(1, 100);
            int sa = rand.Next(0, 500);
            pCant.Value = cant;
            pSa.Value = sa;
            pSn.Value = sa + cant;
            pObs.Value = $"Movimiento {i}";
            cmd.ExecuteNonQuery();
        }
        tx.Commit();
    }

    static void InsertMovimientosCaja(SqliteConnection conn, int total)
    {
        var rand = new Random(42);

        // Crear 1000 cajas
        Exec(conn, "PRAGMA foreign_keys = OFF;");
        using (var txC = conn.BeginTransaction())
        {
            for (int i = 1; i <= 1000; i++)
            {
                Exec(conn, $"INSERT INTO Cajas (FechaApertura, SaldoInicial, Estado, Id_empresa, Id_usuario, Id_sucursal) VALUES ('{DateTime.Now.AddDays(-rand.Next(0, 365)):yyyy-MM-dd HH:mm:ss}', {rand.Next(1000, 50000)}, 'Cerrada', 1, 1, 1)");
            }
            txC.Commit();
        }

        using var tx = conn.BeginTransaction();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO MovimientosCaja (Id_caja, Fecha, Monto, Id_tipoMovimiento, Descripcion, Id_venta) 
                            VALUES ($caj, $fec, $mont, $tipo, $desc, $vent)";
        var pCaj = cmd.Parameters.Add("$caj", SqliteType.Integer);
        var pFec = cmd.Parameters.Add("$fec", SqliteType.Text);
        var pMont = cmd.Parameters.Add("$mont", SqliteType.Real);
        var pTipo = cmd.Parameters.Add("$tipo", SqliteType.Integer);
        var pDesc = cmd.Parameters.Add("$desc", SqliteType.Text);
        var pVent = cmd.Parameters.Add("$vent", SqliteType.Integer);

        for (int i = 1; i <= total; i++)
        {
            pCaj.Value = rand.Next(1, 1001);
            pFec.Value = DateTime.Now.AddDays(-rand.Next(0, 365)).ToString("yyyy-MM-dd HH:mm:ss");
            pMont.Value = Math.Round((decimal)(rand.NextDouble() * 10000), 2);
            pTipo.Value = rand.Next(1, 5);
            pDesc.Value = $"Movimiento caja {i}";
            pVent.Value = DBNull.Value;
            cmd.ExecuteNonQuery();
        }
        tx.Commit();
        Exec(conn, "PRAGMA foreign_keys = ON;");
    }

    static void InsertAuditoria(SqliteConnection conn, int total)
    {
        var rand = new Random(42);
        var tablas = new[] { "Productos", "Ventas", "Clientes", "Compras", "Usuarios", "Caja" };
        var acciones = new[] { "INSERT", "UPDATE", "DELETE" };

        using var tx = conn.BeginTransaction();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT INTO AuditoriaLogs (Fecha, Tabla, Id_registro, Accion, ValoresNuevos, Id_usuario) 
                            VALUES ($fec, $tbl, $reg, $acc, $val, $usr)";
        var pFec = cmd.Parameters.Add("$fec", SqliteType.Text);
        var pTbl = cmd.Parameters.Add("$tbl", SqliteType.Text);
        var pReg = cmd.Parameters.Add("$reg", SqliteType.Integer);
        var pAcc = cmd.Parameters.Add("$acc", SqliteType.Text);
        var pVal = cmd.Parameters.Add("$val", SqliteType.Text);
        var pUsr = cmd.Parameters.Add("$usr", SqliteType.Integer);

        for (int i = 1; i <= total; i++)
        {
            pFec.Value = DateTime.Now.AddDays(-rand.Next(0, 365)).ToString("yyyy-MM-dd HH:mm:ss");
            pTbl.Value = tablas[rand.Next(tablas.Length)];
            pReg.Value = rand.Next(1, 100000);
            pAcc.Value = acciones[rand.Next(acciones.Length)];
            pVal.Value = $"{{\"campo\":\"valor_{i}\"}}";
            pUsr.Value = rand.Next(1, 101);
            cmd.ExecuteNonQuery();
        }
        tx.Commit();
    }

    // ════════════════════════════════════════════════════════
    //  BENCHMARK CONSULTAS
    // ════════════════════════════════════════════════════════
    static void BenchmarkConsultas(SqliteConnection conn)
    {
        int N = 100;
        var queries = new (string label, string sql)[]
        {
            ("Búsqueda por código de barras", "SELECT * FROM Productos WHERE CodigoBarra = '78900000042' LIMIT 1"),
            ("Listado productos (50/pág)", "SELECT * FROM Productos WHERE Id_empresa = 1 ORDER BY Nombre LIMIT 50"),
            ("Stock bajo", "SELECT COUNT(*) FROM Productos WHERE StockActual <= StockMinimo AND Id_empresa = 1"),
            ("Búsqueda por nombre (LIKE)", "SELECT * FROM Productos WHERE Nombre LIKE '%Producto 50000%' LIMIT 10"),
            ("Detalle venta con JOIN", "SELECT vd.*, p.Nombre FROM VentaDetalles vd JOIN Productos p ON vd.Id_producto = p.Id WHERE vd.Id_venta = 1000"),
            ("Ventas del día", $"SELECT COUNT(*), SUM(Total) FROM Ventas WHERE Fecha >= '{DateTime.Now:yyyy-MM-dd}' AND Id_empresa = 1"),
            ("Top 10 más vendidos", "SELECT p.Nombre, SUM(vd.Cantidad) as TV FROM VentaDetalles vd JOIN Productos p ON vd.Id_producto = p.Id GROUP BY p.Id ORDER BY TV DESC LIMIT 10"),
            ("Reporte mensual", "SELECT strftime('%Y-%m', Fecha), COUNT(*), SUM(Total) FROM Ventas WHERE Id_empresa=1 GROUP BY 1 ORDER BY 1 DESC LIMIT 12"),
            ("Compras por proveedor", "SELECT pr.Nombre, COUNT(c.Id), SUM(c.Total) FROM Compras c JOIN Proveedores pr ON c.Id_proveedor=pr.Id GROUP BY pr.Id ORDER BY 3 DESC LIMIT 10"),
            ("Inventario completo", "SELECT p.Nombre, p.StockActual, p.StockMinimo, c.Nombre as Cat FROM Productos p JOIN Categorias c ON p.Id_categoria=c.Id WHERE p.Id_empresa=1 ORDER BY p.StockActual LIMIT 50"),
            ("Cliente por email", "SELECT * FROM Clientes WHERE Email = 'cliente25000@test.com'"),
            ("Auditoría reciente", "SELECT * FROM AuditoriaLogs WHERE Tabla = 'Ventas' ORDER BY Fecha DESC LIMIT 50"),
            ("Movimientos stock (producto)", "SELECT * FROM MovimientosStock WHERE Id_producto = 500 ORDER BY Fecha DESC LIMIT 20"),
            ("Caja del día", "SELECT * FROM Cajas WHERE FechaApertura >= '2026-01-01' AND Id_empresa = 1"),
        };

        foreach (var (label, sql) in queries)
        {
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < N; i++) Exec(conn, sql);
            sw.Stop();
            Console.WriteLine($"  {label,-40} {sw.ElapsedMilliseconds / (double)N,7:F2} ms/query");
        }
    }

    // ════════════════════════════════════════════════════════
    //  BENCHMARK CONCURRENCIA
    // ════════════════════════════════════════════════════════
    static void BenchmarkConcurrencia()
    {
        var dbPath = "StressTestConc.db";
        if (System.IO.File.Exists(dbPath)) System.IO.File.Delete(dbPath);

        using (var c = new SqliteConnection($"Data Source={dbPath}"))
        {
            c.Open();
            Exec(c, "PRAGMA journal_mode = WAL; PRAGMA synchronous = NORMAL;");
            Exec(c, @"CREATE TABLE Ventas (Id INTEGER PRIMARY KEY AUTOINCREMENT, Fecha TEXT, Total REAL, Id_empresa INTEGER);
                      CREATE TABLE VentaDetalles (Id INTEGER PRIMARY KEY AUTOINCREMENT, Id_venta INTEGER, Id_producto INTEGER, Cantidad INTEGER, Subtotal REAL);
                      CREATE TABLE Productos (Id INTEGER PRIMARY KEY AUTOINCREMENT, Nombre TEXT, StockActual INTEGER);");
            for (int i = 1; i <= 10_000; i++)
                Exec(c, $"INSERT INTO Productos (Nombre, StockActual) VALUES ('P{i}', {i % 300})");
        }

        var sw = Stopwatch.StartNew();
        int total = 0;
        object lockObj = new();

        var threads = new System.Threading.Thread[5];
        for (int t = 0; t < 5; t++)
        {
            threads[t] = new System.Threading.Thread(() =>
            {
                using var tc = new SqliteConnection($"Data Source={dbPath}");
                tc.Open();
                Exec(tc, "PRAGMA journal_mode = WAL;");
                var tsw = Stopwatch.StartNew();
                int local = 0;
                var rand = new Random();
                while (tsw.ElapsedMilliseconds < 10_000)
                {
                    try
                    {
                        Exec(tc, $"INSERT INTO Ventas (Fecha, Total, Id_empresa) VALUES ('{DateTime.Now:yyyy-MM-dd HH:mm:ss}', {100 + local}, 1)");
                        long vid = (long)ExecScalar(tc, "SELECT last_insert_rowid()");
                        int det = rand.Next(1, 4);
                        for (int d = 0; d < det; d++)
                            Exec(tc, $"INSERT INTO VentaDetalles (Id_venta, Id_producto, Cantidad, Subtotal) VALUES ({vid}, {rand.Next(1, 10001)}, {rand.Next(1, 10)}, {rand.Next(10, 5000)})");
                        local++;
                    }
                    catch { break; }
                }
                lock (lockObj) { total += local; }
            });
            threads[t].Start();
        }
        foreach (var t in threads) t.Join();
        sw.Stop();

        Console.WriteLine($"  5 threads × 10s:    {total:N0} ventas (con detalles)");
        Console.WriteLine($"  Throughput:          {total / sw.Elapsed.TotalSeconds:F0} ventas/segundo");
        Console.WriteLine($"  Latencia promedio:   {sw.Elapsed.TotalMilliseconds / total:F2} ms/venta");

        Thread.Sleep(1000); // Esperar a que SQLite libere WAL
        TryDelete(dbPath);
        TryDelete(dbPath + "-wal");
        TryDelete(dbPath + "-shm");
    }

    // ════════════════════════════════════════════════════════
    //  HELPERS
    // ════════════════════════════════════════════════════════
    static void Measure(string label, Action action)
    {
        var sw = Stopwatch.StartNew();
        action();
        sw.Stop();
        var size = new System.IO.FileInfo(DbPath).Length / 1024.0 / 1024.0;
        Console.WriteLine($"  {label,-35} {sw.ElapsedMilliseconds,7} ms | DB: {size:N1} MB");
    }

    static void Exec(SqliteConnection c, string sql)
    {
        using var cmd = c.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }

    static object ExecScalar(SqliteConnection c, string sql)
    {
        using var cmd = c.CreateCommand();
        cmd.CommandText = sql;
        return cmd.ExecuteScalar()!;
    }

    static void TryDelete(string path)
    {
        try { if (System.IO.File.Exists(path)) System.IO.File.Delete(path); } catch { }
    }
}
