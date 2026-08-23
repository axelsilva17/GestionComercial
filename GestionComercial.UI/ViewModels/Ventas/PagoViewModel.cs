using Caliburn.Micro;
using GestionComercial.Aplicacion.DTOs.Ventas;
using GestionComercial.Aplicacion.Interfaces.Servicios;
using GestionComercial.Aplicacion.Servicios;
using GestionComercial.Dominio.Entidades.Descuento;
using GestionComercial.Dominio.Entidades.Producto;
using GestionComercial.Dominio.Entidades.Ventas;
using GestionComercial.Dominio.Interfaces;
using GestionComercial.Dominio.Interfaces.Repositorios;
using GestionComercial.Dominio.Interfaces.Servicios;
using GestionComercial.UI.ViewModels.Base;
using GestionComercial.UI.ViewModels.Main;
using GestionComercial.UI.Views.Comandos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GestionComercial.UI.ViewModels.Ventas
{
    public class PagoViewModel : NavigableViewModel
    {
        private readonly IVentaServicio _ventaServicio;
        private readonly IUnitOfWork    _uow;
        private readonly SesionServicio _sesion;
        private readonly IDescuentoConfiguracionServicio _descuentoConfiguracionServicio;

        private int _idVenta;
        private Venta? _ventaCompleta;
        private List<DescuentoConfiguracion>? _descuentosCache;
        private Dictionary<int, Categoria>? _categoriasCache;

        // ── Jerarquía de métodos de pago ─────────────────────────────────────
        private PagoNodoJerarquico? _nodoRaiz;
        private PagoNodoJerarquico? _nivelActual;
        public PagoNodoJerarquico? NivelActual
        {
            get => _nivelActual;
            set { _nivelActual = value; NotifyOfPropertyChange(() => NivelActual); NotifyOfPropertyChange(() => Breadcrumb); NotifyOfPropertyChange(() => EsNivelRaiz); }
        }

        private ObservableCollection<PagoNodoJerarquico> _nodosVisibles = new();
        public ObservableCollection<PagoNodoJerarquico> NodosVisibles
        {
            get => _nodosVisibles;
            set { _nodosVisibles = value; NotifyOfPropertyChange(() => NodosVisibles); }
        }

        public string Breadcrumb
        {
            get
            {
                var partes = new List<string>();
                var nodo = NivelActual;
                while (nodo != null)
                {
                    partes.Insert(0, nodo.Nombre);
                    nodo = nodo.Padre;
                }
                return string.Join(" → ", partes);
            }
        }

        public bool EsNivelRaiz => NivelActual == null || NivelActual == _nodoRaiz;

        public ICommand VolverCommand { get; }

        public PagoViewModel(IVentaServicio ventaServicio, IUnitOfWork uow, SesionServicio sesion, IDescuentoConfiguracionServicio descuentoConfiguracionServicio)
        {
            _ventaServicio = ventaServicio;
            _uow           = uow;
            _sesion        = sesion;
            _descuentoConfiguracionServicio = descuentoConfiguracionServicio;
            Titulo         = "Cobrar Venta";
            VolverCommand  = new RelayCommand(() => Volver());
        }

        ///         /// Maneja atajos de teclado globales en la vista de pago.
        public void HandleKeyDown(Key key, ModifierKeys modifiers)
        {
            if (modifiers != ModifierKeys.None) return;

            switch (key)
            {
                case Key.F1:
                    var efectivo = NodosVisibles.FirstOrDefault(n => n.Nombre == "Efectivo");
                    if (efectivo != null) SeleccionarNodo(efectivo);
                    break;
                case Key.F2:
                    var tarjeta = NodosVisibles.FirstOrDefault(n => n.Nombre == "Tarjeta");
                    if (tarjeta != null) SeleccionarNodo(tarjeta);
                    break;
                case Key.F3:
                    Volver();
                    break;
                case Key.F4:
                    Volver();
                    break;
                case Key.F6:
                    if (PuedeCobrar) _ = Confirmar();
                    break;
                case Key.Escape:
                    if (!EsNivelRaiz)
                        Volver();
                    else
                        _ = Cancelar();
                    break;
            }
        }

        // ── Datos de la venta ─────────────────────────────────────────────────
        private string _clienteNombre = "Consumidor Final";
        public string ClienteNombre
        {
            get => _clienteNombre;
            set { _clienteNombre = value; NotifyOfPropertyChange(() => ClienteNombre); }
        }

        private decimal _totalVenta;
        public decimal TotalVenta
        {
            get => _totalVenta;
            set { _totalVenta = value; NotifyOfPropertyChange(() => TotalVenta); RecalcularVuelto(); }
        }

        private decimal _totalPagado;
        public decimal TotalPagado
        {
            get => _totalPagado;
            set
            {
                _totalPagado = value;
                NotifyOfPropertyChange(() => TotalPagado);
                NotifyOfPropertyChange(() => Faltante);
                NotifyOfPropertyChange(() => HayFaltante);
                NotifyOfPropertyChange(() => PuedeCobrar);
                RecalcularVuelto();
                // Notificaciones explícitas para asegurar que el binding se actualice
                NotifyOfPropertyChange(() => Vuelto);
                NotifyOfPropertyChange(() => HayVuelto);
            }
        }

        public decimal Faltante => Math.Max(TotalVenta - TotalPagado, 0);

        private decimal _vuelto;
        public decimal Vuelto
        {
            get => _vuelto;
            set { _vuelto = value; NotifyOfPropertyChange(() => Vuelto); NotifyOfPropertyChange(() => HayVuelto); }
        }
        public bool HayVuelto => Vuelto > 0;

        public bool HayFaltante => Faltante > 0;

        public bool PuedeCobrar => TotalPagado >= TotalVenta && Pagos.Any();

        // ── Métodos de pago disponibles (combo) ───────────────────────────────
        private ObservableCollection<PagoItemDto> _metodosPago = new();
        public ObservableCollection<PagoItemDto> MetodosPago
        {
            get => _metodosPago;
            set { _metodosPago = value; NotifyOfPropertyChange(() => MetodosPago); }
        }

        private PagoItemDto? _metodoSeleccionado;
        public PagoItemDto? MetodoSeleccionado
        {
            get => _metodoSeleccionado;
            set { _metodoSeleccionado = value; NotifyOfPropertyChange(() => MetodoSeleccionado); }
        }

        private string _montoIngresado = string.Empty;
        public string MontoIngresado
        {
            get => _montoIngresado;
            set { _montoIngresado = value; NotifyOfPropertyChange(() => MontoIngresado); }
        }

        // ── Líneas de pago seleccionadas ──────────────────────────────────────
        private ObservableCollection<PagoLineaVm> _pagos = new();
        public ObservableCollection<PagoLineaVm> Pagos
        {
            get => _pagos;
            set { _pagos = value; NotifyOfPropertyChange(() => Pagos); }
        }

        // ── Lifecycle ─────────────────────────────────────────────────────────
        protected override async Task OnActivateAsync(CancellationToken cancellationToken)
            => await CargarMetodosAsync();

        private async Task CargarMetodosAsync()
        {
            try
            {
                var sucursal  = await _uow.Sucursales.ObtenerPorIdAsync(_sesion.IdSucursal);
                if (sucursal == null)
                {
                    MostrarError("No se encontró la sucursal configurada.");
                    return;
                }
                var idEmpresa = sucursal.Id_empresa;
                if (idEmpresa <= 0)
                {
                    MostrarError("ID de empresa inválido.");
                    return;
                }
                var metodos   = await _uow.MetodosPago.ObtenerTodosPorEmpresaAsync(idEmpresa);
                if (metodos == null)
                {
                    MetodosPago = new ObservableCollection<PagoItemDto>();
                    return;
                }

                MetodosPago = new ObservableCollection<PagoItemDto>(
                    metodos.Where(m => m.Activo).Select(m => new PagoItemDto
                    {
                        IdMetodoPago = m.Id,
                        NombreMetodo = m.Nombre,
                        Categoria    = m.Categoria ?? "Otro",
                        Subcategoria = m.Subcategoria,
                        Monto        = 0,
                    }));

                // Construir árbol jerárquico
                _nodoRaiz = new PagoNodoJerarquico { Id = 0, Nombre = "Raíz", EsHoja = false };

                // Nivel 1: Efectivo (hoja) | Transferencia (hoja) | Tarjeta (no hoja)
                var efectivo = MetodosPago.FirstOrDefault(m => m.Categoria == "Efectivo");
                if (efectivo != null)
                {
                    _nodoRaiz.Hijos.Add(new PagoNodoJerarquico
                    {
                        Id = efectivo.IdMetodoPago,
                        Nombre = "Efectivo",
                        EsHoja = true,
                        MetodoPagoId = efectivo.IdMetodoPago,
                        Icono = "💵",
                        Padre = _nodoRaiz
                    });
                }

                var transferencia = MetodosPago.FirstOrDefault(m => m.Categoria == "Transferencia");
                if (transferencia != null)
                {
                    _nodoRaiz.Hijos.Add(new PagoNodoJerarquico
                    {
                        Id = transferencia.IdMetodoPago,
                        Nombre = "Transferencia",
                        EsHoja = true,
                        MetodoPagoId = transferencia.IdMetodoPago,
                        Icono = "🏦",
                        Padre = _nodoRaiz
                    });
                }

                var tarjetas = MetodosPago.Where(m => m.Categoria == "Tarjeta").ToList();
                if (tarjetas.Any())
                {
                    var nodoTarjeta = new PagoNodoJerarquico
                    {
                        Id = -1,
                        Nombre = "Tarjeta",
                        EsHoja = false,
                        Icono = "💳",
                        Padre = _nodoRaiz
                    };

                    // Nivel 2: Débito (no hoja) | Crédito (no hoja)
                    var debitos = tarjetas.Where(m => m.Subcategoria == "Debito").ToList();
                    if (debitos.Any())
                    {
                        var nodoDebito = new PagoNodoJerarquico
                        {
                            Id = -2,
                            Nombre = "Débito",
                            EsHoja = false,
                            Icono = "💳",
                            Padre = nodoTarjeta
                        };

                        // Nivel 3: tarjetas específicas débito
                        foreach (var tarjeta in debitos)
                        {
                            nodoDebito.Hijos.Add(new PagoNodoJerarquico
                            {
                                Id = tarjeta.IdMetodoPago,
                                Nombre = tarjeta.NombreMetodo,
                                EsHoja = true,
                                MetodoPagoId = tarjeta.IdMetodoPago,
                                Icono = "💳",
                                Padre = nodoDebito
                            });
                        }

                        nodoTarjeta.Hijos.Add(nodoDebito);
                    }

                    var creditos = tarjetas.Where(m => m.Subcategoria == "Credito").ToList();
                    if (creditos.Any())
                    {
                        var nodoCredito = new PagoNodoJerarquico
                        {
                            Id = -3,
                            Nombre = "Crédito",
                            EsHoja = false,
                            Icono = "💳",
                            Padre = nodoTarjeta
                        };

                        // Nivel 3: tarjetas específicas crédito
                        foreach (var tarjeta in creditos)
                        {
                            nodoCredito.Hijos.Add(new PagoNodoJerarquico
                            {
                                Id = tarjeta.IdMetodoPago,
                                Nombre = tarjeta.NombreMetodo,
                                EsHoja = true,
                                MetodoPagoId = tarjeta.IdMetodoPago,
                                Icono = "💳",
                                Padre = nodoCredito
                            });
                        }

                        nodoTarjeta.Hijos.Add(nodoCredito);
                    }

                    _nodoRaiz.Hijos.Add(nodoTarjeta);
                }

                // Mostrar nivel 1
                NivelActual = _nodoRaiz;
                NodosVisibles = new ObservableCollection<PagoNodoJerarquico>(_nodoRaiz.Hijos);

                MetodoSeleccionado = MetodosPago.FirstOrDefault(m => m.Categoria == "Efectivo")
                                  ?? MetodosPago.FirstOrDefault();

                // Precompletar con el total en el campo de monto
                MontoIngresado = TotalVenta.ToString("F2");
            }
            catch (Exception ex)
            {
                var mensaje = $"Error al cargar métodos de pago: {ex.Message}";
                MostrarError(mensaje);
                System.Windows.MessageBox.Show(mensaje, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        // ── Acciones ──────────────────────────────────────────────────────────
        public void AgregarPago()
        {
            if (MetodoSeleccionado == null) { MostrarError("Seleccioná un método de pago."); return; }

            var texto = MontoIngresado.Replace(",", ".");
            if (!decimal.TryParse(texto,
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out var monto) || monto <= 0)
            {
                MostrarError("Ingresá un monto válido mayor a 0.");
                return;
            }

            LimpiarError();

            // Si el método ya existe, sumar el monto
                var existente = Pagos.FirstOrDefault(p => p.IdMetodoPago == MetodoSeleccionado.IdMetodoPago);
                if (existente != null)
                {
                    var idx = Pagos.IndexOf(existente);
                    Pagos[idx] = new PagoLineaVm
                    {
                        IdMetodoPago = existente.IdMetodoPago,
                        NombreMetodo = existente.NombreMetodo,
                        Categoria    = existente.Categoria,
                        Monto        = existente.Monto + monto,
                    };
                }
                else
                {
                    Pagos.Add(new PagoLineaVm
                    {
                        IdMetodoPago = MetodoSeleccionado.IdMetodoPago,
                        NombreMetodo = MetodoSeleccionado.NombreMetodo,
                        Categoria    = MetodoSeleccionado.Categoria,
                        Monto        = monto,
                    });
            }

            MontoIngresado = string.Empty;
            RecalcularTotalPagado();
            _ = RecalcularDescuentoPreviewAsync();
        }

        public void QuitarPago(PagoLineaVm linea)
        {
            if (linea == null) return;
            Pagos.Remove(linea);
            RecalcularTotalPagado();
            _ = RecalcularDescuentoPreviewAsync();
        }

		/// 		/// Agrega un pago con el método seleccionado.
		/// Si hay un monto escrito usa ese, si no completa el faltante automáticamente.
		private void SeleccionarOCompletar(PagoItemDto metodo)
		{
			MetodoSeleccionado = metodo;

			// Si el campo está vacío o inválido, completar con faltante o total
			var texto = (MontoIngresado ?? "").Replace(",", ".");
			if (!decimal.TryParse(texto,
					System.Globalization.NumberStyles.Any,
					System.Globalization.CultureInfo.InvariantCulture,
					out var monto) || monto <= 0)
			{
				MontoIngresado = (Faltante > 0 ? Faltante : TotalVenta).ToString("F2");
			}

			AgregarPago();
		}

		public void AgregarEfectivo()
		{
			var efectivo = MetodosPago.FirstOrDefault(m => m.Categoria == "Efectivo");
			if (efectivo == null) { MostrarError("No hay método de pago en efectivo configurado."); return; }
			SeleccionarOCompletar(efectivo);
		}

		public void AgregarDebito()
		{
			var debito = MetodosPago.FirstOrDefault(m =>
				m.Categoria == "Tarjeta" && m.Subcategoria == "Debito");
			if (debito == null) { MostrarError("No hay método de pago débito configurado."); return; }
			SeleccionarOCompletar(debito);
		}

		public void AgregarCredito()
		{
			var credito = MetodosPago.FirstOrDefault(m =>
				m.Categoria == "Tarjeta" && m.Subcategoria == "Credito");
			if (credito == null) { MostrarError("No hay método de pago crédito configurado."); return; }
			SeleccionarOCompletar(credito);
		}

		public void AgregarQR()
		{
			var qr = MetodosPago.FirstOrDefault(m =>
				m.NombreMetodo.Contains("QR", StringComparison.OrdinalIgnoreCase) ||
				m.NombreMetodo.Contains("Transferencia", StringComparison.OrdinalIgnoreCase));
			if (qr == null) { MostrarError("No hay método de pago QR configurado."); return; }
			SeleccionarOCompletar(qr);
		}

        public void SeleccionarNodo(PagoNodoJerarquico nodo)
        {
            if (nodo == null) return;

            if (nodo.EsHoja && nodo.MetodoPagoId.HasValue)
            {
                var metodo = MetodosPago.FirstOrDefault(m => m.IdMetodoPago == nodo.MetodoPagoId);
                if (metodo != null)
                {
                    SeleccionarOCompletar(metodo);
                }
            }
            else if (nodo.Hijos.Any())
            {
                NivelActual = nodo;
                NodosVisibles = new ObservableCollection<PagoNodoJerarquico>(nodo.Hijos);
            }
        }

        public void Volver()
        {
            if (NivelActual?.Padre != null && NivelActual != _nodoRaiz)
            {
                NivelActual = NivelActual.Padre;
                NodosVisibles = new ObservableCollection<PagoNodoJerarquico>(NivelActual.Hijos);
            }
        }

        public async Task Confirmar()
        {
            System.Diagnostics.Debug.WriteLine("[PagoVM-Confirmar] Iniciando confirmación de pago...");
            System.Diagnostics.Debug.WriteLine($"[PagoVM-Confirmar] _idVenta={_idVenta}, Vuelto={Vuelto}, TotalPagado={TotalPagado}, TotalVenta={TotalVenta}");
            
            if (!PuedeCobrar) { MostrarError("El monto no cubre el total."); return; }

            IsLoading = true;
            LimpiarError();
            try
            {
                var pagosDto = Pagos.Select(p => new PagoItemDto
                {
                    IdMetodoPago = p.IdMetodoPago,
                    Monto        = p.Monto,
                    Categoria    = p.Categoria,
                }).ToList();

                // Calcular el vuelto total y asignarlo al primer pago en efectivo
                var hayVuelto = Vuelto > 0;
                if (hayVuelto)
                {
                    var primerEfectivo = pagosDto.FirstOrDefault(p => p.Categoria == "Efectivo");
                    if (primerEfectivo != null)
                    {
                        primerEfectivo.Vuelto = Vuelto;
                    }
                }

                System.Diagnostics.Debug.WriteLine($"[PagoVM-Confirmar] Pagos: {pagosDto.Count}, Total: {pagosDto.Sum(p => p.Monto)}");

                // Verificar null antes de llamar al servicio
                if (_idVenta <= 0)
                {
                    System.Diagnostics.Debug.WriteLine("[PagoVM-Confirmar] ERROR: ID de venta inválido");
                    MostrarError("ID de venta inválido.");
                    return;
                }

                if (pagosDto == null || !pagosDto.Any())
                {
                    System.Diagnostics.Debug.WriteLine("[PagoVM-Confirmar] ERROR: No hay pagos registrados");
                    MostrarError("No hay pagos registrados.");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"[PagoVM-Confirmar] Llamando a RegistrarPagoAsync para venta #{_idVenta}...");
                await _ventaServicio.RegistrarPagoAsync(_idVenta, pagosDto);
                System.Diagnostics.Debug.WriteLine("[PagoVM-Confirmar] Pago registrado exitosamente");

                // Ir al comprobante
                System.Diagnostics.Debug.WriteLine("[PagoVM-Confirmar] Navegando a ComprobanteViewModel...");
                var vm = IoC.Get<ComprobanteViewModel>();
                System.Diagnostics.Debug.WriteLine($"[PagoVM-Confirmar] ComprobanteVM obtenido: {vm != null}");
                System.Diagnostics.Debug.WriteLine($"[PagoVM-Confirmar] Llamando CargarAsync con idVenta={_idVenta}, Vuelto={Vuelto}");
                await vm.CargarAsync(_idVenta, Vuelto);
                System.Diagnostics.Debug.WriteLine("[PagoVM-Confirmar] CargarAsync completado");
                await IoC.Get<ShellViewModel>().ActivateItemAsync(vm, CancellationToken.None);
                System.Diagnostics.Debug.WriteLine("[PagoVM-Confirmar] Proceso completado");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[PagoVM-Confirmar] ERROR: {ex}");
                var mensaje = $"Error al confirmar el pago: {ex.Message}";
                MostrarError(mensaje);
                System.Windows.MessageBox.Show(mensaje, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            finally { IsLoading = false; }
        }

        public async Task Cancelar()
        {
            // La venta queda en Pendiente — el operador puede anularla desde el historial
            await IoC.Get<ShellViewModel>()
                     .ActivateItemAsync(IoC.Get<VentaViewModel>(), CancellationToken.None);
        }

        public async Task GuardarPendienteAsync()
        {
            var confirmacion = System.Windows.MessageBox.Show(
                "La venta quedará pendiente de cobro. ¿Desea continuar?",
                "Confirmar",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Question);

            if (confirmacion != System.Windows.MessageBoxResult.Yes) return;

            // No registrar pago, no marcar pagada, no revertir stock.
            // La venta ya existe en BD con Estado Pendiente y stock descontado.
            await IoC.Get<ShellViewModel>()
                     .ActivateItemAsync(IoC.Get<VentaListadoViewModel>(), CancellationToken.None);

            System.Windows.MessageBox.Show(
                "Venta guardada como pendiente.",
                "Información",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }

        private void RecalcularTotalPagado()
        {
            TotalPagado = Pagos.Sum(p => p.Monto);
            NotifyOfPropertyChange(() => PuedeCobrar);
        }

        private void RecalcularVuelto()
            => Vuelto = TotalPagado > TotalVenta ? TotalPagado - TotalVenta : 0;

        private async Task RecalcularDescuentoPreviewAsync()
        {
            if (_descuentosCache == null || _ventaCompleta == null || _categoriasCache == null)
                return;

            var idsMetodosPago = Pagos.Select(p => p.IdMetodoPago).Distinct().ToList();
            var esPagoUnico = idsMetodosPago.Count == 1;
            decimal totalDescuentoMetodoPago = 0;
            decimal porcentajeDescuentoMetodo = 0;

            if (esPagoUnico && idsMetodosPago.Count == 1)
            {
                // Per-item discounts
                foreach (var detalle in _ventaCompleta.Detalles)
                {
                    var subtotalDetalle = detalle.Cantidad * detalle.PrecioUnitario - detalle.Descuento;
                    if (subtotalDetalle <= 0) continue;

                    var descuento = await _descuentoConfiguracionServicio.ObtenerDescuentoAplicableAsync(
                        _sesion.IdEmpresa,
                        detalle.Id_producto,
                        detalle.Producto?.Id_categoria,
                        idsMetodosPago,
                        esPagoUnico: true,
                        _descuentosCache,
                        _categoriasCache);
                    if (descuento != null)
                    {
                        totalDescuentoMetodoPago += Math.Round(subtotalDetalle * descuento.Valor / 100, 2, MidpointRounding.AwayFromZero);
                        porcentajeDescuentoMetodo = descuento.Valor;
                    }
                }

                // REGLA B: total-venta solo si no hubo per-item
                if (totalDescuentoMetodoPago == 0)
                {
                    var descuentoTotalVenta = await _descuentoConfiguracionServicio.ObtenerDescuentoTotalVentaAsync(
                        _sesion.IdEmpresa, idsMetodosPago[0], _descuentosCache);
                    if (descuentoTotalVenta != null)
                    {
                        var baseCalculo = _ventaCompleta.TotalBruto - _ventaCompleta.TotalDescuento;
                        totalDescuentoMetodoPago = Math.Round(baseCalculo * descuentoTotalVenta.Valor / 100, 2, MidpointRounding.AwayFromZero);
                        porcentajeDescuentoMetodo = descuentoTotalVenta.Valor;
                    }
                }
            }

            TotalVenta = _ventaCompleta.TotalBruto - _ventaCompleta.TotalDescuento - totalDescuentoMetodoPago;

            // Sincronizar línea de descuento por método de pago en el preview
            var lineaMetodo = LineasDescuento.FirstOrDefault(l => l.EsMetodoPago);
            if (totalDescuentoMetodoPago > 0)
            {
                var descuentoTexto = porcentajeDescuentoMetodo > 0
                    ? $"Método de pago -{porcentajeDescuentoMetodo:0.##}%"
                    : "Método de pago";
                if (lineaMetodo == null)
                {
                    LineasDescuento.Add(new DescuentoLineaVm
                    {
                        ProductoNombre = "Método de pago",
                        Monto = totalDescuentoMetodoPago,
                        Descripcion = descuentoTexto,
                        EsMetodoPago = true,
                        Porcentaje = porcentajeDescuentoMetodo
                    });
                }
                else
                {
                    var idx = LineasDescuento.IndexOf(lineaMetodo);
                    LineasDescuento[idx] = new DescuentoLineaVm
                    {
                        ProductoNombre = "Método de pago",
                        Monto = totalDescuentoMetodoPago,
                        Descripcion = descuentoTexto,
                        EsMetodoPago = true,
                        Porcentaje = porcentajeDescuentoMetodo
                    };
                }
            }
            else if (lineaMetodo != null)
            {
                LineasDescuento.Remove(lineaMetodo);
            }
            NotifyOfPropertyChange(() => TieneDescuentos);
        }

        // ── Popup Historial (same as VentaViewModel) ─────────────────────────────
        private bool _mostrarHistorial;
        public bool MostrarHistorial
        {
            get => _mostrarHistorial;
            set { _mostrarHistorial = value; NotifyOfPropertyChange(() => MostrarHistorial); }
        }

        private ObservableCollection<VentaResumenDto> _historialVentas = new();
        public ObservableCollection<VentaResumenDto> HistorialVentas
        {
            get => _historialVentas;
            set { _historialVentas = value; NotifyOfPropertyChange(() => HistorialVentas); }
        }

        private DateTime? _fechaDesde;
        public DateTime? FechaDesde
        {
            get => _fechaDesde;
            set { SetProperty(ref _fechaDesde, value); }
        }

        private DateTime? _fechaHasta;
        public DateTime? FechaHasta
        {
            get => _fechaHasta;
            set { SetProperty(ref _fechaHasta, value); }
        }

        private string _dniClienteFiltro = string.Empty;
        public string DniClienteFiltro
        {
            get => _dniClienteFiltro;
            set { SetProperty(ref _dniClienteFiltro, value); }
        }

        private int? _estadoVentaFiltro;
        public int? EstadoVentaFiltro
        {
            get => _estadoVentaFiltro;
            set { SetProperty(ref _estadoVentaFiltro, value); }
        }

        ///         /// Carga el historial de ventas (reutiliza la lógica de VentaViewModel).
        public async Task CargarHistorialAsync()
        {
            try
            {
                var desde = FechaDesde ?? DateTime.Today.AddDays(-30);
                var hasta = FechaHasta ?? DateTime.Today.AddDays(1).AddSeconds(-1);

                IEnumerable<VentaResumenDto> ventas;

                if (FechaDesde.HasValue || FechaHasta.HasValue || !string.IsNullOrWhiteSpace(DniClienteFiltro) || EstadoVentaFiltro.HasValue)
                {
                    ventas = await _ventaServicio.ObtenerVentasAsync(
                        _sesion.IdSucursal, FechaDesde, FechaHasta, DniClienteFiltro, EstadoVentaFiltro);
                }
                else
                {
                    ventas = await _ventaServicio.ObtenerPorSucursalAsync(_sesion.IdSucursal, desde, hasta);
                }

                var listaVentas = ventas.Take(50).ToList();
                HistorialVentas = new ObservableCollection<VentaResumenDto>(listaVentas);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[PagoVM] CargarHistorialAsync ERROR: {ex.Message}");
            }
        }

        ///         /// Filtra el historial aplicando los filtros activos.
        public void FiltrarHistorial()
        {
            _ = CargarHistorialAsync();
        }

        ///         /// Cierra el popup de historial.
        public void CerrarHistorial()
        {
            MostrarHistorial = false;
        }

        private bool _ventaCompletada;
        public bool VentaCompletada
        {
            get => _ventaCompletada;
            set { _ventaCompletada = value; NotifyOfPropertyChange(() => VentaCompletada); }
        }

        // ── Descuentos aplicados ─────────────────────────────────────────
        private ObservableCollection<DescuentoLineaVm> _lineasDescuento = new();
        public ObservableCollection<DescuentoLineaVm> LineasDescuento
        {
            get => _lineasDescuento;
            set { _lineasDescuento = value; NotifyOfPropertyChange(() => LineasDescuento); }
        }

        public bool TieneDescuentos => LineasDescuento.Any();

        ///         /// Inicializa el PagoViewModel con los datos de la venta.
        /// Carga los descuentos aplicados desde la base de datos.
        public async Task InicializarConVenta(int idVenta, string clienteNombre, decimal totalFinal)
        {
            _idVenta       = idVenta;
            ClienteNombre  = clienteNombre;
            TotalVenta     = totalFinal;
            Pagos          = new();
            MontoIngresado = totalFinal.ToString("F2");
            RecalcularVuelto();

            // Cargar venta completa y caches para preview de descuentos
            _ventaCompleta = await _uow.Ventas.ObtenerConDetallesAsync(idVenta);
            if (_ventaCompleta != null && _sesion.IdEmpresa > 0)
            {
                _descuentosCache = await _descuentoConfiguracionServicio.ObtenerTodosAsync(_sesion.IdEmpresa);
                var categorias = await _uow.Categorias.ObtenerPorEmpresaAsync(_sesion.IdEmpresa);
                _categoriasCache = new Dictionary<int, Categoria>();
                if (categorias != null)
                {
                    foreach (var cat in categorias)
                        _categoriasCache[cat.Id] = cat;
                }
            }

            // Cargar descuentos aplicados desde la venta
            await CargarDescuentosAsync(idVenta);
        }

        private async Task CargarDescuentosAsync(int idVenta)
        {
            try
            {
                var venta = await _uow.Ventas.ObtenerConDetallesAsync(idVenta);
                if (venta == null) return;

                var lineas = new List<DescuentoLineaVm>();

                // Líneas de descuento por configuración (por detalle)
                foreach (var detalle in venta.Detalles.Where(d => d.Descuento > 0 || d.Descuentos.Any()))
                {
                    var descuentoConfig = detalle.Descuentos?.FirstOrDefault();
                    lineas.Add(new DescuentoLineaVm
                    {
                        ProductoNombre = detalle.Producto?.Nombre ?? $"Item #{detalle.Id_producto}",
                        Monto = detalle.DescuentoTotal,
                        Descripcion = descuentoConfig != null ? $"-{descuentoConfig.Porcentaje:0.##}%" : "Configurado",
                        EsMetodoPago = false,
                        Porcentaje = descuentoConfig?.Porcentaje ?? 0
                    });
                }

                // Línea de descuento por método de pago
                if (venta.DescuentoMetodoPago > 0)
                {
                    lineas.Add(new DescuentoLineaVm
                    {
                        ProductoNombre = "Método de pago",
                        Monto = venta.DescuentoMetodoPago,
                        Descripcion = "Método de pago",
                        EsMetodoPago = true,
                        Porcentaje = 0
                    });
                }

                LineasDescuento = new ObservableCollection<DescuentoLineaVm>(lineas);
                NotifyOfPropertyChange(() => TieneDescuentos);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[PagoVM] Error cargando descuentos: {ex.Message}");
            }
        }
    }

    public class PagoLineaVm
    {
        public int     IdMetodoPago { get; set; }
        public string  NombreMetodo { get; set; } = string.Empty;
        public string  Categoria    { get; set; } = "Otro";
        public decimal Monto        { get; set; }
    }

    public class DescuentoLineaVm
    {
        public string  ProductoNombre { get; set; } = string.Empty;
        public decimal Monto          { get; set; }
        public string  Descripcion    { get; set; } = string.Empty;
        public bool    EsMetodoPago   { get; set; }
        public decimal Porcentaje     { get; set; }
    }
}
