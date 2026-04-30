using System.ComponentModel.DataAnnotations;

namespace CondosmartWeb.Models
{
	public class ChamadoViewModel
	{
		[Display(Name = "Código")]
		public int Id { get; set; }

		[Required(ErrorMessage = "A descrição é obrigatória")]
		[StringLength(200, ErrorMessage = "A descrição não pode exceder 200 caracteres.")]
		[Display(Name = "Descrição")]
		public string Descricao { get; set; } = string.Empty;

		[Display(Name = "Data do Chamado")]
		[DataType(DataType.DateTime)]
		[Required(ErrorMessage = "A data do chamado é obrigatória")]
		public DateTime DataChamado { get; set; } = DateTime.Now;

		[Range(1, int.MaxValue, ErrorMessage = "Informe um morador válido")]
		[Display(Name = "Morador")]
		public int? MoradorId { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "Informe um síndico válido")]
		[Display(Name = "Síndico")]
		public int? SindicoId { get; set; }

		[Required(ErrorMessage = "O condomínio é obrigatório")]
		[Range(1, int.MaxValue, ErrorMessage = "Informe um condomínio válido")]
		[Display(Name = "Condomínio")]
		public int CondominioId { get; set; }

		[Required(ErrorMessage = "O status é obrigatório")]
		[StringLength(30, ErrorMessage = "O status não pode exceder 30 caracteres.")]
		[RegularExpression("^(aberto|em_andamento|resolvido|cancelado)$", ErrorMessage = "Selecione um status válido.")]
		[Display(Name = "Status")]
		public string Status { get; set; } = "aberto";

		// Opcional, normalmente não editável
		[Display(Name = "Criado em")]
		public DateTime? CreatedAt { get; set; }
	}
}
