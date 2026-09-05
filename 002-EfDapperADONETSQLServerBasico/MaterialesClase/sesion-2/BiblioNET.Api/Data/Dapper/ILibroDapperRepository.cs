using BiblioNET.Api.Dtos;

namespace BiblioNET.Api.Data.Dapper;

public interface ILibroDapperRepository
{
    Task<IReadOnlyList<LibroDto>> ListarAsync();
    Task<LibroDto?> ObtenerPorIdAsync(int libroId);
    Task<int> InsertarAsync(LibroRequest request);
    Task<int> ActualizarAsync(int libroId, LibroRequest request);
    Task<int> EliminarAsync(int libroId);
}
