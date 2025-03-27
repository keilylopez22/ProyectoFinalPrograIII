/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace ApiECommerce.Data
{
    public class ECommersContext : DBContext
    {
        public DBSet<Proveedor> Proveedores {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "server=sql5.freesqldatabase.com;port=3306;database=sql5769382;user=sql5769382;password=AdGnpPleA8;";

        optionsBuilder.UseMySql(connectionString);
        // O si usas PostgreSQL: optionsBuilder.UseNpgsql("TuConnectionStringAqui");

        
       
    }
        
    }
}*/