using System;
using System.Collections.Generic;

namespace Core.Models;

public partial class Sindico
{
    public int Id { get; set; }

    public string Cpf { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public string? Telefone { get; set; }

    public string? Email { get; set; }

    public string? Rua { get; set; }

    public string? Bairro { get; set; }

    public string? Numero { get; set; }

    public string? Complemento { get; set; }

    public string? Cep { get; set; }

    public string? Cidade { get; set; }

    public string? Uf { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<AreaDeLazer> AreaDeLazers { get; set; } = new List<AreaDeLazer>();

    public virtual ICollection<Ata> Ata { get; set; } = new List<Ata>();

    public virtual ICollection<Chamado> Chamados { get; set; } = new List<Chamado>();

    public virtual ICollection<Condominio> Condominios { get; set; } = new List<Condominio>();

    public virtual ICollection<Porteiro> Porteiros { get; set; } = new List<Porteiro>();

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public virtual ICollection<UnidadesResidenciais> UnidadesResidenciais { get; set; } = new List<UnidadesResidenciais>();

    public virtual ICollection<Visitantes> Visitantes { get; set; } = new List<Visitantes>();
}
