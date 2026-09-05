using System.Data;
using BiblioNET.Api.Dtos;
using Dapper;

namespace BiblioNET.Api.Data.Dapper;

public sealed class PrestamoDapperRepository(
    ISqlConnectionFactory connectionFactory)
{
    public async Task<int> CrearAsync(CrearPrestamoRequest request)
    {
        await using var connection = connectionFactory.Create();
        await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var stock = await connection.QuerySingleOrDefaultAsync<int?>(
                """
                SELECT Stock
                FROM dbo.Libros
                WHERE LibroId = @LibroId
                  AND Activo = 1;
                """,
                new { request.LibroId },
                transaction);

            if (stock is null)
                throw new InvalidOperationException("El libro no existe.");

            if (stock < request.Cantidad)
                throw new InvalidOperationException("No existe stock suficiente.");

            var prestamoId = await connection.QuerySingleAsync<int>(
                """
                INSERT INTO dbo.Prestamos
                    (LectorId, FechaPrestamo, FechaVencimiento, Estado)
                VALUES
                    (@LectorId, CAST(GETDATE() AS DATE),
                     DATEADD(DAY, @DiasPrestamo, CAST(GETDATE() AS DATE)),
                     'ACTIVO');

                SELECT CAST(SCOPE_IDENTITY() AS INT);
                """,
                new
                {
                    request.LectorId,
                    request.DiasPrestamo
                },
                transaction);

            await connection.ExecuteAsync(
                """
                INSERT INTO dbo.PrestamoDetalle
                    (PrestamoId, LibroId, Cantidad, Devuelto)
                VALUES
                    (@PrestamoId, @LibroId, @Cantidad, 0);

                UPDATE dbo.Libros
                SET Stock = Stock - @Cantidad
                WHERE LibroId = @LibroId;
                """,
                new
                {
                    PrestamoId = prestamoId,
                    request.LibroId,
                    request.Cantidad
                },
                transaction);

            await transaction.CommitAsync();
            return prestamoId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
