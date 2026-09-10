using GestionComercial.Dominio.Entidades.Seguridad;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
                new Permiso { Id = 15, Nombre = "Usuarios.Gestionar", Descripcion = "Gestionar usuarios"  },
                new Permiso { Id = 16, Nombre = "Descuentos.Ver",     Descripcion = "Ver descuentos"       },
                new Permiso { Id = 17, Nombre = "Caja.Auditoria",     Descripcion = "Auditoría de caja"    }
            );

            // Gerente - todos los permisos
            builder.Entity<RolPermiso>().HasData(
                Enumerable.Range(1, 16).Select(i =>
                    new RolPermiso { Id = i, Id_rol = 1, Id_permiso = i }).ToArray()
            );

            // Administrador - todo menos Usuarios.Gestionar
            builder.Entity<RolPermiso>().HasData(
                Enumerable.Range(1, 15).Select(i =>
                    new RolPermiso { Id = 16 + i, Id_rol = 2, Id_permiso = i }).ToArray()
            );
            // Administrador - permiso Id=16 (Descuentos.Ver)
            builder.Entity<RolPermiso>().HasData(
                new RolPermiso { Id = 32, Id_rol = 2, Id_permiso = 16 }
            );

            // Vendedor
            builder.Entity<RolPermiso>().HasData(
                new RolPermiso { Id = 37, Id_rol = 3, Id_permiso = 1  },
                new RolPermiso { Id = 38, Id_rol = 3, Id_permiso = 2  },
                new RolPermiso { Id = 39, Id_rol = 3, Id_permiso = 6  },
                new RolPermiso { Id = 40, Id_rol = 3, Id_permiso = 9  },
                new RolPermiso { Id = 41, Id_rol = 3, Id_permiso = 10 },
                new RolPermiso { Id = 42, Id_rol = 3, Id_permiso = 12 },
                new RolPermiso { Id = 43, Id_rol = 3, Id_permiso = 13 }
            );
        }

        /// <summary>
        /// Reconcilia los permisos semilla en una BD existente (idempotente, solo ADD).
        /// SOLO garantiza el par (1,17): Gerente → Caja.Auditoria. El permiso 17 existe
        /// desde la migración 20260903231401, pero NINGUNA migración inserta su par de
        /// RolPermiso, así que este es el único par que el reconcile debe asegurar.
        /// El resto de los pares de Sembrar() ya quedan cubiertos por las migraciones;
        /// reconciliarlos de nuevo RE-INSERTABA permisos que el usuario había quitado
        /// (bug original: los permisos se revertían al reiniciar).
        /// </summary>
        public static async Task ReconciliarPermisosSemillaAsync(
            GestionComercial.Persistencia.Contexto.GestionComercialContext context,
            CancellationToken ct = default)
        {
            try
            {
                // Pares existentes (Id_rol, Id_permiso) para roles 1..3
                var existentes = await context.RolPermisos
                    .Where(rp => rp.Id_rol >= 1 && rp.Id_rol <= 3)
                    .Select(rp => new { rp.Id_rol, rp.Id_permiso })
                    .ToListAsync(ct);

                var existentesSet = new HashSet<(int rol, int permiso)>(existentes.Select(x => (x.Id_rol, x.Id_permiso)));

                // Único par que no cubre ninguna migración: Gerente (1) → Caja.Auditoria (17)
                var semilla = new List<(int rol, int permiso)>
                {
                    (1,17)
                };

                var faltantes = semilla.Where(p => !existentesSet.Contains(p)).ToList();

                if (faltantes.Count > 0)
                {
                    foreach (var (rol, permiso) in faltantes)
                    {
                        context.RolPermisos.Add(new RolPermiso
                        {
                            Id_rol = rol,
                            Id_permiso = permiso
                        });
                    }
                    await context.SaveChangesAsync(ct);
                    Debug.WriteLine($"[SemillaPermisos] Reconciliados {faltantes.Count} permisos faltantes");
                }
                else
                {
                    Debug.WriteLine("[SemillaPermisos] Permisos ya sincronizados");
                }
            }
            catch (Exception ex)
            {
                // Nunca crashear el arranque por esto
                Debug.WriteLine($"[SemillaPermisos] Error reconciliando: {ex.Message}");
            }
        }
    }
}
