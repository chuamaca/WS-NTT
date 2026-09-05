using BiblioNET.Api.Dtos;

namespace BiblioNET.Api.Data.Dapper;

public interface IBenchmarkDapperRepository
{
    Task<long> ContarSinIndiceAsync(
        int generoId,
        int stock);

    Task<long> ContarConIndiceAsync(
        int generoId,
        int stock);

    Task<IReadOnlyList<BenchmarkLibroDto>> ConsultarSinIndiceAsync(
        int generoId,
        int stock);

    Task<IReadOnlyList<BenchmarkLibroDto>> ConsultarConIndiceAsync(
        int generoId,
        int stock);
}