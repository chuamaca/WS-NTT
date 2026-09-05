using BiblioNET.Consola.Models;
using Microsoft.Data.SqlClient;

namespace BiblioNET.Consola.Data;

internal static class LibroReaderMapper
{
    public static Libro Map(SqlDataReader reader)
    {
        return new Libro
        {
            LibroId = reader.GetInt32(reader.GetOrdinal("LibroId")),
            GeneroId = reader.GetInt32(reader.GetOrdinal("GeneroId")),
            Genero = reader.GetString(reader.GetOrdinal("Genero")),
            Titulo = reader.GetString(reader.GetOrdinal("Titulo")),
            Autor = reader.GetString(reader.GetOrdinal("Autor")),
            ISBN = reader.GetString(reader.GetOrdinal("ISBN")),
            Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
            Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
        };
    }
}
