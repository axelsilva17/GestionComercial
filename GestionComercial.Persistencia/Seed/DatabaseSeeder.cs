using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using GestionComercial.Persistencia.Contexto;

namespace GestionComercial.Persistencia.Seed
{
    /// <summary>
    /// Carga datos de prueba directamente en la DB SQLite usando SQL crudo.
    /// Se ejecuta solo si las tablas principales están vacías (primera ejecución).
    /// Estos datos son para testing en la máquina del desarrollador.
    /// </summary>
    public static class DatabaseSeeder
    {
        /// <summary>
        /// Entry point: detecta si hay datos y carga faltantes.
        /// Llamar después de MigrateAsync() en el Bootstrapper.
        /// </summary>
        public static async Task SeedIfNeededAsync(GestionComercialContext context)
        {
            // Solo seedear si no hay productos (tabla principal de negocio)
            var totalProd = await context.Productos.CountAsync();
            if (totalProd >= 100) return; // Ya tiene datos suficientes

            System.Diagnostics.Debug.WriteLine("[Seed] DB con pocos datos, iniciando carga de prueba...");
            var sw = System.Diagnostics.Stopwatch.StartNew();

            // Obtener conexión raw de EF Core
            var conn = context.Database.GetDbConnection();
            var wasOpen = conn.State == System.Data.ConnectionState.Open;
            if (!wasOpen) await conn.OpenAsync();

            try
            {
                // PRAGMAs para inserción masiva rápida
                await ExecAsync(conn, "PRAGMA journal_mode=WAL;");
                await ExecAsync(conn, "PRAGMA synchronous=OFF;");
                await ExecAsync(conn, "PRAGMA foreign_keys=OFF;");
                await ExecAsync(conn, "PRAGMA cache_size=-64000;"); // 64MB cache

                var empId = await EnsureEmpresaAsync(conn);
                var sucId = await EnsureSucursalAsync(conn, empId);
                var usrId = await EnsureUsuarioAsync(conn, sucId);

                // Seeds base (si están vacías)
                if (Convert.ToInt32(await ScalarAsync(conn, "SELECT COUNT(*) FROM Categoria")) == 0)
                    await SeedCategoriasAsync(conn);
                if (Convert.ToInt32(await ScalarAsync(conn, "SELECT COUNT(*) FROM UnidadMedida")) == 0)
                    await SeedUnidadesMedidaAsync(conn);
                if (Convert.ToInt32(await ScalarAsync(conn, "SELECT COUNT(*) FROM MetodoPago")) == 0)
                    await SeedMetodosPagoAsync(conn);
                if (Convert.ToInt32(await ScalarAsync(conn, "SELECT COUNT(*) FROM TipoMovimientoStock")) == 0)
                    await SeedTiposMovimientoAsync(conn);

                // Seeds masivos (solo si faltan)
                if (Convert.ToInt32(await ScalarAsync(conn, "SELECT COUNT(*) FROM Producto")) < 100)
                    await SeedProductosAsync(conn, 5000, empId);
                if (Convert.ToInt32(await ScalarAsync(conn, "SELECT COUNT(*) FROM Cliente")) < 100)
                    await SeedClientesAsync(conn, 2000, empId);
                if (Convert.ToInt32(await ScalarAsync(conn, "SELECT COUNT(*) FROM Proveedor")) < 50)
                    await SeedProveedoresAsync(conn, 200, empId);
                if (Convert.ToInt32(await ScalarAsync(conn, "SELECT COUNT(*) FROM Venta")) < 100)
                    await SeedVentasAsync(conn, 10_000, sucId, usrId);
                if (Convert.ToInt32(await ScalarAsync(conn, "SELECT COUNT(*) FROM Compra")) < 50)
                    await SeedComprasAsync(conn, 2000, sucId, usrId);
                if (Convert.ToInt32(await ScalarAsync(conn, "SELECT COUNT(*) FROM MovimientoStock")) < 100)
                    await SeedMovimientosStockAsync(conn, 5000, sucId, usrId);
                if (Convert.ToInt32(await ScalarAsync(conn, "SELECT COUNT(*) FROM Caja")) < 10)
                    await SeedCajasAsync(conn, 50, sucId, usrId);
                if (Convert.ToInt32(await ScalarAsync(conn, "SELECT COUNT(*) FROM DescuentoConfiguracion")) < 10)
                    await SeedDescuentosAsync(conn, 30, empId);

                // Restaurar PRAGMAs normales
                await ExecAsync(conn, "PRAGMA foreign_keys=ON;");
                await ExecAsync(conn, "PRAGMA synchronous=NORMAL;");

                System.Diagnostics.Debug.WriteLine($"[Seed] Carga completada en {sw.Elapsed.TotalSeconds:N1}s");
            }
            finally
            {
                if (!wasOpen) conn.Close();
            }
        }

        // ════════════════════════════════════════════════════════
        //  ENSURE BASE ENTITIES
        // ════════════════════════════════════════════════════════
        static async Task<int> EnsureEmpresaAsync(System.Data.Common.DbConnection c)
        {
            var id = await ScalarAsync(c, "SELECT Id FROM Empresa LIMIT 1");
            if (id != null) return Convert.ToInt32(id);
            await ExecAsync(c, $"INSERT INTO Empresa (Nombre, CUIT, Direccion, Telefono, Email, Activo, FechaAlta) VALUES ('Mi Empresa SA', '30-71234567-9', 'Av. Principal 1234', '11-5555-5555', 'empresa@test.com', 1, datetime('now'))");
            return Convert.ToInt32((await ScalarAsync(c, "SELECT last_insert_rowid()"))!);
        }

        static async Task<int> EnsureSucursalAsync(System.Data.Common.DbConnection c, int empId)
        {
            var id = await ScalarAsync(c, "SELECT Id FROM Sucursal LIMIT 1");
            if (id != null) return Convert.ToInt32(id);
            await ExecAsync(c, $"INSERT INTO Sucursal (Nombre, Direccion, Id_empresa, Activo, FechaAlta) VALUES ('Sucursal Central', 'Av. Principal 1234', {empId}, 1, datetime('now'))");
            return Convert.ToInt32((await ScalarAsync(c, "SELECT last_insert_rowid()"))!);
        }

        static async Task<int> EnsureUsuarioAsync(System.Data.Common.DbConnection c, int sucId)
        {
            var id = await ScalarAsync(c, "SELECT Id FROM Usuario LIMIT 1");
            if (id != null) return Convert.ToInt32(id);
            // Crear los 3 usuarios demo
            var hash1 = BCrypt.Net.BCrypt.HashPassword("Admin123!", 10);
            var hash2 = BCrypt.Net.BCrypt.HashPassword("Vendedor123!", 10);
            var hash3 = BCrypt.Net.BCrypt.HashPassword("Gerente123!", 10);
            await ExecAsync(c, $"INSERT INTO Usuario (Id, Nombre, Apellido, Email, PasswordHash, Id_sucursal, Id_rol, IntentosFallidos, Activo, FechaAlta) VALUES (1, 'Admin', 'Sistema', 'admin@miempresa.com', '{hash1}', {sucId}, 2, 0, 1, datetime('now'))");
            await ExecAsync(c, $"INSERT INTO Usuario (Id, Nombre, Apellido, Email, PasswordHash, Id_sucursal, Id_rol, IntentosFallidos, Activo, FechaAlta) VALUES (2, 'Vendedor', 'Demo', 'vendedor@miempresa.com', '{hash2}', {sucId}, 3, 0, 1, datetime('now'))");
            await ExecAsync(c, $"INSERT INTO Usuario (Id, Nombre, Apellido, Email, PasswordHash, Id_sucursal, Id_rol, IntentosFallidos, Activo, FechaAlta) VALUES (3, 'Gerente', 'Demo', 'gerente@miempresa.com', '{hash3}', {sucId}, 1, 0, 1, datetime('now'))");
            return 1;
        }

        // ════════════════════════════════════════════════════════
        //  SEED BASE
        // ════════════════════════════════════════════════════════
        static async Task SeedCategoriasAsync(System.Data.Common.DbConnection c)
        {
            var items = new[] { "Bebidas", "Alimentos", "Limpieza", "Electrónica", "Hogar", "Textil", "Deportes", "Juguetes", "Automotor", "Otros" };
            foreach (var n in items)
                await ExecAsync(c, $"INSERT INTO Categoria (Nombre, Activo, FechaAlta) VALUES ('{n}', 1, datetime('now'))");
        }

        static async Task SeedUnidadesMedidaAsync(System.Data.Common.DbConnection c)
        {
            var items = new[] { ("Unidad", "UN"), ("Kilogramo", "KG"), ("Litro", "L"), ("Metro", "M"), ("Caja", "CJ"), ("Paquete", "PQ") };
            foreach (var (n, a) in items)
                await ExecAsync(c, $"INSERT INTO UnidadMedida (Nombre, Abreviatura, Activo, FechaAlta) VALUES ('{n}', '{a}', 1, datetime('now'))");
        }

        static async Task SeedMetodosPagoAsync(System.Data.Common.DbConnection c)
        {
            var items = new[] { "Efectivo", "Tarjeta Crédito", "Tarjeta Débito", "Transferencia", "QR Mercado Pago", "Cuenta Corriente" };
            foreach (var n in items)
                await ExecAsync(c, $"INSERT INTO MetodoPago (Nombre, Activo, FechaAlta) VALUES ('{n}', 1, datetime('now'))");
        }

        static async Task SeedTiposMovimientoAsync(System.Data.Common.DbConnection c)
        {
            var items = new[] { "Entrada", "Salida", "Ajuste", "Devolución", "Transferencia" };
            foreach (var n in items)
                await ExecAsync(c, $"INSERT INTO TipoMovimientoStock (Nombre, Activo, FechaAlta) VALUES ('{n}', 1, datetime('now'))");
        }

        // ════════════════════════════════════════════════════════
        //  SEED MASIVO (parámetros para evitar SQL injection)
        // ════════════════════════════════════════════════════════
        static async Task SeedProductosAsync(System.Data.Common.DbConnection c, int total, int empId)
        {
            var rand = new Random(42);
            var nombres = new[] { "Leche", "Pan", "Aceite", "Arroz", "Fideos", "Azúcar", "Sal", "Café", "Yerba", "Gaseosa",
                "Agua", "Jugo", "Cerveza", "Vino", "Chocolate", "Galletitas", "Cereal", "Sopa", "Mayonesa", "Ketchup",
                "Manteca", "Queso", "Yogur", "Huevos", "Pollo", "Carne", "Cerdo", "Pescado", "Atún", "Sardina",
                "Shampoo", "Jabón", "Papel", "Detergente", "Lavandina", "Esponja", "Bolsa", "Fósforo", "Vela", "Cinta",
                "Taladro", "Tornillos", "Clavos", "Pintura", "Brocha", "Llave", "Martillo", "Sierra", "Cable", "Enchufe" };
            var catIds = await GetIdsAsync(c, "SELECT Id FROM Categoria");
            var umIds = await GetIdsAsync(c, "SELECT Id FROM UnidadMedida");

            using var cmd = CreateCmd(c, @"INSERT INTO Producto (Nombre, CodigoBarra, PrecioVentaActual, PrecioCostoActual, StockActual, StockMinimo, Id_categoria, Id_empresa, Id_unidadMedida, Activo, FechaAlta) VALUES ($nom,$cod,$pv,$pc,$sa,$sm,$cat,$emp,$um,1,$fec)");
            var pn = AddParam(cmd, "$nom", SqliteType.Text);
            var pc2 = AddParam(cmd, "$cod", SqliteType.Text);
            var ppv = AddParam(cmd, "$pv", SqliteType.Real);
            var ppc = AddParam(cmd, "$pc", SqliteType.Real);
            var psa = AddParam(cmd, "$sa", SqliteType.Integer);
            var psm = AddParam(cmd, "$sm", SqliteType.Integer);
            var pcat = AddParam(cmd, "$cat", SqliteType.Integer);
            var pemp = AddParam(cmd, "$emp", SqliteType.Integer);
            var pum = AddParam(cmd, "$um", SqliteType.Integer);
            var pfec = AddParam(cmd, "$fec", SqliteType.Text);

            for (int i = 1; i <= total; i++)
            {
                pn.Value = $"{nombres[i % nombres.Length]} {i}";
                pc2.Value = $"789{i:D8}";
                ppv.Value = Math.Round((decimal)(rand.NextDouble() * 5000 + 50), 2);
                ppc.Value = Math.Round((decimal)(rand.NextDouble() * 2000 + 10), 2);
                psa.Value = rand.Next(0, 200);
                psm.Value = rand.Next(5, 30);
                pcat.Value = catIds.Length > 0 ? catIds[rand.Next(catIds.Length)] : 1;
                pemp.Value = empId;
                pum.Value = umIds.Length > 0 ? umIds[rand.Next(umIds.Length)] : 1;
                pfec.Value = DateTime.Now.AddDays(-rand.Next(0, 365)).ToString("yyyy-MM-dd HH:mm:ss");
                await cmd.ExecuteNonQueryAsync();
            }
        }

        static async Task SeedClientesAsync(System.Data.Common.DbConnection c, int total, int empId)
        {
            var rand = new Random(42);
            using var cmd = CreateCmd(c, @"INSERT INTO Cliente (Nombre, Documento, Email, Telefono, Id_empresa, Activo, FechaAlta) VALUES ($nom,$doc,$email,$tel,$emp,1,$fec)");
            var pn = AddParam(cmd, "$nom", SqliteType.Text);
            var pdoc = AddParam(cmd, "$doc", SqliteType.Integer);
            var pe = AddParam(cmd, "$email", SqliteType.Text);
            var pt = AddParam(cmd, "$tel", SqliteType.Text);
            var pem = AddParam(cmd, "$emp", SqliteType.Integer);
            var pfec = AddParam(cmd, "$fec", SqliteType.Text);

            for (int i = 1; i <= total; i++)
            {
                pn.Value = $"Cliente {i}";
                pdoc.Value = 10000000 + i;
                pe.Value = $"cliente{i}@mail.com";
                pt.Value = $"11{rand.Next(4000, 5999)}-{rand.Next(1000, 9999)}";
                pem.Value = empId;
                pfec.Value = DateTime.Now.AddDays(-rand.Next(0, 365)).ToString("yyyy-MM-dd HH:mm:ss");
                await cmd.ExecuteNonQueryAsync();
            }
        }

        static async Task SeedProveedoresAsync(System.Data.Common.DbConnection c, int total, int empId)
        {
            var rand = new Random(42);
            var rubros = new[] { "Alimentos", "Bebidas", "Limpieza", "Electrónica", "Ropa", "Hogar" };
            using var cmd = CreateCmd(c, @"INSERT INTO Proveedor (Nombre, Email, Telefono, CUIT, Id_empresa, Activo, FechaAlta) VALUES ($nom,$email,$tel,$cuit,$emp,1,$fec)");
            var pn = AddParam(cmd, "$nom", SqliteType.Text);
            var pe = AddParam(cmd, "$email", SqliteType.Text);
            var pt = AddParam(cmd, "$tel", SqliteType.Text);
            var pc = AddParam(cmd, "$cuit", SqliteType.Text);
            var pem = AddParam(cmd, "$emp", SqliteType.Integer);
            var pfec = AddParam(cmd, "$fec", SqliteType.Text);

            for (int i = 1; i <= total; i++)
            {
                pn.Value = $"Proveedor {rubros[i % rubros.Length]} {i}";
                pe.Value = $"proveedor{i}@distribuidora.com";
                pt.Value = $"11{rand.Next(4000, 5999)}-{rand.Next(1000, 9999)}";
                pc.Value = $"30-{rand.Next(10000000, 99999999)}-{rand.Next(0, 9)}";
                pem.Value = empId;
                pfec.Value = DateTime.Now.AddDays(-rand.Next(0, 365)).ToString("yyyy-MM-dd HH:mm:ss");
                await cmd.ExecuteNonQueryAsync();
            }
        }

        static async Task SeedVentasAsync(System.Data.Common.DbConnection c, int total, int sucId, int usrId)
        {
            var rand = new Random(42);
            var totalProd = Convert.ToInt32(await ScalarAsync(c, "SELECT COUNT(*) FROM Producto"));
            var totalCli = Convert.ToInt32(await ScalarAsync(c, "SELECT COUNT(*) FROM Cliente"));

            using var cmdV = CreateCmd(c, @"INSERT INTO Venta (Fecha, TotalBruto, TotalDescuento, TotalFinal, Estado, Id_sucursal, Id_cliente, Id_usuario, Activo, FechaAlta) VALUES ($fec,$tb,$td,$tf,$est,$suc,$cli,$usr,1,$fal)");
            var pFec = AddParam(cmdV, "$fec", SqliteType.Text);
            var pTb = AddParam(cmdV, "$tb", SqliteType.Real);
            var pTd = AddParam(cmdV, "$td", SqliteType.Real);
            var pTf = AddParam(cmdV, "$tf", SqliteType.Real);
            var pEst = AddParam(cmdV, "$est", SqliteType.Integer);
            var pSuc = AddParam(cmdV, "$suc", SqliteType.Integer);
            var pCli = AddParam(cmdV, "$cli", SqliteType.Integer);
            var pUsr = AddParam(cmdV, "$usr", SqliteType.Integer);
            var pFal = AddParam(cmdV, "$fal", SqliteType.Text);

            using var cmdD = CreateCmd(c, @"INSERT INTO VentaDetalle (Id_venta, Id_producto, Cantidad, PrecioUnitario, CostoUnitario, Subtotal, Descuento, MargenUnitario) VALUES ($vid,$pid,$cant,$pu,$cu,$sub,$desc,$mu)");
            var pVid = AddParam(cmdD, "$vid", SqliteType.Integer);
            var pPid = AddParam(cmdD, "$pid", SqliteType.Integer);
            var pCant = AddParam(cmdD, "$cant", SqliteType.Real);
            var pPu = AddParam(cmdD, "$pu", SqliteType.Real);
            var pCu = AddParam(cmdD, "$cu", SqliteType.Real);
            var pSub = AddParam(cmdD, "$sub", SqliteType.Real);
            var pDesc = AddParam(cmdD, "$desc", SqliteType.Real);
            var pMu = AddParam(cmdD, "$mu", SqliteType.Real);

            using var cmdP = CreateCmd(c, @"INSERT INTO Pago (Id_venta, Monto, Id_metodoPago, Fecha) VALUES ($vid,$mont,$met,$fec)");
            var pVid2 = AddParam(cmdP, "$vid", SqliteType.Integer);
            var pMont = AddParam(cmdP, "$mont", SqliteType.Real);
            var pMet = AddParam(cmdP, "$met", SqliteType.Integer);
            var pPFec = AddParam(cmdP, "$fec", SqliteType.Text);

            for (int i = 1; i <= total; i++)
            {
                pFec.Value = DateTime.Now.AddDays(-rand.Next(0, 365)).AddHours(rand.Next(8, 22)).ToString("yyyy-MM-dd HH:mm:ss");
                int numDet = rand.Next(1, 6);
                decimal totalBruto = 0;
                var dets = new (int pid, int cant, decimal pu, decimal cu)[numDet];
                for (int d = 0; d < numDet; d++)
                {
                    int cant = rand.Next(1, 8);
                    decimal pu = Math.Round((decimal)(rand.NextDouble() * 5000 + 50), 2);
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
                await cmdV.ExecuteNonQueryAsync();
                long vid = Convert.ToInt64(await ScalarAsync(c, "SELECT last_insert_rowid()"));

                foreach (var d in dets)
                {
                    pVid.Value = vid; pPid.Value = d.pid; pCant.Value = d.cant;
                    pPu.Value = d.pu; pCu.Value = d.cu;
                    pSub.Value = Math.Round(d.cant * d.pu, 2);
                    pDesc.Value = 0m;
                    pMu.Value = Math.Round(d.pu - d.cu, 2);
                    await cmdD.ExecuteNonQueryAsync();
                }

                int numPagos = rand.Next(1, 3);
                decimal restante = totalBruto;
                for (int p = 0; p < numPagos; p++)
                {
                    decimal monto = p == numPagos - 1 ? restante : Math.Round(restante / (numPagos - p), 2);
                    pVid2.Value = vid; pMont.Value = monto; pMet.Value = rand.Next(1, 7);
                    pPFec.Value = DateTime.Now.AddDays(-rand.Next(0, 365)).ToString("yyyy-MM-dd HH:mm:ss");
                    await cmdP.ExecuteNonQueryAsync();
                    restante -= monto;
                }
            }
        }

        static async Task SeedComprasAsync(System.Data.Common.DbConnection c, int total, int sucId, int usrId)
        {
            var rand = new Random(42);
            var totalProd = Convert.ToInt32(await ScalarAsync(c, "SELECT COUNT(*) FROM Producto"));
            var totalProv = Convert.ToInt32(await ScalarAsync(c, "SELECT COUNT(*) FROM Proveedor"));

            using var cmdC = CreateCmd(c, @"INSERT INTO Compra (Fecha, Total, Estado, Id_proveedor, Id_sucursal, Id_usuario, Activo, FechaAlta) VALUES ($fec,$tot,$est,$prov,$suc,$usr,1,$fal)");
            var pFec = AddParam(cmdC, "$fec", SqliteType.Text);
            var pTot = AddParam(cmdC, "$tot", SqliteType.Real);
            var pEst = AddParam(cmdC, "$est", SqliteType.Integer);
            var pProv = AddParam(cmdC, "$prov", SqliteType.Integer);
            var pSuc = AddParam(cmdC, "$suc", SqliteType.Integer);
            var pUsr = AddParam(cmdC, "$usr", SqliteType.Integer);
            var pFal = AddParam(cmdC, "$fal", SqliteType.Text);

            using var cmdD = CreateCmd(c, @"INSERT INTO CompraDetalle (Id_compra, Id_producto, Cantidad, PrecioCosto, Subtotal) VALUES ($cid,$pid,$cant,$pc,$sub)");
            var pCid = AddParam(cmdD, "$cid", SqliteType.Integer);
            var pPid = AddParam(cmdD, "$pid", SqliteType.Integer);
            var pCant = AddParam(cmdD, "$cant", SqliteType.Real);
            var pPc = AddParam(cmdD, "$pc", SqliteType.Real);
            var pSub = AddParam(cmdD, "$sub", SqliteType.Real);

            for (int i = 1; i <= total; i++)
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
                pEst.Value = 2;
                pProv.Value = rand.Next(1, totalProv + 1);
                pSuc.Value = sucId;
                pUsr.Value = usrId;
                pFal.Value = DateTime.Now.AddDays(-rand.Next(0, 365)).ToString("yyyy-MM-dd HH:mm:ss");
                await cmdC.ExecuteNonQueryAsync();
                long cid = Convert.ToInt64(await ScalarAsync(c, "SELECT last_insert_rowid()"));
                foreach (var d in dets)
                {
                    pCid.Value = cid; pPid.Value = d.pid; pCant.Value = d.cant;
                    pPc.Value = d.pc; pSub.Value = Math.Round(d.cant * d.pc, 2);
                    await cmdD.ExecuteNonQueryAsync();
                }
            }
        }

        static async Task SeedMovimientosStockAsync(System.Data.Common.DbConnection c, int total, int sucId, int usrId)
        {
            var rand = new Random(42);
            var totalProd = Convert.ToInt32(await ScalarAsync(c, "SELECT COUNT(*) FROM Producto"));
            var tipoIds = await GetIdsAsync(c, "SELECT Id FROM TipoMovimientoStock");
            if (tipoIds.Length == 0) tipoIds = new[] { 1, 2, 3 };

            using var cmd = CreateCmd(c, @"INSERT INTO MovimientoStock (Fecha, TipoMovimiento, Cantidad, StockAnterior, StockNuevo, Id_sucursal, Id_producto, Id_usuario, Observacion) VALUES ($fec,$tipo,$cant,$sa,$sn,$suc,$prod,$usr,$obs)");
            var pFec = AddParam(cmd, "$fec", SqliteType.Text);
            var pTipo = AddParam(cmd, "$tipo", SqliteType.Integer);
            var pCant = AddParam(cmd, "$cant", SqliteType.Real);
            var pSa = AddParam(cmd, "$sa", SqliteType.Real);
            var pSn = AddParam(cmd, "$sn", SqliteType.Real);
            var pSuc = AddParam(cmd, "$suc", SqliteType.Integer);
            var pProd = AddParam(cmd, "$prod", SqliteType.Integer);
            var pUsr = AddParam(cmd, "$usr", SqliteType.Integer);
            var pObs = AddParam(cmd, "$obs", SqliteType.Text);

            for (int i = 1; i <= total; i++)
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
                await cmd.ExecuteNonQueryAsync();
            }
        }

        static async Task SeedCajasAsync(System.Data.Common.DbConnection c, int total, int sucId, int usrId)
        {
            var rand = new Random(42);
            var turnos = new[] { "Mañana", "Tarde", "Noche" };

            using var cmdC = CreateCmd(c, @"INSERT INTO Caja (FechaApertura, FechaCierre, MontoInicial, MontoFinal, Estado, EsPrimaria, Turno, Id_sucursal, UsuarioApertura_id, UsuarioCierre_id, Activo, FechaAlta) VALUES ($fa,$fc,$mi,$mf,$est,$prim,$turn,$suc,$ua,$uc,1,$fal)");
            var pFa = AddParam(cmdC, "$fa", SqliteType.Text);
            var pFc = AddParam(cmdC, "$fc", SqliteType.Text);
            var pMi = AddParam(cmdC, "$mi", SqliteType.Real);
            var pMf = AddParam(cmdC, "$mf", SqliteType.Real);
            var pEst = AddParam(cmdC, "$est", SqliteType.Integer);
            var pPrim = AddParam(cmdC, "$prim", SqliteType.Integer);
            var pTurn = AddParam(cmdC, "$turn", SqliteType.Text);
            var pSuc = AddParam(cmdC, "$suc", SqliteType.Integer);
            var pUa = AddParam(cmdC, "$ua", SqliteType.Integer);
            var pUc = AddParam(cmdC, "$uc", SqliteType.Integer);
            var pFal = AddParam(cmdC, "$fal", SqliteType.Text);

            using var cmdM = CreateCmd(c, @"INSERT INTO MovimientoCaja (Tipo, Monto, Fecha, Concepto, Id_caja, Id_usuario) VALUES ($tip,$mon,$fec,$con,$caj,$usr)");
            var pTip = AddParam(cmdM, "$tip", SqliteType.Integer);
            var pMon = AddParam(cmdM, "$mon", SqliteType.Real);
            var pMFec = AddParam(cmdM, "$fec", SqliteType.Text);
            var pCon = AddParam(cmdM, "$con", SqliteType.Text);
            var pCaj = AddParam(cmdM, "$caj", SqliteType.Integer);
            var pMUsr = AddParam(cmdM, "$usr", SqliteType.Integer);

            for (int i = 1; i <= total; i++)
            {
                var fechaAp = DateTime.Now.AddDays(-rand.Next(0, 90)).AddHours(rand.Next(8, 20));
                var fechaCierre = fechaAp.AddHours(rand.Next(4, 12));
                decimal montoInicial = rand.Next(10000, 50000);
                decimal montoFinal = montoInicial;

                pFa.Value = fechaAp.ToString("yyyy-MM-dd HH:mm:ss");
                pFc.Value = fechaCierre.ToString("yyyy-MM-dd HH:mm:ss");
                pMi.Value = montoInicial;
                pMf.Value = montoFinal;
                pEst.Value = 2;
                pPrim.Value = i <= 3 ? 1 : 0;
                pTurn.Value = turnos[i % turnos.Length];
                pSuc.Value = sucId;
                pUa.Value = usrId;
                pUc.Value = usrId;
                pFal.Value = fechaAp.ToString("yyyy-MM-dd HH:mm:ss");
                await cmdC.ExecuteNonQueryAsync();
                long cajaId = Convert.ToInt64(await ScalarAsync(c, "SELECT last_insert_rowid()"));

                int numMovs = rand.Next(5, 16);
                for (int m = 0; m < numMovs; m++)
                {
                    bool esIngreso = rand.Next(3) != 0;
                    decimal monto = Math.Round((decimal)(rand.NextDouble() * 5000 + 100), 2);
                    pTip.Value = esIngreso ? 1 : 2;
                    pMon.Value = monto;
                    pMFec.Value = fechaAp.AddHours(rand.Next(0, Math.Max(1, (int)(fechaCierre - fechaAp).TotalHours))).ToString("yyyy-MM-dd HH:mm:ss");
                    pCon.Value = esIngreso ? $"Venta #{rand.Next(1000, 9999)}" : "Egreso manual";
                    pCaj.Value = cajaId;
                    pMUsr.Value = usrId;
                    await cmdM.ExecuteNonQueryAsync();
                    montoFinal += esIngreso ? monto : -monto;
                }
                await ExecAsync(c, $"UPDATE Caja SET MontoFinal = {montoFinal.ToString(System.Globalization.CultureInfo.InvariantCulture)} WHERE Id = {cajaId}");
            }
        }

        static async Task SeedDescuentosAsync(System.Data.Common.DbConnection c, int total, int empId)
        {
            var rand = new Random(42);
            var catIds = await GetIdsAsync(c, "SELECT Id FROM Categoria");
            var prodIds = await GetIdsAsync(c, "SELECT Id FROM Producto");
            var mpIds = await GetIdsAsync(c, "SELECT Id FROM MetodoPago");

            using var cmdD = CreateCmd(c, @"INSERT INTO DescuentoConfiguracion (Nombre, ModoDescuento, Alcance, Valor, Id_producto, Id_categoria, AplicaCualquierMetodoPago, Id_empresa, FechaDesde, FechaHasta, Activo, FechaAlta) VALUES ($nom,$mod,$alc,$val,$prod,$cat,$any,$emp,$fd,$fh,1,$fal)");
            var pNom = AddParam(cmdD, "$nom", SqliteType.Text);
            var pMod = AddParam(cmdD, "$mod", SqliteType.Integer);
            var pAlc = AddParam(cmdD, "$alc", SqliteType.Integer);
            var pVal = AddParam(cmdD, "$val", SqliteType.Real);
            var pProd = AddParam(cmdD, "$prod", SqliteType.Integer);
            var pCat = AddParam(cmdD, "$cat", SqliteType.Integer);
            var pAny = AddParam(cmdD, "$any", SqliteType.Integer);
            var pEmp = AddParam(cmdD, "$emp", SqliteType.Integer);
            var pFd = AddParam(cmdD, "$fd", SqliteType.Text);
            var pFh = AddParam(cmdD, "$fh", SqliteType.Text);
            var pFal = AddParam(cmdD, "$fal", SqliteType.Text);

            using var cmdP = CreateCmd(c, @"INSERT INTO DescuentoMetodoPago (Id_descuentoConfiguracion, Id_metodoPago) VALUES ($did,$mpid)");
            var pDid = AddParam(cmdP, "$did", SqliteType.Integer);
            var pMpid = AddParam(cmdP, "$mpid", SqliteType.Integer);

            for (int i = 1; i <= total; i++)
            {
                int alcance = rand.Next(1, 4);
                decimal valor = Math.Round((decimal)(rand.NextDouble() * 30 + 5), 0);
                pNom.Value = $"Descuento {i} - {valor}%";
                pMod.Value = 1;
                pAlc.Value = alcance;
                pVal.Value = valor;
                pProd.Value = alcance == 1 && prodIds.Length > 0 ? prodIds[rand.Next(prodIds.Length)] : (object)DBNull.Value;
                pCat.Value = alcance == 2 && catIds.Length > 0 ? catIds[rand.Next(catIds.Length)] : (object)DBNull.Value;
                pAny.Value = alcance == 3 ? 0 : 1;
                pEmp.Value = empId;
                pFd.Value = DateTime.Now.AddDays(-rand.Next(0, 60)).ToString("yyyy-MM-dd");
                pFh.Value = DateTime.Now.AddDays(rand.Next(1, 90)).ToString("yyyy-MM-dd");
                pFal.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                await cmdD.ExecuteNonQueryAsync();
                long descId = Convert.ToInt64(await ScalarAsync(c, "SELECT last_insert_rowid()"));

                if (alcance == 3 && mpIds.Length > 0)
                {
                    int numMp = rand.Next(1, Math.Min(4, mpIds.Length + 1));
                    for (int m = 0; m < numMp; m++)
                    {
                        pDid.Value = descId;
                        pMpid.Value = mpIds[m];
                        await cmdP.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        // ════════════════════════════════════════════════════════
        //  HELPERS
        // ════════════════════════════════════════════════════════
        static SqliteCommand CreateCmd(System.Data.Common.DbConnection c, string sql)
        {
            var cmd = c.CreateCommand() as SqliteCommand ?? new SqliteCommand(sql, c as SqliteConnection);
            cmd.CommandText = sql;
            return cmd;
        }

        static SqliteParameter AddParam(SqliteCommand cmd, string name, SqliteType type)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = name;
            p.SqliteType = type;
            cmd.Parameters.Add(p);
            return p;
        }

        static async Task<int[]> GetIdsAsync(System.Data.Common.DbConnection c, string sql)
        {
            using var cmd = c.CreateCommand();
            cmd.CommandText = sql;
            using var r = await cmd.ExecuteReaderAsync();
            var list = new List<int>();
            while (await r.ReadAsync()) list.Add(r.GetInt32(0));
            return list.ToArray();
        }

        static async Task ExecAsync(System.Data.Common.DbConnection c, string sql)
        {
            using var cmd = c.CreateCommand();
            cmd.CommandText = sql;
            await cmd.ExecuteNonQueryAsync();
        }

        static async Task<object?> ScalarAsync(System.Data.Common.DbConnection c, string sql)
        {
            using var cmd = c.CreateCommand();
            cmd.CommandText = sql;
            return await cmd.ExecuteScalarAsync();
        }
    }
}
