using GestionComercial.Aplicacion.Excepciones;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Dominio.Entidades.Caja;
using GestionComercial.Dominio.Interfaces;

namespace GestionComercial.Aplicacion.Servicios
{
    public class CajaServicio : ICajaServicio
    {
        private readonly IUnitOfWork _uow;
        public CajaServicio(IUnitOfWork uow) => _uow = uow;

        public async Task<Caja?> ObtenerCajaAbiertaAsync(int idSucursal)
            => await _uow.Cajas.ObtenerCajaAbiertaAsync(idSucursal);

        public async Task<Caja> AbrirCajaAsync(int idSucursal, int idUsuario, decimal montoInicial)
        {
            var cajaExistente = await _uow.Cajas.ObtenerCajaAbiertaAsync(idSucursal);
            if (cajaExistente != null)
                throw new NegocioException("Ya existe una caja abierta para esta sucursal");

            var caja = new Caja
            {
                FechaApertura = DateTime.Now,
                MontoInicial = montoInicial,
                MontoFinal = montoInicial,
                Estado = 1, // 1=Abierta
                Id_sucursal = idSucursal,
                UsuarioApertura_id = idUsuario,
            };

            await _uow.Cajas.AgregarAsync(caja);
            await _uow.GuardarCambiosAsync();
            return caja;
        }

        public async Task<Caja> CerrarCajaAsync(int idCaja, int idUsuario, decimal montoFinal)
        {
            var caja = await _uow.Cajas.ObtenerPorIdAsync(idCaja)
                ?? throw new CajaNoAbiertaException();

            if (!caja.EstaAbierta)
                throw new CajaNoAbiertaException();

            caja.FechaCierre = DateTime.Now;
            caja.MontoFinal = montoFinal;
            caja.Estado = 2; // 2=Cerrada
            caja.UsuarioCierre_id = idUsuario;

            _uow.Cajas.Actualizar(caja);
            await _uow.GuardarCambiosAsync();
            return caja;
        }

        public async Task RegistrarMovimientoAsync(int idCaja, TipoMovimientoCajaEnum tipo, decimal monto, string descripcion)
        {
            var caja = await _uow.Cajas.ObtenerPorIdAsync(idCaja)
                ?? throw new CajaNoAbiertaException();

            if (!caja.EstaAbierta)
                throw new CajaNoAbiertaException();

            var movimiento = new TipoMovimientoCaja
            {
                Id_caja = idCaja,
                Tipo = (int)tipo,
                Monto = monto,
                Fecha = DateTime.Now,
                Concepto = descripcion,
            };

            await _uow.MovimientosCaja.AgregarAsync(movimiento);

            caja.MontoFinal += tipo == TipoMovimientoCajaEnum.Ingreso ? monto : -monto;
            _uow.Cajas.Actualizar(caja);
            await _uow.GuardarCambiosAsync();
        }
    }
}