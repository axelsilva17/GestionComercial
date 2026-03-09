
using GestionComercial.Dominio.Entidades.Cliente;
using Microsoft.EntityFrameworkCore;

public static class SemillaCliente
    {
        public static void Sembrar(ModelBuilder builder)
        {
            builder.Entity<Cliente>().HasData(
                new Cliente { Id = 1, Nombre = "Consumidor Final",  Documento = 0,        Telefono = null,         Email = null,                    Id_empresa = 1 },
                new Cliente { Id = 2, Nombre = "Juan Pérez",        Documento = 30111111, Telefono = "3794555001", Email = "juan@gmail.com",         Id_empresa = 1 },
                new Cliente { Id = 3, Nombre = "María González",    Documento = 32222222, Telefono = "3794555002", Email = "maria@gmail.com",        Id_empresa = 1 },
                new Cliente { Id = 4, Nombre = "Carlos Rodríguez",  Documento = 28333333, Telefono = "3794555003", Email = "carlos@empresa.com",     Id_empresa = 1 },
                new Cliente { Id = 5, Nombre = "Laura Martínez",    Documento = 35444444, Telefono = "3794555004", Email = "laura@gmail.com",        Id_empresa = 1 },
                new Cliente { Id = 6, Nombre = "Constructora ABC",  Documento = 30555555, Telefono = "3794555005", Email = "compras@constructora.com",Id_empresa = 1 }
            );
        }
    }