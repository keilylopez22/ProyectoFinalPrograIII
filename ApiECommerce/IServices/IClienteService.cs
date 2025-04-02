using ProyectoFinal_PrograIII.Modelo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoFinal_PrograIII.ApiECommerce.IServices
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> ObtenerClientesAsync();
        Task<Cliente> ObtenerClienteAsync(int id);
        Task<bool> CrearClienteAsync(Cliente cliente);
        Task<bool> ActualizarClienteAsync(Cliente cliente);
        Task<bool> EliminarClienteAsync(int id);
    }
}