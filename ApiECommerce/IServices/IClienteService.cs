using ApiECommerce.Modelo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiECommerce.IServices
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> ObtenerClientesAsync(string? nombre = null, int pageNumber = 1, int pageSize = 10);
        Task<Cliente> ObtenerClienteAsync(int id);
        Task<bool> CrearClienteAsync(Cliente cliente);
        Task<bool> ActualizarClienteAsync(Cliente cliente);
        Task<bool> EliminarClienteAsync(int id);
    }
}