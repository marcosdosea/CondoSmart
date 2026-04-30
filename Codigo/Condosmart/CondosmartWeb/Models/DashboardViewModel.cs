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
        public IReadOnlyList<DashboardMensalidadeItemViewModel> MensalidadesAbertas { get; set; } = [];
    }

    public class DashboardMensalidadeItemViewModel
    {
        public int Id { get; set; }
        public string Unidade { get; set; } = "-";
        public string Morador { get; set; } = "-";
        public DateTime Vencimento { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; } = "-";
    }
}
