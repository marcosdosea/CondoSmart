using System;
using System.Collections.Generic;

namespace CondosmartWeb.Models;

public partial class Porteiro
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string? Cpf { get; set; }

    public string? Telefone { get; set; }

    public string? Email { get; set; }

    public string? Rua { get; set; }

    public string? Bairro { get; set; }

    public string? Numero { get; set; }

    public string? Complemento { get; set; }

    public string? Cep { get; set; }

    public string? Cidade { get; set; }

    public string? Uf { get; set; }

    public int? SindicoId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Sindico? Sindico { get; set; }

    public virtual ICollection<Visitante> Visitantes { get; set; } = new List<Visitante>();
}
