using _004.Entregable04_Dapper.Model;

namespace _004.Entregable04_Dapper.Data;

public interface IClienteDapperRepository
{
    Task<IReadOnlyList<Cliente>> ListarAsync();
    Task<Cliente?> ObtenerPorIdAsync(int idCliente);
    Task<int> InsertarAsync(ClienteRequest request);
    Task<int> ActualizarAsync(int idCliente, ClienteRequest request);
    Task<int> EliminarAsync(int idCliente);
}
