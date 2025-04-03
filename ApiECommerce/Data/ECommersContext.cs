/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiECommerce.Modelo;
using Microsoft.EntityFrameworkCore;
namespace ApiECommerce.Data
{
    public class ECommersContext : DbContext
    {
        //public DbSet<Proveedor> Proveedores {get; set;}

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "server=sql5.freesqldatabase.com;port=3306;database=sql5769382;user=sql5769382;password=AdGnpPleA8;";
        //la siguiente linea me la dio chat
        optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30)));
        //optionsBuilder.UseMySql(connectionString);
        // O si usas PostgreSQL: optionsBuilder.UseNpgsql("TuConnectionStringAqui");

        
       
    }
        /*public ECommersContext(DbContextOptions<ECommersContext> options) : base(options)
        {
        }
        public DbSet<Proveedor> Proveedores { get; set; } // Cambia 'DBSet' a 'DbSet'

    }
}*/