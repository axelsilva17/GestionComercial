using GestionComercial.Dominio.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Persistencia.Semillas
{
    public static class SemillaPermisos
    {
        public static void Sembrar(ModelBuilder builder)
        {
            builder.Entity<Permiso>().HasData(
                new Permiso { Id = 1,  Nombre = "Ventas.Ver",         Descripcion = "Ver ventas"          },
                new Permiso { Id = 2,  Nombre = "Ventas.Crear",       Descripcion = "Crear ventas"        },
                new Permiso { Id = 3,  Nombre = "Ventas.Anular",      Descripcion = "Anular ventas"       },
                new Permiso { Id = 4,  Nombre = "Compras.Ver",        Descripcion = "Ver compras"         },
                new Permiso { Id = 5,  Nombre = "Compras.Crear",      Descripcion = "Crear compras"       },
                new Permiso { Id = 6,  Nombre = "Productos.Ver",      Descripcion = "Ver productos"       },
                new Permiso { Id = 7,  Nombre = "Productos.Crear",    Descripcion = "Crear productos"     },
                new Permiso { Id = 8,  Nombre = "Productos.Editar",   Descripcion = "Editar productos"    },
                new Permiso { Id = 9,  Nombre = "Clientes.Ver",       Descripcion = "Ver clientes"        },
                new Permiso { Id = 10, Nombre = "Clientes.Crear",     Descripcion = "Crear clientes"      },
                new Permiso { Id = 11, Nombre = "Reportes.Ver",       Descripcion = "Ver reportes"        },
                new Permiso { Id = 12, Nombre = "Caja.Abrir",         Descripcion = "Abrir caja"          },
                new Permiso { Id = 13, Nombre = "Caja.Cerrar",        Descripcion = "Cerrar caja"         },
                new Permiso { Id = 14, Nombre = "Configuracion.Ver",  Descripcion = "Ver configuración"   },
                new Permiso { Id = 15, Nombre = "Usuarios.Gestionar", Descripcion = "Gestionar usuarios"  }
            );

            // Gerente - todos los permisos
            builder.Entity<RolPermiso>().HasData(
                Enumerable.Range(1, 15).Select(i =>
                    new RolPermiso { Id = i, Id_rol = 1, Id_permiso = i }).ToArray()
            );

            // Administrador - todo menos Usuarios.Gestionar
            builder.Entity<RolPermiso>().HasData(
                Enumerable.Range(1, 14).Select(i =>
                    new RolPermiso { Id = 15 + i, Id_rol = 2, Id_permiso = i }).ToArray()
            );

            // Vendedor
            builder.Entity<RolPermiso>().HasData(
                new RolPermiso { Id = 30, Id_rol = 3, Id_permiso = 1  },
                new RolPermiso { Id = 31, Id_rol = 3, Id_permiso = 2  },
                new RolPermiso { Id = 32, Id_rol = 3, Id_permiso = 6  },
                new RolPermiso { Id = 33, Id_rol = 3, Id_permiso = 9  },
                new RolPermiso { Id = 34, Id_rol = 3, Id_permiso = 10 },
                new RolPermiso { Id = 35, Id_rol = 3, Id_permiso = 12 },
                new RolPermiso { Id = 36, Id_rol = 3, Id_permiso = 13 }
            );
        }
    }
}
