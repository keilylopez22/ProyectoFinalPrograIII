/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Data;
using YourNamespace.Models;
using YourNamespace.DTOs;

namespace YourNamespace.Services
{
    public class CompraServicio : ICompraServicio
    {
        private readonly YourDbContext _context;

        public CompraServicio(YourDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ActualizarComprasAsync(int id, CompraDTO compraDto) // Changed Compra compra to int id, CompraDTO compraDto
        {
            var compraExistente = await _context.compras
                                        .Include(c => c.DetalleCompras)
                                        .FirstOrDefaultAsync(c => c.Id == id);

            if (compraExistente == null)
            {
                return false; // O manejar como error NotFound
            }

            // Actualizar propiedades de la compra
            compraExistente.Fecha = compraDto.Fecha;
            compraExistente.IdProveedor = compraDto.IdProveedor;
            // compraExistente.Estado = "Actualizada"; // O el estado que corresponda

            // Eliminar detalles existentes
            _context.DetalleCompra.RemoveRange(compraExistente.DetalleCompras);

            // Crear nuevos detalles y calcular el total
            double nuevoTotal = 0;
            var nuevosDetalles = new List<DetalleCompra>();
            foreach (var detalleDto in compraDto.DetalleCompras)
            {
                var producto = await _context.productos.FindAsync(detalleDto.IdProductos);
                if (producto == null)
                {
                    // Manejar el caso de producto no encontrado, tal vez lanzar una excepción o retornar un error específico
                    return false; 
                }
                // No se actualiza el stock ni el precio del producto en la actualización de la compra,
                // eso debería ser parte de la lógica de negocio si se permite modificar detalles que afecten inventario.
                // Aquí solo reflejamos los datos de la compra actualizada.

                double precioUnitario = detalleDto.PrecioUnitario;
                double subTotal = precioUnitario * detalleDto.CantidadProductos;
                nuevoTotal += subTotal;

                nuevosDetalles.Add(new DetalleCompra
                {
                    IdCompra = compraExistente.Id, // Asegurar que el IdCompra está asignado
                    IdProductos = detalleDto.IdProductos,
                    CantidadProductos = detalleDto.CantidadProductos,
                    PrecioUnitario = (decimal)precioUnitario,
                    SubTotal = (decimal)subTotal
                });
            }

            compraExistente.DetalleCompras = nuevosDetalles;
            compraExistente.Total = nuevoTotal;

            _context.compras.Update(compraExistente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarComprasAsync(int id)
        {
            var compra = await _context.compras.FindAsync(id);
            if (compra == null)
            {
                return false;
            }

            _context.compras.Remove(compra);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}*/