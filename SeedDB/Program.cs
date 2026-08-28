using System;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using GestionComercial.Persistencia.Contexto;

class Program
{
    static string DbPath = null!;

    static void Main(string[] args)
    {
        DbPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "GestionComercial.UI", "GestionComercial.db"));
        if (!File.Exists(DbPath))
        {
            var alts = new[] { @"C:\Users\Usuario\Desktop\GestionComercial\GestionComercial.UI\GestionComercial.db", "GestionComercial.db" };
            foreach (var a in alts) if (File.Exists(a)) { DbPath = a; break; }
        }
        if (!File.Exists(DbPath)) { Console.WriteLine("DB no encontrada"); return; }

        Console.WriteLine($"DB: {DbPath}");
        Console.WriteLine($"Tamaño: {new FileInfo(DbPath).Length / 1024.0 / 1024.0:N1} MB\n");

        var connStr = $"Data Source={DbPath}";

        // 1. Conectar primero
        using var conn = new SqliteConnection(connStr);
        conn.Open();

        // 2. Crear schema con EF Core
        Console.Write("Creando schema... ");
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var options = new DbContextOptionsBuilder<GestionComercialContext>().UseSqlite(connStr).Options;
        using (var ctx = new GestionComercialContext(options)) ctx.Database.EnsureCreated();
        Console.WriteLine($"{sw.ElapsedMilliseconds} ms ✓\n");
        Exec(conn, "PRAGMA journal_mode = WAL;");
        Exec(conn, "PRAGMA synchronous = OFF;");
        Exec(conn, "PRAGMA foreign_keys = OFF;");

        Console.WriteLine("═══ CARGANDO DATOS ═══");

        // Obtener/crear empresa
        var idEmp = Scalar(conn, "SELECT Id FROM Empresa LIMIT 1");
        if (idEmp == null)
        {
            Exec(conn, "INSERT INTO Empresa (Nombre, CUIT, Direccion, Telefono, Email, FechaAlta) VALUES ('Mi Empresa SA', '30-71234567-9', 'Av. Principal 1234', '11-5555-5555', 'empresa@test.com', datetime('now'))");
            idEmp = Scalar(conn, "SELECT Id FROM Empresa LIMIT 1");
        }
        long empId = (long)idEmp!;

        // Obtener/crear sucursal
        var idSuc = Scalar(conn, "SELECT Id FROM Sucursal LIMIT 1");
        if (idSuc == null)
        {
            Exec(conn, $"INSERT INTO Sucursal (Nombre, Direccion, Id_empresa, FechaAlta) VALUES ('Sucursal Central', 'Av. Principal 1234', {empId}, datetime('now'))");
            idSuc = Scalar(conn, "SELECT Id FROM Sucursal LIMIT 1");
        }
        long sucId = (long)idSuc!;

        // Obtener usuario
        var idUsr = Scalar(conn, "SELECT Id FROM Usuario LIMIT 1");
        long usrId = idUsr != null ? (long)idUsr : 1;

        // Seeds base
        var cats = Scalar(conn, "SELECT COUNT(*) FROM Categoria");
        if ((long)cats == 0) { Console.Write("  Categorías... "); SeedCategorias(conn); Console.WriteLine("✓"); }

        var ums = Scalar(conn, "SELECT COUNT(*) FROM UnidadMedida");
        if ((long)ums == 0) { Console.Write("  Unidades Medida... "); SeedUnidadesMedida(conn); Console.WriteLine("✓"); }

        var mps = Scalar(conn, "SELECT COUNT(*) FROM MetodoPago");
        if ((long)mps == 0) { Console.Write("  Métodos Pago... "); SeedMetodosPago(conn); Console.WriteLine("✓"); }

        var tms = Scalar(conn, "SELECT COUNT(*) FROM TipoMovimientoStock");
        if ((long)tms == 0) { Console.Write("  Tipos Movimiento... "); SeedTiposMovimiento(conn); Console.WriteLine("✓"); }

        // Seeds masivos
        if ((long)Scalar(conn, "SELECT COUNT(*) FROM Producto") < 5000)
        { Console.Write("  Productos (5,000)... "); var s = System.Diagnostics.Stopwatch.StartNew(); SeedProductos(conn, 5000, empId); Console.WriteLine($"{s.ElapsedMilliseconds} ms"); }

        if ((long)Scalar(conn, "SELECT COUNT(*) FROM Cliente") < 2000)
        { Console.Write("  Clientes (2,000)... "); var s = System.Diagnostics.Stopwatch.StartNew(); SeedClientes(conn, 2000, empId); Console.WriteLine($"{s.ElapsedMilliseconds} ms"); }

        if ((long)Scalar(conn, "SELECT COUNT(*) FROM Proveedor") < 200)
        { Console.Write("  Proveedores (200)... "); var s = System.Diagnostics.Stopwatch.StartNew(); SeedProveedores(conn, 200, empId); Console.WriteLine($"{s.ElapsedMilliseconds} ms"); }

        if ((long)Scalar(conn, "SELECT COUNT(*) FROM Venta") < 10_000)
        { Console.Write("  Ventas (10,000)... "); var s = System.Diagnostics.Stopwatch.StartNew(); SeedVentas(conn, 10_000, sucId, usrId); Console.WriteLine($"{s.ElapsedMilliseconds} ms"); }

        if ((long)Scalar(conn, "SELECT COUNT(*) FROM Compra") < 2000)
        { Console.Write("  Compras (2,000)... "); var s = System.Diagnostics.Stopwatch.StartNew(); SeedCompras(conn, 2000, sucId, usrId); Console.WriteLine($"{s.ElapsedMilliseconds} ms"); }

        if ((long)Scalar(conn, "SELECT COUNT(*) FROM MovimientoStock") < 5000)
        { Console.Write("  Movimientos Stock (5,000)... "); var s = System.Diagnostics.Stopwatch.StartNew(); SeedMovimientosStock(conn, 5000, sucId, usrId); Console.WriteLine($"{s.ElapsedMilliseconds} ms"); }

        // Cajas con movimientos
        if ((long)Scalar(conn, "SELECT COUNT(*) FROM Caja") < 50)
        { Console.Write("  Cajas (50)... "); var s = System.Diagnostics.Stopwatch.StartNew(); SeedCajas(conn, 50, sucId, usrId); Console.WriteLine($"{s.ElapsedMilliseconds} ms"); }

        // Descuentos
        if ((long)Scalar(conn, "SELECT COUNT(*) FROM DescuentoConfiguracion") < 30)
        { Console.Write("  Descuentos (30)... "); var s = System.Diagnostics.Stopwatch.StartNew(); SeedDescuentos(conn, 30, empId); Console.WriteLine($"{s.ElapsedMilliseconds} ms"); }

        Exec(conn, "PRAGMA foreign_keys = ON;");
        Exec(conn, "PRAGMA synchronous = NORMAL;");
        sw.Stop();

        // Resumen
        Console.WriteLine("\n═══════════════════════════════════════════");
        Console.WriteLine("  RESUMEN");
        Console.WriteLine("═══════════════════════════════════════════");
        Console.WriteLine($"  Categorías:         {Scalar(conn, "SELECT COUNT(*) FROM Categoria")}");
        Console.WriteLine($"  Unidades Medida:    {Scalar(conn, "SELECT COUNT(*) FROM UnidadMedida")}");
        Console.WriteLine($"  Métodos Pago:       {Scalar(conn, "SELECT COUNT(*) FROM MetodoPago")}");
        Console.WriteLine($"  Productos:          {Scalar(conn, "SELECT COUNT(*) FROM Producto"):N0}");
        Console.WriteLine($"  Clientes:           {Scalar(conn, "SELECT COUNT(*) FROM Cliente"):N0}");
        Console.WriteLine($"  Proveedores:        {Scalar(conn, "SELECT COUNT(*) FROM Proveedor"):N0}");
        Console.WriteLine($"  Ventas:             {Scalar(conn, "SELECT COUNT(*) FROM Venta"):N0}");
        Console.WriteLine($"  Detalles venta:     {Scalar(conn, "SELECT COUNT(*) FROM VentaDetalle"):N0}");
        Console.WriteLine($"  Pagos:              {Scalar(conn, "SELECT COUNT(*) FROM Pago"):N0}");
        Console.WriteLine($"  Compras:            {Scalar(conn, "SELECT COUNT(*) FROM Compra"):N0}");
        Console.WriteLine($"  Detalles compra:    {Scalar(conn, "SELECT COUNT(*) FROM CompraDetalle"):N0}");
        Console.WriteLine($"  Movimientos Stock:  {Scalar(conn, "SELECT COUNT(*) FROM MovimientoStock"):N0}");
        Console.WriteLine($"  Cajas:              {Scalar(conn, "SELECT COUNT(*) FROM Caja"):N0}");
        Console.WriteLine($"  Movimientos Caja:   {Scalar(conn, "SELECT COUNT(*) FROM MovimientoCaja"):N0}");
        Console.WriteLine($"  Descuentos:         {Scalar(conn, "SELECT COUNT(*) FROM DescuentoConfiguracion"):N0}");
        Console.WriteLine($"  Tamaño DB:          {new FileInfo(DbPath).Length / 1024.0 / 1024.0:N1} MB");
        Console.WriteLine($"  Tiempo total:       {sw.Elapsed.TotalSeconds:N1} s");
        Console.WriteLine("═══════════════════════════════════════════\n");
        Console.WriteLine("¡DB cargada! Abrí el sistema desde Visual Studio.");
    }


    // ════════════════════════════════════════════════════════
    //  SEED BASE
    // ════════════════════════════════════════════════════════
    static void SeedCategorias(SqliteConnection c)
    {
        var items = new[] { "Bebidas", "Alimentos", "Limpieza", "Electrónica", "Hogar", "Textil", "Deportes", "Juguetes", "Automotor", "Otros" };
        using var tx = c.BeginTransaction();
        foreach (var n in items) Exec(c, $"INSERT INTO Categoria (Nombre) VALUES ('{n}')");
        tx.Commit();
    }

    static void SeedUnidadesMedida(SqliteConnection c)
    {
        var items = new[] { ("Unidad", "UN"), ("Kilogramo", "KG"), ("Litro", "L"), ("Metro", "M"), ("Caja", "CJ"), ("Paquete", "PQ") };
        using var tx = c.BeginTransaction();
        foreach (var (n, a) in items) Exec(c, $"INSERT INTO UnidadMedida (Nombre, Abreviatura) VALUES ('{n}', '{a}')");
        tx.Commit();
    }

    static void SeedMetodosPago(SqliteConnection c)
    {
        var items = new[] { "Efectivo", "Tarjeta Crédito", "Tarjeta Débito", "Transferencia", "QR Mercado Pago", "Cuenta Corriente" };
        using var tx = c.BeginTransaction();
        foreach (var n in items) Exec(c, $"INSERT INTO MetodoPago (Nombre) VALUES ('{n}')");
        tx.Commit();
    }

    static void SeedTiposMovimiento(SqliteConnection c)
    {
        var items = new[] { "Entrada", "Salida", "Ajuste", "Devolución", "Transferencia" };
        using var tx = c.BeginTransaction();
        foreach (var n in items) Exec(c, $"INSERT INTO TipoMovimientoStock (Nombre) VALUES ('{n}')");
        tx.Commit();
    }

    // ════════════════════════════════════════════════════════
    //  SEED MASIVO
    // ════════════════════════════════════════════════════════
    static void SeedProductos(SqliteConnection c, int total, long empId)
    {
        var existentes = (int)(long)Scalar(c, "SELECT COUNT(*) FROM Producto");
        var rand = new Random(42);
        var nombres = new[] { "Leche", "Pan", "Aceite", "Arroz", "Fideos", "Azúcar", "Sal", "Café", "Yerba", "Gaseosa",
            "Agua", "Jugo", "Cerveza", "Vino", "Chocolate", "Galletitas", "Cereal", "Sopa", "Mayonesa", "Ketchup",
            "Manteca", "Queso", "Yogur", "Huevos", "Pollo", "Carne", "Cerdo", "Pescado", "Atún", "Sardina",
            "Shampoo", "Jabón", "Papel", "Detergente", "Lavandina", "Esponja", "Bolsa", "Fósforo", "Vela", "Cinta",
            "Taladro", "Tornillos", "Clavos", "Pintura", "Brocha", "Llave", "Martillo", "Sierra", "Cable", "Enchufe" };
        var catIds = GetIds(c, "SELECT Id FROM Categoria");
        var umIds = GetIds(c, "SELECT Id FROM UnidadMedida");

        using var tx = c.BeginTransaction();
        using var cmd = c.CreateCommand();
        cmd.CommandText = @"INSERT INTO Producto (Nombre, CodigoBarra, PrecioVentaActual, PrecioCostoActual, StockActual, StockMinimo, Id_categoria, Id_empresa, Id_unidadMedida, Activo, FechaAlta) VALUES ($nom,$cod,$pv,$pc,$sa,$sm,$cat,$emp,$um,1,$fec)";
        var pn = cmd.Parameters.Add("$nom", SqliteType.Text);
        var pc2 = cmd.Parameters.Add("$cod", SqliteType.Text);
        var ppv = cmd.Parameters.Add("$pv", SqliteType.Real);
        var ppc = cmd.Parameters.Add("$pc", SqliteType.Real);
        var psa = cmd.Parameters.Add("$sa", SqliteType.Integer);
        var psm = cmd.Parameters.Add("$sm", SqliteType.Integer);
        var pcat = cmd.Parameters.Add("$cat", SqliteType.Integer);
        var pemp = cmd.Parameters.Add("$emp", SqliteType.Integer);
        var pum = cmd.Parameters.Add("$um", SqliteType.Integer);
        var pfec = cmd.Parameters.Add("$fec", SqliteType.Text);

        for (int i = existentes + 1; i <= total; i++)
        {
            pn.Value = $"{nombres[i % nombres.Length]} {i}";
            pc2.Value = $"789{i:D8}";
            ppv.Value = Math.Round((decimal)(rand.NextDouble() * 500 + 5), 2);
            ppc.Value = Math.Round((decimal)(rand.NextDouble() * 200 + 1), 2);
            psa.Value = rand.Next(0, 200);
            psm.Value = rand.Next(5, 30);
            pcat.Value = catIds.Length > 0 ? catIds[rand.Next(catIds.Length)] : 1;
            pemp.Value = empId;
            pum.Value = umIds.Length > 0 ? umIds[rand.Next(umIds.Length)] : 1;
            pfec.Value = DateTime.Now.AddDays(-rand.Next(0, 365)).ToString("yyyy-MM-dd HH:mm:ss");
            cmd.ExecuteNonQuery();
        }
        tx.Commit();
    }

    static void SeedClientes(SqliteConnection c, int total, long empId)
    {
        var existentes = (int)(long)Scalar(c, "SELECT COUNT(*) FROM Cliente");
        var rand = new Random(42);
        var barrios = new[] { "Centro", "Norte", "Sur", "Este", "Oeste", "Pueblo Nuevo", "San Martín", "Belgrano", "Murillo", "San Justo" };

        using var tx = c.BeginTransaction();
        using var cmd = c.CreateCommand();
        cmd.CommandText = @"INSERT INTO Cliente (Nombre, Documento, Email, Telefono, Id_empresa, Activo, FechaAlta) VALUES ($nom,$doc,$email,$tel,$emp,1,$fec)";
        var pn = cmd.Parameters.Add("$nom", SqliteType.Text);
        var pdoc = cmd.Parameters.Add("$doc", SqliteType.Integer);
        var pe = cmd.Parameters.Add("$email", SqliteType.Text);
        var pt = cmd.Parameters.Add("$tel", SqliteType.Text);
        var pem = cmd.Parameters.Add("$emp", SqliteType.Integer);
        var pfec = cmd.Parameters.Add("$fec", SqliteType.Text);

        for (int i = existentes + 1; i <= total; i++)
        {
            pn.Value = $"Cliente {i}";
            pdoc.Value = 10000000 + i;
            pe.Value = $"cliente{i}@mail.com";
            pt.Value = $"11{rand.Next(4000, 5999)}-{rand.Next(1000, 9999)}";
            pem.Value = empId;
            pfec.Value = DateTime.Now.AddDays(-rand.Next(0, 365)).ToString("yyyy-MM-dd HH:mm:ss");
            cmd.ExecuteNonQuery();
        }
        tx.Commit();
    }

    static void SeedProveedores(SqliteConnection c, int total, long empId)
    {
        var existentes = (int)(long)Scalar(c, "SELECT COUNT(*) FROM Proveedor");
        var rand = new Random(42);
        var rubros = new[] { "Alimentos", "Bebidas", "Limpieza", "Electrónica", "Ropa", "Hogar" };

        using var tx = c.BeginTransaction();
        using var cmd = c.CreateCommand();
        cmd.CommandText = @"INSERT INTO Proveedor (Nombre, Email, Telefono, CUIT, Id_empresa, Activo, FechaAlta) VALUES ($nom,$email,$tel,$cuit,$emp,1,$fec)";
        var pn = cmd.Parameters.Add("$nom", SqliteType.Text);
        var pe = cmd.Parameters.Add("$email", SqliteType.Text);
        var pt = cmd.Parameters.Add("$tel", SqliteType.Text);
        var pc = cmd.Parameters.Add("$cuit", SqliteType.Text);
        var pem = cmd.Parameters.Add("$emp", SqliteType.Integer);
        var pfec = cmd.Parameters.Add("$fec", SqliteType.Text);

        for (int i = existentes + 1; i <= total; i++)
        {
            pn.Value = $"Proveedor {rubros[i % rubros.Length]} {i}";
            pe.Value = $"proveedor{i}@distribuidora.com";
            pt.Value = $"11{rand.Next(4000, 5999)}-{rand.Next(1000, 9999)}";
            pc.Value = $"30-{rand.Next(10000000, 99999999)}-{rand.Next(0, 9)}";
            pem.Value = empId;
            pfec.Value = DateTime.Now.AddDays(-rand.Next(0, 365)).ToString("yyyy-MM-dd HH:mm:ss");
            cmd.ExecuteNonQuery();
        }
        tx.Commit();
    }

    static void SeedVentas(SqliteConnection c, int total, long sucId, long usrId)
    {
        var existentes = (int)(long)Scalar(c, "SELECT COUNT(*) FROM Venta");
        var totalProd = (int)(long)Scalar(c, "SELECT COUNT(*) FROM Producto");
        var totalCli = (int)(long)Scalar(c, "SELECT COUNT(*) FROM Cliente");
        var rand = new Random(42);

        using var tx = c.BeginTransaction();
        using var cmdV = c.CreateCommand();
        cmdV.CommandText = @"INSERT INTO Venta (Fecha, TotalBruto, TotalDescuento, TotalFinal, Estado, Id_sucursal, Id_cliente, Id_usuario, Activo, FechaAlta) VALUES ($fec,$tb,$td,$tf,$est,$suc,$cli,$usr,1,$fal)";
        var pFec = cmdV.Parameters.Add("$fec", SqliteType.Text);
        var pTb = cmdV.Parameters.Add("$tb", SqliteType.Real);
        var pTd = cmdV.Parameters.Add("$td", SqliteType.Real);
        var pTf = cmdV.Parameters.Add("$tf", SqliteType.Real);
        var pEst = cmdV.Parameters.Add("$est", SqliteType.Integer);
        var pSuc = cmdV.Parameters.Add("$suc", SqliteType.Integer);
        var pCli = cmdV.Parameters.Add("$cli", SqliteType.Integer);
        var pUsr = cmdV.Parameters.Add("$usr", SqliteType.Integer);
        var pFal = cmdV.Parameters.Add("$fal", SqliteType.Text);

        using var cmdD = c.CreateCommand();
        cmdD.CommandText = @"INSERT INTO VentaDetalle (Id_venta, Id_producto, Cantidad, PrecioUnitario, CostoUnitario, Subtotal, Descuento, MargenUnitario) VALUES ($vid,$pid,$cant,$pu,$cu,$sub,$desc,$mu)";
        var pVid = cmdD.Parameters.Add("$vid", SqliteType.Integer);
        var pPid = cmdD.Parameters.Add("$pid", SqliteType.Integer);
        var pCant = cmdD.Parameters.Add("$cant", SqliteType.Real);
        var pPu = cmdD.Parameters.Add("$pu", SqliteType.Real);
        var pCu = cmdD.Parameters.Add("$cu", SqliteType.Real);
        var pSub = cmdD.Parameters.Add("$sub", SqliteType.Real);
        var pDesc = cmdD.Parameters.Add("$desc", SqliteType.Real);
        var pMu = cmdD.Parameters.Add("$mu", SqliteType.Real);

        using var cmdP = c.CreateCommand();
        cmdP.CommandText = @"INSERT INTO Pago (Id_venta, Monto, Id_metodoPago, Fecha) VALUES ($vid,$mont,$met,$fec)";
        var pVid2 = cmdP.Parameters.Add("$vid", SqliteType.Integer);
        var pMont = cmdP.Parameters.Add("$mont", SqliteType.Real);
        var pMet = cmdP.Parameters.Add("$met", SqliteType.Integer);
        var pPFec = cmdP.Parameters.Add("$fec", SqliteType.Text);

        for (int i = existentes + 1; i <= total; i++)
        {
            pFec.Value = DateTime.Now.AddDays(-rand.Next(0, 365)).AddHours(rand.Next(8, 22)).ToString("yyyy-MM-dd HH:mm:ss");
            int numDet = rand.Next(1, 6);
            decimal totalBruto = 0;
            var dets = new (int pid, int cant, decimal pu, decimal cu)[numDet];
            for (int d = 0; d < numDet; d++)
            {
                int cant = rand.Next(1, 8);
                decimal pu = Math.Round((decimal)(rand.NextDouble() * 500 + 5), 2);
                decimal cu = Math.Round(pu * (decimal)(rand.NextDouble() * 0.4 + 0.3), 2);
                totalBruto += cant * pu;
                dets[d] = (rand.Next(1, totalProd + 1), cant, pu, cu);
            }

            pTb.Value = Math.Round(totalBruto, 2);
            pTd.Value = 0m;
            pTf.Value = Math.Round(totalBruto, 2);
            pEst.Value = 2; // Pagada
            pSuc.Value = sucId;
            pCli.Value = totalCli > 0 ? (object)rand.Next(1, totalCli + 1) : DBNull.Value;
            pUsr.Value = usrId;
            pFal.Value = DateTime.Now.AddDays(-rand.Next(0, 365)).AddHours(rand.Next(8, 22)).ToString("yyyy-MM-dd HH:mm:ss");
            cmdV.ExecuteNonQuery();
            long vid = (long)Scalar(c, "SELECT last_insert_rowid()");

            foreach (var d in dets)
            {
                pVid.Value = vid; pPid.Value = d.pid; pCant.Value = d.cant;
                pPu.Value = d.pu; pCu.Value = d.cu;
                pSub.Value = Math.Round(d.cant * d.pu, 2);
                pDesc.Value = 0m;
                pMu.Value = Math.Round(d.pu - d.cu, 2);
                cmdD.ExecuteNonQuery();
            }

            int numPagos = rand.Next(1, 3);
            decimal restante = totalBruto;
            for (int p = 0; p < numPagos; p++)
            {
                decimal monto = p == numPagos - 1 ? restante : Math.Round(restante / (numPagos - p), 2);
                pVid2.Value = vid; pMont.Value = monto; pMet.Value = rand.Next(1, 7);
                pPFec.Value = DateTime.Now.AddDays(-rand.Next(0, 365)).ToString("yyyy-MM-dd HH:mm:ss");
                cmdP.ExecuteNonQuery();
                restante -= monto;
            }
        }
        tx.Commit();
    }

    static void SeedCompras(SqliteConnection c, int total, long sucId, long usrId)
    {
        var existentes = (int)(long)Scalar(c, "SELECT COUNT(*) FROM Compra");
        var totalProd = (int)(long)Scalar(c, "SELECT COUNT(*) FROM Producto");
        var totalProv = (int)(long)Scalar(c, "SELECT COUNT(*) FROM Proveedor");
        var rand = new Random(42);

        using var tx = c.BeginTransaction();
        using var cmdC = c.CreateCommand();
        cmdC.CommandText = @"INSERT INTO Compra (Fecha, Total, Estado, Id_proveedor, Id_sucursal, Id_usuario, Activo, FechaAlta) VALUES ($fec,$tot,$est,$prov,$suc,$usr,1,$fal)";
        var pFec = cmdC.Parameters.Add("$fec", SqliteType.Text);
        var pTot = cmdC.Parameters.Add("$tot", SqliteType.Real);
        var pEst = cmdC.Parameters.Add("$est", SqliteType.Integer);
        var pProv = cmdC.Parameters.Add("$prov", SqliteType.Integer);
        var pSuc = cmdC.Parameters.Add("$suc", SqliteType.Integer);
        var pUsr = cmdC.Parameters.Add("$usr", SqliteType.Integer);
        var pFal = cmdC.Parameters.Add("$fal", SqliteType.Text);

        using var cmdD = c.CreateCommand();
        cmdD.CommandText = @"INSERT INTO CompraDetalle (Id_compra, Id_producto, Cantidad, PrecioCosto, Subtotal) VALUES ($cid,$pid,$cant,$pc,$sub)";
        var pCid = cmdD.Parameters.Add("$cid", SqliteType.Integer);
        var pPid = cmdD.Parameters.Add("$pid", SqliteType.Integer);
        var pCant = cmdD.Parameters.Add("$cant", SqliteType.Real);
        var pPc = cmdD.Parameters.Add("$pc", SqliteType.Real);
        var pSub = cmdD.Parameters.Add("$sub", SqliteType.Real);

        for (int i = existentes + 1; i <= total; i++)
        {
            pFec.Value = DateTime.Now.AddDays(-rand.Next(0, 365)).ToString("yyyy-MM-dd HH:mm:ss");
            int numDet = rand.Next(1, 5);
            decimal totalC = 0;
            var dets = new (int pid, int cant, decimal pc)[numDet];
            for (int d = 0; d < numDet; d++)
            {
                int cant = rand.Next(10, 100);
                decimal pc = Math.Round((decimal)(rand.NextDouble() * 200 + 1), 2);
                totalC += cant * pc;
                dets[d] = (rand.Next(1, totalProd + 1), cant, pc);
            }
            pTot.Value = Math.Round(totalC, 2);
            pEst.Value = 2; // Recibida
            pProv.Value = rand.Next(1, totalProv + 1);
            pSuc.Value = sucId;
            pUsr.Value = usrId;
            pFal.Value = DateTime.Now.AddDays(-rand.Next(0, 365)).ToString("yyyy-MM-dd HH:mm:ss");
            cmdC.ExecuteNonQuery();
            long cid = (long)Scalar(c, "SELECT last_insert_rowid()");
            foreach (var d in dets)
            {
                pCid.Value = cid; pPid.Value = d.pid; pCant.Value = d.cant;
                pPc.Value = d.pc; pSub.Value = Math.Round(d.cant * d.pc, 2);
                cmdD.ExecuteNonQuery();
            }
        }
        tx.Commit();
    }

    static void SeedMovimientosStock(SqliteConnection c, int total, long sucId, long usrId)
    {
        var existentes = (int)(long)Scalar(c, "SELECT COUNT(*) FROM MovimientoStock");
        var totalProd = (int)(long)Scalar(c, "SELECT COUNT(*) FROM Producto");
        var rand = new Random(42);
        var tipoIds = GetIds(c, "SELECT Id FROM TipoMovimientoStock");
        if (tipoIds.Length == 0) tipoIds = new[] { 1, 2, 3 };

        using var tx = c.BeginTransaction();
        using var cmd = c.CreateCommand();
        cmd.CommandText = @"INSERT INTO MovimientoStock (Fecha, TipoMovimiento, Cantidad, StockAnterior, StockNuevo, Id_sucursal, Id_producto, Id_usuario, Observacion) VALUES ($fec,$tipo,$cant,$sa,$sn,$suc,$prod,$usr,$obs)";
        var pFec = cmd.Parameters.Add("$fec", SqliteType.Text);
        var pTipo = cmd.Parameters.Add("$tipo", SqliteType.Integer);
        var pCant = cmd.Parameters.Add("$cant", SqliteType.Real);
        var pSa = cmd.Parameters.Add("$sa", SqliteType.Real);
        var pSn = cmd.Parameters.Add("$sn", SqliteType.Real);
        var pSuc = cmd.Parameters.Add("$suc", SqliteType.Integer);
        var pProd = cmd.Parameters.Add("$prod", SqliteType.Integer);
        var pUsr = cmd.Parameters.Add("$usr", SqliteType.Integer);
        var pObs = cmd.Parameters.Add("$obs", SqliteType.Text);

        for (int i = existentes + 1; i <= total; i++)
        {
            pFec.Value = DateTime.Now.AddDays(-rand.Next(0, 180)).ToString("yyyy-MM-dd HH:mm:ss");
            pTipo.Value = tipoIds[rand.Next(tipoIds.Length)];
            int cant = rand.Next(1, 50);
            int sa = rand.Next(0, 200);
            pCant.Value = cant;
            pSa.Value = sa;
            pSn.Value = Math.Max(0, sa + (rand.Next(2) == 0 ? cant : -cant));
            pSuc.Value = sucId;
            pProd.Value = rand.Next(1, totalProd + 1);
            pUsr.Value = usrId;
            pObs.Value = $"Movimiento {i}";
            cmd.ExecuteNonQuery();
        }
        tx.Commit();
    }

    // ════════════════════════════════════════════════════════
    //  SEED CAJAS + MOVIMIENTOS CAJA
    // ════════════════════════════════════════════════════════
    static void SeedCajas(SqliteConnection c, int total, long sucId, long usrId)
    {
        var existentes = (int)(long)Scalar(c, "SELECT COUNT(*) FROM Caja");
        var rand = new Random(42);
        var turnos = new[] { "Mañana", "Tarde", "Noche" };

        using var tx = c.BeginTransaction();

        // Insertar cajas (todas cerradas para tener datos históricos)
        using var cmdC = c.CreateCommand();
        cmdC.CommandText = @"INSERT INTO Caja (FechaApertura, FechaCierre, MontoInicial, MontoFinal, Estado, EsPrimaria, Turno, Id_sucursal, UsuarioApertura_id, UsuarioCierre_id, Activo, FechaAlta) 
                             VALUES ($fa,$fc,$mi,$mf,$est,$prim,$turn,$suc,$ua,$uc,1,$fal)";
        var pFa = cmdC.Parameters.Add("$fa", SqliteType.Text);
        var pFc = cmdC.Parameters.Add("$fc", SqliteType.Text);
        var pMi = cmdC.Parameters.Add("$mi", SqliteType.Real);
        var pMf = cmdC.Parameters.Add("$mf", SqliteType.Real);
        var pEst = cmdC.Parameters.Add("$est", SqliteType.Integer);
        var pPrim = cmdC.Parameters.Add("$prim", SqliteType.Integer);
        var pTurn = cmdC.Parameters.Add("$turn", SqliteType.Text);
        var pSuc = cmdC.Parameters.Add("$suc", SqliteType.Integer);
        var pUa = cmdC.Parameters.Add("$ua", SqliteType.Integer);
        var pUc = cmdC.Parameters.Add("$uc", SqliteType.Integer);
        var pFal = cmdC.Parameters.Add("$fal", SqliteType.Text);

        using var cmdM = c.CreateCommand();
        cmdM.CommandText = @"INSERT INTO MovimientoCaja (Tipo, Monto, Fecha, Concepto, Id_caja, Id_usuario) 
                             VALUES ($tip,$mon,$fec,$con,$caj,$usr)";
        var pTip = cmdM.Parameters.Add("$tip", SqliteType.Integer);
        var pMon = cmdM.Parameters.Add("$mon", SqliteType.Real);
        var pMFec = cmdM.Parameters.Add("$fec", SqliteType.Text);
        var pCon = cmdM.Parameters.Add("$con", SqliteType.Text);
        var pCaj = cmdM.Parameters.Add("$caj", SqliteType.Integer);
        var pMUsr = cmdM.Parameters.Add("$usr", SqliteType.Integer);

        for (int i = existentes + 1; i <= total; i++)
        {
            var fechaAp = DateTime.Now.AddDays(-rand.Next(0, 90)).AddHours(rand.Next(8, 20));
            var fechaCierre = fechaAp.AddHours(rand.Next(4, 12));
            decimal montoInicial = rand.Next(10000, 50000);
            decimal montoFinal = montoInicial;

            pFa.Value = fechaAp.ToString("yyyy-MM-dd HH:mm:ss");
            pFc.Value = fechaCierre.ToString("yyyy-MM-dd HH:mm:ss");
            pMi.Value = montoInicial;
            pMf.Value = montoFinal;
            pEst.Value = 2; // Cerrada
            pPrim.Value = i <= 3 ? 1 : 0;
            pTurn.Value = turnos[i % turnos.Length];
            pSuc.Value = sucId;
            pUa.Value = usrId;
            pUc.Value = usrId;
            pFal.Value = fechaAp.ToString("yyyy-MM-dd HH:mm:ss");
            cmdC.ExecuteNonQuery();
            long cajaId = (long)Scalar(c, "SELECT last_insert_rowid()");

            // 5-15 movimientos por caja (ingresos por ventas + algunos egresos)
            int numMovs = rand.Next(5, 16);
            for (int m = 0; m < numMovs; m++)
            {
                bool esIngreso = rand.Next(3) != 0; // 66% ingresos
                decimal monto = Math.Round((decimal)(rand.NextDouble() * 5000 + 100), 2);

                pTip.Value = esIngreso ? 1 : 2; // 1=Ingreso, 2=Egreso
                pMon.Value = monto;
                pMFec.Value = fechaAp.AddHours(rand.Next(0, (int)(fechaCierre - fechaAp).TotalHours)).ToString("yyyy-MM-dd HH:mm:ss");
                pCon.Value = esIngreso ? $"Venta #{rand.Next(1000, 9999)}" : $"Egreso manual";
                pCaj.Value = cajaId;
                pMUsr.Value = usrId;
                cmdM.ExecuteNonQuery();

                montoFinal += esIngreso ? monto : -monto;
            }

            // Actualizar monto final real
            Exec(c, $"UPDATE Caja SET MontoFinal = {montoFinal.ToString(System.Globalization.CultureInfo.InvariantCulture)} WHERE Id = {cajaId}");
        }
        tx.Commit();
    }

    // ════════════════════════════════════════════════════════
    //  SEED DESCUENTOS
    // ════════════════════════════════════════════════════════
    static void SeedDescuentos(SqliteConnection c, int total, long empId)
    {
        var existentes = (int)(long)Scalar(c, "SELECT COUNT(*) FROM DescuentoConfiguracion");
        var rand = new Random(42);
        var catIds = GetIds(c, "SELECT Id FROM Categoria");
        var prodIds = GetIds(c, "SELECT Id FROM Producto");
        var mpIds = GetIds(c, "SELECT Id FROM MetodoPago");

        using var tx = c.BeginTransaction();
        using var cmdD = c.CreateCommand();
        cmdD.CommandText = @"INSERT INTO DescuentoConfiguracion (Nombre, ModoDescuento, Alcance, Valor, Id_producto, Id_categoria, AplicaCualquierMetodoPago, Id_empresa, FechaDesde, FechaHasta, Activo, FechaAlta) 
                             VALUES ($nom,$mod,$alc,$val,$prod,$cat,$any,$emp,$fd,$fh,1,$fal)";
        var pNom = cmdD.Parameters.Add("$nom", SqliteType.Text);
        var pMod = cmdD.Parameters.Add("$mod", SqliteType.Integer);
        var pAlc = cmdD.Parameters.Add("$alc", SqliteType.Integer);
        var pVal = cmdD.Parameters.Add("$val", SqliteType.Real);
        var pProd = cmdD.Parameters.Add("$prod", SqliteType.Integer);
        var pCat = cmdD.Parameters.Add("$cat", SqliteType.Integer);
        var pAny = cmdD.Parameters.Add("$any", SqliteType.Integer);
        var pEmp = cmdD.Parameters.Add("$emp", SqliteType.Integer);
        var pFd = cmdD.Parameters.Add("$fd", SqliteType.Text);
        var pFh = cmdD.Parameters.Add("$fh", SqliteType.Text);
        var pFal = cmdD.Parameters.Add("$fal", SqliteType.Text);

        using var cmdP = c.CreateCommand();
        cmdP.CommandText = @"INSERT INTO DescuentoMetodoPago (Id_descuentoConfiguracion, Id_metodoPago) VALUES ($did,$mpid)";
        var pDid = cmdP.Parameters.Add("$did", SqliteType.Integer);
        var pMpid = cmdP.Parameters.Add("$mpid", SqliteType.Integer);

        for (int i = existentes + 1; i <= total; i++)
        {
            int alcance = rand.Next(1, 4); // 1=Producto, 2=Categoría, 3=MétodoPago
            decimal valor = Math.Round((decimal)(rand.NextDouble() * 30 + 5), 0);

            pNom.Value = $"Descuento {i} - {valor}%";
            pMod.Value = 1; // Porcentaje
            pAlc.Value = alcance;
            pVal.Value = valor;
            pProd.Value = alcance == 1 && prodIds.Length > 0 ? prodIds[rand.Next(prodIds.Length)] : (object)DBNull.Value;
            pCat.Value = alcance == 2 && catIds.Length > 0 ? catIds[rand.Next(catIds.Length)] : (object)DBNull.Value;
            pAny.Value = alcance == 3 ? 0 : 1;
            pEmp.Value = empId;
            pFd.Value = DateTime.Now.AddDays(-rand.Next(0, 60)).ToString("yyyy-MM-dd");
            pFh.Value = DateTime.Now.AddDays(rand.Next(1, 90)).ToString("yyyy-MM-dd");
            pFal.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            cmdD.ExecuteNonQuery();
            long descId = (long)Scalar(c, "SELECT last_insert_rowid()");

            // Si es por método de pago, vincular 1-3 métodos
            if (alcance == 3 && mpIds.Length > 0)
            {
                int numMp = rand.Next(1, Math.Min(4, mpIds.Length + 1));
                for (int m = 0; m < numMp; m++)
                {
                    pDid.Value = descId;
                    pMpid.Value = mpIds[m];
                    cmdP.ExecuteNonQuery();
                }
            }
        }
        tx.Commit();
    }

    // ════════════════════════════════════════════════════════
    //  HELPERS
    // ════════════════════════════════════════════════════════
    static int[] GetIds(SqliteConnection c, string sql)
    {
        using var cmd = c.CreateCommand(); cmd.CommandText = sql;
        using var r = cmd.ExecuteReader();
        var list = new System.Collections.Generic.List<int>();
        while (r.Read()) list.Add(r.GetInt32(0));
        return list.ToArray();
    }
    static void Exec(SqliteConnection c, string sql) { using var cmd = c.CreateCommand(); cmd.CommandText = sql; cmd.ExecuteNonQuery(); }
    static object Scalar(SqliteConnection c, string sql) { using var cmd = c.CreateCommand(); cmd.CommandText = sql; return cmd.ExecuteScalar(); }
}
