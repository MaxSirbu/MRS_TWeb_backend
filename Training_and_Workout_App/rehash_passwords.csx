// Script temporar — rehash-uieste parolele plain-text din DB
// Rulare: dotnet script rehash_passwords.csx (sau executa logica direct din program)
// FOLOSIT O SINGURA DATA pentru migrarea parolelor vechi

using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;

record UserRow(int Id, string Username, string PlainPassword);

var usersToUpdate = new List<UserRow>
{
    new(1, "",                   "parola123"),
    new(2, "nichita@example.com","nichita123"),
    new(3, "test@test.com",      "parola123"),
    new(4, "nichita@gmail.com",  "nichita123"),
};

// PasswordHasher nu depinde de Entity — folosim un User dummy
var hasher = new PasswordHasher<object>();

var connStr = "Server=.\\SQLEXPRESS;Database=Training_DB;Trusted_Connection=True;TrustServerCertificate=True";
using var conn = new SqlConnection(connStr);
conn.Open();

foreach (var u in usersToUpdate)
{
    var hash = hasher.HashPassword(new object(), u.PlainPassword);
    using var cmd = new SqlCommand("UPDATE Users SET Password = @h WHERE Id = @id", conn);
    cmd.Parameters.AddWithValue("@h", hash);
    cmd.Parameters.AddWithValue("@id", u.Id);
    int rows = cmd.ExecuteNonQuery();
    Console.WriteLine($"Updated Id={u.Id} ({u.Username}): {rows} row(s)");
}

Console.WriteLine("Done! All passwords re-hashed.");
