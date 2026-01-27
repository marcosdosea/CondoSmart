namespace Core.DTO
{
    public class AtaDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public DateTime DataReuniao { get; set; }

        public int CondominioId { get; set; }
        public int? SindicoId { get; set; }
        public string? Temas { get; set; }
    }
}
