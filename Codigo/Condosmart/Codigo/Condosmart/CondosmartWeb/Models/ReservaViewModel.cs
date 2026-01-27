using System;
using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models
{
    public class ReservaViewModel
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Área é obrigatório")]
        [Display(Name = "Área")]
        public int AreaId { get; set; }

        [Display(Name = "Morador")]
        public int? MoradorId { get; set; }

        [Display(Name = "Visitante")]
        public int? VisitanteId { get; set; }

        [Required(ErrorMessage = "O campo Condomínio é obrigatório")]
        [Display(Name = "Condomínio")]
        public int CondominioId { get; set; }

        [Display(Name = "Síndico")]
        public int? SindicoId { get; set; }

        [Required(ErrorMessage = "A data/hora de início é obrigatória")]
        [Display(Name = "Data de Início")]
        public DateTime DataInicio { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "A data/hora de fim é obrigatória")]
        [Display(Name = "Data de Fim")]
        public DateTime DataFim { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "O status é obrigatório")]
        [StringLength(20)]
        [Display(Name = "Status")]
        public string Status { get; set; } = "pendente";

        // Opcional: mostrar quando foi criado
        [Display(Name = "Criado em")]
        public DateTime? CreatedAt { get; set; }
    }
}