namespace Core.DTO
{
    public class GeracaoParcelasResultDTO
    {
        public int CondominioId { get; set; }

        public int AnoReferencia { get; set; }

        public int QuantidadeParcelasSolicitada { get; set; }

        public int ParcelasGeradas { get; set; }

        public int ParcelasIgnoradas { get; set; }
    }
}
