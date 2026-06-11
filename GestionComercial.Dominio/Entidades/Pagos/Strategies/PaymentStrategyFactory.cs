using GestionComercial.Dominio.Interfaces;

namespace GestionComercial.Dominio.Entidades.Pagos.Strategies
{
    public class PaymentStrategyFactory
    {
        private readonly Dictionary<string, IProcesadorPago> _strategies;

        public PaymentStrategyFactory()
        {
            _strategies = new Dictionary<string, IProcesadorPago>(StringComparer.OrdinalIgnoreCase)
            {
                ["Efectivo"]     = new EfectivoStrategy(),
                ["Tarjeta"]      = new TarjetaStrategy(),
                ["Transferencia"]= new TransferenciaStrategy(),
                ["Otro"]         = new OtroStrategy(),
            };
        }

        public IProcesadorPago Resolve(string categoria)
        {
            if (_strategies.TryGetValue(categoria, out var strategy))
                return strategy;

            return _strategies["Otro"];
        }
    }
}
