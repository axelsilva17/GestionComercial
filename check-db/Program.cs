using Microsoft.Data.Sqlite;
using BC = BCrypt.Net.BCrypt;

var dbPath = args.Length > 0 ? args[0] : @"C:\Users\Usuario\Desktop\GestionComercial\GestionComercial.UI\GestionComercial.db";
Console.WriteLine($"Updating DB: {dbPath}");

var conn = new SqliteConnection($"Data Source={dbPath}");
conn.Open();

// 1. List existing users
Console.WriteLine("\n=== BEFORE ===");
var cmd = conn.CreateCommand();
cmd.CommandText = "SELECT Id, Email, Nombre, PasswordHash FROM Usuario ORDER BY Id";
using (var r = cmd.ExecuteReader())
    while (r.Read()) Console.WriteLine($"  Id={r[0]} Email={r[1]} Nombre={r[2]} Hash={((string)r[3])[..15]}...");

// 2. Delete ALL existing users (they have wrong emails/hashes from old migrations)
// Disable FK checks first
cmd.CommandText = "PRAGMA foreign_keys = OFF";
cmd.ExecuteNonQuery();
cmd.CommandText = "DELETE FROM Usuario";
var deleted = cmd.ExecuteNonQuery();
Console.WriteLine($"\nDeleted {deleted} old users (FK checks disabled)");
cmd.CommandText = "PRAGMA foreign_keys = ON";
cmd.ExecuteNonQuery();

// 3. Insert correct demo users
var users = new[]
{
    (1, "Admin", "Sistema", "admin@demo.com", BC.HashPassword("Admin123!", 10), 2, 1),
    (2, "Vendedor", "Demo", "vendedor@demo.com", BC.HashPassword("Vendedor123!", 10), 3, 1),
    (3, "Gerente", "Demo", "gerente@demo.com", BC.HashPassword("Gerente123!", 10), 1, 1),
};

foreach (var (id, nombre, apellido, email, hash, rol, suc) in users)
{
    cmd.CommandText = @"INSERT INTO Usuario (Id, Nombre, Apellido, Email, PasswordHash, Id_rol, Id_sucursal, IntentosFallidos, Activo, FechaAlta) 
                         VALUES ($id, $nom, $ape, $email, $hash, $rol, $suc, 0, 1, '2026-01-01 00:00:00')";
    cmd.Parameters.Clear();
    cmd.Parameters.AddWithValue("$id", id);
    cmd.Parameters.AddWithValue("$nom", nombre);
    cmd.Parameters.AddWithValue("$ape", apellido);
    cmd.Parameters.AddWithValue("$email", email);
    cmd.Parameters.AddWithValue("$hash", hash);
    cmd.Parameters.AddWithValue("$rol", rol);
    cmd.Parameters.AddWithValue("$suc", suc);
    cmd.ExecuteNonQuery();
    Console.WriteLine($"  Inserted: {email} / {(id == 1 ? "Admin123!" : id == 2 ? "Vendedor123!" : "Gerente123!")}");
}

// 4. Verify
Console.WriteLine("\n=== AFTER ===");
cmd.CommandText = "SELECT Id, Email, Nombre, PasswordHash FROM Usuario ORDER BY Id";
cmd.Parameters.Clear();
using (var r = cmd.ExecuteReader())
    while (r.Read())
    {
        var hash = (string)r[3];
        Console.Write($"  Id={r[0]} Email={r[1]} Nombre={r[2]} Hash={hash[..15]}... ");
        foreach (var pw in new[] { "Admin123!", "Vendedor123!", "Gerente123!" })
        {
            if (BC.Verify(pw, hash)) { Console.Write($"✓ {pw}"); break; }
        }
        Console.WriteLine();
    }

conn.Close();
Console.WriteLine("\nDone!");
