using System;
using System.Collections.Generic;

namespace CondosmartWeb.Models;

public partial class Condominio
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string? Cnpj { get; set; }

    public string? Email { get; set; }

    public string? Telefone { get; set; }

    public string? Rua { get; set; }

    public string? Bairro { get; set; }

    public string? Numero { get; set; }

    public string? Complemento { get; set; }

    public string? Cep { get; set; }

    public string? Cidade { get; set; }

    public string? Uf { get; set; }

    public int? Unidades { get; set; }

    public int? SindicoId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<AreaDeLazer> AreaDeLazers { get; set; } = new List<AreaDeLazer>();

    public virtual ICollection<Ata> Ata { get; set; } = new List<Ata>();

    public virtual ICollection<Chamado> Chamados { get; set; } = new List<Chamado>();

    public virtual ICollection<Moradore> Moradores { get; set; } = new List<Moradore>();

    public virtual ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public virtual Sindico? Sindico { get; set; }

    public virtual ICollection<UnidadesResidenciai> UnidadesResidenciais { get; set; } = new List<UnidadesResidenciai>();
}
