namespace CondosmartWeb.Models
{
    public class DashboardViewModel
    {
        // KPIs superiores
        public decimal TotalRecebidoMes { get; set; }
        public double TaxaInadimplencia { get; set; }
        public int EntradasHoje { get; set; }
        public int MensalidadesEmAtraso { get; set; }

        // Cards centrais
        public int UnidadesOcupadas { get; set; }
        public int TotalUnidades { get; set; }
        public int ReservasConfirmadas { get; set; }
        public int TotalAreasLazer { get; set; }
    }
}
