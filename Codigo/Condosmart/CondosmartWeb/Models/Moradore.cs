using System;
using System.Collections.Generic;

namespace CondosmartWeb.Models;

public partial class Moradore
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Cpf { get; set; } = null!;

    public string? Rg { get; set; }

    public string? Telefone { get; set; }

    public string? Email { get; set; }

    public byte[]? Biometria { get; set; }

    public string? Rua { get; set; }

    public string? Bairro { get; set; }

    public string? Numero { get; set; }

    public string? Complemento { get; set; }

    public string? Cep { get; set; }

    public string? Cidade { get; set; }

    public string? Uf { get; set; }

    public int? CondominioId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Chamado> Chamados { get; set; } = new List<Chamado>();

    public virtual Condominio? Condominio { get; set; }

    public virtual ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public virtual ICollection<UnidadesResidenciai> UnidadesResidenciais { get; set; } = new List<UnidadesResidenciai>();

    public virtual ICollection<Visitante> Visitantes { get; set; } = new List<Visitante>();
}
