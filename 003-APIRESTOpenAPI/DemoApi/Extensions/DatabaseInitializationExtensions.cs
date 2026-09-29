using DemoApi.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Extensions;

public static class DatabaseInitializationExtensions
{
    public static async Task EnsureDatabaseSchemaAsync(this AppDbContext db)
    {
        await db.Database.EnsureCreatedAsync();

        await db.Database.ExecuteSqlRawAsync("""
            CREATE TABLE IF NOT EXISTS Categories (
                Id INTEGER NOT NULL CONSTRAINT PK_Categories PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Description TEXT NULL
            );
            """);

        await db.Database.ExecuteSqlRawAsync(
            "CREATE UNIQUE INDEX IF NOT EXISTS IX_Categories_Name ON Categories (Name);");

        try
        {
            await db.Database.ExecuteSqlRawAsync(
                "ALTER TABLE Products ADD COLUMN CategoryId INTEGER NOT NULL DEFAULT 1;");
        }
        catch (SqliteException exception) when (exception.SqliteErrorCode == 1)
        {
            // The column already exists in a database created with the current model.
        }

        await db.Database.ExecuteSqlRawAsync("""
            INSERT INTO Categories (Id, Name, Description)
            SELECT 1, 'General', 'Categoría predeterminada para productos existentes.'
            WHERE NOT EXISTS (SELECT 1 FROM Categories WHERE Id = 1);
            """);
    }
}