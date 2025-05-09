using ApiECommerce.Modelo;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiECommerce.DTOs;

namespace ApiECommerce.IServices
{
    public interface IClienteService
    {
        Task<ResultadoClientes> ObtenerClientesAsync(string? nombre = null, int pageNumber = 1, int pageSize = 10);
        Task<Cliente> ObtenerClienteAsync(int id);
        Task<bool> CrearClienteAsync(Cliente cliente);
        Task<bool> ActualizarClienteAsync(Cliente cliente);
        Task<bool> EliminarClienteAsync(int id);
    }
}