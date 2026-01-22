using System;
using System.Collections.Generic;

namespace CondosmartWeb.Models;

public partial class UnidadesResidenciai
{
    public int Id { get; set; }

    public string Identificador { get; set; } = null!;

    public string? Rua { get; set; }

    public string? Bairro { get; set; }

    public string? Numero { get; set; }

    public string? Complemento { get; set; }

    public string? Cep { get; set; }

    public string? Cidade { get; set; }

    public string? Uf { get; set; }

    public string? TelefoneResidencial { get; set; }

    public string? TelefoneCelular { get; set; }

    public byte[]? FotoRosto { get; set; }

    public int? MoradorId { get; set; }

    public int CondominioId { get; set; }

    public int? SindicoId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Condominio Condominio { get; set; } = null!;

    public virtual Moradore? Morador { get; set; }

    public virtual ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();

    public virtual Sindico? Sindico { get; set; }

    public virtual ICollection<Visitante> Visitantes { get; set; } = new List<Visitante>();
}
