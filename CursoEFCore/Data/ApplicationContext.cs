using CursoEFCore.Data.Configurations;
using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CursoEFCore.Data
{
    class ApplicationContext : DbContext
    {
        public static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(p => p.AddConsole());
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(_loggerFactory)
                .EnableSensitiveDataLogging()
                .UseSqlServer("Server=SUPORTE\\SQLEXPRESS;Database=CursoEFCore;User Id=suporte;Password=suporte;", 
                p=>p.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null));
            // ou Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
            MapearPropriedadesEsquecidas(modelBuilder);
        }

        private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder)
        {
            foreach(var entity in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entity.GetProperties().Where(p => p.ClrType == typeof(string));

                foreach(var property in properties)
                {
                    if (string.IsNullOrEmpty(property.GetColumnType()) && !property.GetMaxLength().HasValue)
                    {
                        property.SetColumnType("VARCHAR(100)");
                    } 
                }
            }
        }


    }
}
