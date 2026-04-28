using System;
using System.Linq;
using System.Threading.Tasks;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Entidades.Proveedores;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades;
using System.Collections.Generic;
using GestionComercial.Dominio.Entidades;
using GestionComercial.Aplicacion.Interfaces.Servicios;

namespace GestionComercial.Aplicacion.Servicios
{
    public class ProveedorCostoService : IProveedorCostoServicio
    {
        private readonly IUnitOfWork _uow;
        public ProveedorCostoService(IUnitOfWork uow) => _uow = uow;

        public async Task<(int Nuevos, int Actualizados)> AjusteCostoProveedorAsync(int idProveedor, decimal porcentaje)
        {
            if (idProveedor <= 0) throw new ArgumentException("ID de proveedor inválido.", nameof(idProveedor));
            if (porcentaje == 0) return (0, 0);

            var proveedor = await _uow.Proveedores.ObtenerPorIdAsync(idProveedor) ?? throw new KeyNotFoundException($"Proveedor {idProveedor} no encontrado");

            var existentes = await _uow.ProveedoresCostos.ObtenerPorProveedorAsync(idProveedor);
            int nuevos = 0, actualizados = 0;

            if (existentes != null && existentes.Any())
            {
                foreach (var costo in existentes)
                {
                    costo.Costo = costo.Costo * (1 + porcentaje / 100m);
                    _uow.ProveedoresCostos.Actualizar(costo);
                    actualizados++;
                }
            }
            else
            {
                var productos = await _uow.Productos.ObtenerPorEmpresaAsync(proveedor.Id_empresa);
                foreach (var p in productos)
                {
                    var nuevo = ProveedorProductoCosto.Crear(idProveedor, p.Id, p.PrecioCostoActual);
                    await _uow.ProveedoresCostos.AgregarAsync(nuevo);
                    nuevos++;
                }
            }

            await _uow.GuardarCambiosAsync();
            return (nuevos, actualizados);
        }
    }
}
