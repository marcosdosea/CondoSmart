using System;
using System.Collections.Generic;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Core.Data;

public partial class CondosmartContext : DbContext
{
    public CondosmartContext()
    {
    }

    public CondosmartContext(DbContextOptions<CondosmartContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AreaDeLazer> AreaDeLazer { get; set; }

    public virtual DbSet<Ata> Atas { get; set; }

    public virtual DbSet<Chamado> Chamados { get; set; }

    public virtual DbSet<Condominio> Condominios { get; set; }

    public virtual DbSet<Morador> Moradores { get; set; }

    public virtual DbSet<Pagamento> Pagamentos { get; set; }

    public virtual DbSet<Porteiro> Porteiros { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Sindico> Sindicos { get; set; }

    public virtual DbSet<UnidadesResidenciais> UnidadesResidenciais { get; set; }

    public virtual DbSet<Visitantes> Visitantes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=123456;database=condosmart", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.44-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<AreaDeLazer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("area_de_lazer");

            entity.HasIndex(e => e.CondominioId, "fk_area_condominio");

            entity.HasIndex(e => e.SindicoId, "fk_area_sindico");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CondominioId).HasColumnName("condominio_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Descricao)
                .HasMaxLength(200)
                .HasColumnName("descricao");
            entity.Property(e => e.Disponibilidade)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("disponibilidade");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome");
            entity.Property(e => e.SindicoId).HasColumnName("sindico_id");

            entity.HasOne(d => d.Condominio).WithMany(p => p.AreaDeLazers)
                .HasForeignKey(d => d.CondominioId)
                .HasConstraintName("fk_area_condominio");

            entity.HasOne(d => d.Sindico).WithMany(p => p.AreaDeLazers)
                .HasForeignKey(d => d.SindicoId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_area_sindico");
        });

        modelBuilder.Entity<Ata>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("atas");

            entity.HasIndex(e => e.CondominioId, "fk_atas_condominio");

            entity.HasIndex(e => e.SindicoId, "fk_atas_sindico");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CondominioId).HasColumnName("condominio_id");
            entity.Property(e => e.Conteudo)
                .HasColumnType("text")
                .HasColumnName("conteudo");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DataReuniao).HasColumnName("data_reuniao");
            entity.Property(e => e.SindicoId).HasColumnName("sindico_id");
            entity.Property(e => e.Temas)
                .HasMaxLength(150)
                .HasColumnName("temas");
            entity.Property(e => e.Titulo)
                .HasMaxLength(100)
                .HasColumnName("titulo");

            entity.HasOne(d => d.Condominio).WithMany(p => p.Ata)
                .HasForeignKey(d => d.CondominioId)
                .HasConstraintName("fk_atas_condominio");

            entity.HasOne(d => d.Sindico).WithMany(p => p.Ata)
                .HasForeignKey(d => d.SindicoId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_atas_sindico");
        });

        modelBuilder.Entity<Chamado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("chamados");

            entity.HasIndex(e => e.CondominioId, "fk_chamado_condominio");

            entity.HasIndex(e => e.MoradorId, "fk_chamado_morador");

            entity.HasIndex(e => e.SindicoId, "fk_chamado_sindico");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CondominioId).HasColumnName("condominio_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DataChamado)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("data_chamado");
            entity.Property(e => e.Descricao)
                .HasMaxLength(200)
                .HasColumnName("descricao");
            entity.Property(e => e.MoradorId).HasColumnName("morador_id");
            entity.Property(e => e.SindicoId).HasColumnName("sindico_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'aberto'")
                .HasColumnType("enum('aberto','em_andamento','resolvido','cancelado')")
                .HasColumnName("status");

            entity.HasOne(d => d.Condominio).WithMany(p => p.Chamados)
                .HasForeignKey(d => d.CondominioId)
                .HasConstraintName("fk_chamado_condominio");

            entity.HasOne(d => d.Morador).WithMany(p => p.Chamados)
                .HasForeignKey(d => d.MoradorId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_chamado_morador");

            entity.HasOne(d => d.Sindico).WithMany(p => p.Chamados)
                .HasForeignKey(d => d.SindicoId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_chamado_sindico");
        });

        modelBuilder.Entity<Condominio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("condominios");

            entity.HasIndex(e => e.Cnpj, "cnpj").IsUnique();

            entity.HasIndex(e => e.SindicoId, "fk_condominio_sindico");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bairro)
                .HasMaxLength(60)
                .HasColumnName("bairro");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .IsFixedLength()
                .HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .HasMaxLength(60)
                .HasColumnName("cidade");
            entity.Property(e => e.Cnpj)
                .HasMaxLength(14)
                .IsFixedLength()
                .HasColumnName("cnpj");
            entity.Property(e => e.Complemento)
                .HasMaxLength(40)
                .HasColumnName("complemento");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(80)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(80)
                .HasColumnName("nome");
            entity.Property(e => e.Numero)
                .HasMaxLength(10)
                .HasColumnName("numero");
            entity.Property(e => e.Rua)
                .HasMaxLength(80)
                .HasColumnName("rua");
            entity.Property(e => e.SindicoId).HasColumnName("sindico_id");
            entity.Property(e => e.Telefone)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("telefone");
            entity.Property(e => e.Uf)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("uf");
            entity.Property(e => e.Unidades).HasColumnName("unidades");

            entity.HasOne(d => d.Sindico).WithMany(p => p.Condominios)
                .HasForeignKey(d => d.SindicoId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_condominio_sindico");
        });

        modelBuilder.Entity<Morador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("moradores");

            entity.HasIndex(e => e.Cpf, "cpf").IsUnique();

            entity.HasIndex(e => e.CondominioId, "fk_morador_condominio");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bairro)
                .HasMaxLength(60)
                .HasColumnName("bairro");
            entity.Property(e => e.Biometria)
                .HasColumnType("blob")
                .HasColumnName("biometria");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .IsFixedLength()
                .HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .HasMaxLength(60)
                .HasColumnName("cidade");
            entity.Property(e => e.Complemento)
                .HasMaxLength(40)
                .HasColumnName("complemento");
            entity.Property(e => e.CondominioId).HasColumnName("condominio_id");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("cpf");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(80)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(80)
                .HasColumnName("nome");
            entity.Property(e => e.Numero)
                .HasMaxLength(10)
                .HasColumnName("numero");
            entity.Property(e => e.Rg)
                .HasMaxLength(9)
                .IsFixedLength()
                .HasColumnName("rg");
            entity.Property(e => e.Rua)
                .HasMaxLength(80)
                .HasColumnName("rua");
            entity.Property(e => e.Telefone)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("telefone");
            entity.Property(e => e.Uf)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("uf");

            entity.HasOne(d => d.Condominio).WithMany(p => p.Moradores)
                .HasForeignKey(d => d.CondominioId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_morador_condominio");
        });

        modelBuilder.Entity<Pagamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pagamentos");

            entity.HasIndex(e => e.CondominioId, "ix_pagamentos_condominio");

            entity.HasIndex(e => e.MoradorId, "ix_pagamentos_morador");

            entity.HasIndex(e => e.UnidadeId, "ix_pagamentos_unidade");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CondominioId).HasColumnName("condominio_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DataPagamento)
                .HasColumnType("datetime")
                .HasColumnName("data_pagamento");
            entity.Property(e => e.FormaPagamento)
                .HasColumnType("enum('pix','cartao_credito','cartao_debito','boleto','dinheiro')")
                .HasColumnName("forma_pagamento");
            entity.Property(e => e.MoradorId).HasColumnName("morador_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'pendente'")
                .HasColumnType("enum('pendente','pago','cancelado','estornado')")
                .HasColumnName("status");
            entity.Property(e => e.UnidadeId).HasColumnName("unidade_id");
            entity.Property(e => e.Valor)
                .HasPrecision(10, 2)
                .HasColumnName("valor");

            entity.HasOne(d => d.Condominio).WithMany(p => p.Pagamentos)
                .HasForeignKey(d => d.CondominioId)
                .HasConstraintName("fk_pagamento_condominio");

            entity.HasOne(d => d.Morador).WithMany(p => p.Pagamentos)
                .HasForeignKey(d => d.MoradorId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_pagamento_morador");

            entity.HasOne(d => d.Unidade).WithMany(p => p.Pagamentos)
                .HasForeignKey(d => d.UnidadeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_pagamento_unidade");
        });

        modelBuilder.Entity<Porteiro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("porteiros");

            entity.HasIndex(e => e.Cpf, "cpf").IsUnique();

            entity.HasIndex(e => e.SindicoId, "fk_porteiro_sindico");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bairro)
                .HasMaxLength(60)
                .HasColumnName("bairro");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .IsFixedLength()
                .HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .HasMaxLength(60)
                .HasColumnName("cidade");
            entity.Property(e => e.Complemento)
                .HasMaxLength(40)
                .HasColumnName("complemento");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("cpf");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(80)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(80)
                .HasColumnName("nome");
            entity.Property(e => e.Numero)
                .HasMaxLength(10)
                .HasColumnName("numero");
            entity.Property(e => e.Rua)
                .HasMaxLength(80)
                .HasColumnName("rua");
            entity.Property(e => e.SindicoId).HasColumnName("sindico_id");
            entity.Property(e => e.Telefone)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("telefone");
            entity.Property(e => e.Uf)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("uf");

            entity.HasOne(d => d.Sindico).WithMany(p => p.Porteiros)
                .HasForeignKey(d => d.SindicoId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_porteiro_sindico");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("reservas");

            entity.HasIndex(e => e.AreaId, "fk_reserva_area");

            entity.HasIndex(e => e.CondominioId, "fk_reserva_condominio");

            entity.HasIndex(e => e.MoradorId, "fk_reserva_morador");

            entity.HasIndex(e => e.SindicoId, "fk_reserva_sindico");

            entity.HasIndex(e => e.VisitanteId, "fk_reserva_visitante");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AreaId).HasColumnName("area_id");
            entity.Property(e => e.CondominioId).HasColumnName("condominio_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DataFim)
                .HasColumnType("datetime")
                .HasColumnName("data_fim");
            entity.Property(e => e.DataInicio)
                .HasColumnType("datetime")
                .HasColumnName("data_inicio");
            entity.Property(e => e.MoradorId).HasColumnName("morador_id");
            entity.Property(e => e.SindicoId).HasColumnName("sindico_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'confirmado'")
                .HasColumnType("enum('confirmado','pendente','cancelado','concluido')")
                .HasColumnName("status");
            entity.Property(e => e.VisitanteId).HasColumnName("visitante_id");

            entity.HasOne(d => d.Area).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("fk_reserva_area");

            entity.HasOne(d => d.Condominio).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.CondominioId)
                .HasConstraintName("fk_reserva_condominio");

            entity.HasOne(d => d.Morador).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.MoradorId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_reserva_morador");

            entity.HasOne(d => d.Sindico).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.SindicoId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_reserva_sindico");

            entity.HasOne(d => d.Visitante).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.VisitanteId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_reserva_visitante");
        });

        modelBuilder.Entity<Sindico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sindicos");

            entity.HasIndex(e => e.Cpf, "cpf").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bairro)
                .HasMaxLength(60)
                .HasColumnName("bairro");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .IsFixedLength()
                .HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .HasMaxLength(60)
                .HasColumnName("cidade");
            entity.Property(e => e.Complemento)
                .HasMaxLength(40)
                .HasColumnName("complemento");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("cpf");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(80)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(80)
                .HasColumnName("nome");
            entity.Property(e => e.Numero)
                .HasMaxLength(10)
                .HasColumnName("numero");
            entity.Property(e => e.Rua)
                .HasMaxLength(80)
                .HasColumnName("rua");
            entity.Property(e => e.Telefone)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("telefone");
            entity.Property(e => e.Uf)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("uf");
        });

        modelBuilder.Entity<UnidadesResidenciais>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("unidades_residenciais");

            entity.HasIndex(e => e.MoradorId, "fk_unidade_morador");

            entity.HasIndex(e => e.SindicoId, "fk_unidade_sindico");

            entity.HasIndex(e => new { e.CondominioId, e.Identificador }, "ux_unidade_ident_condominio").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bairro)
                .HasMaxLength(60)
                .HasColumnName("bairro");
            entity.Property(e => e.Cep)
                .HasMaxLength(8)
                .IsFixedLength()
                .HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .HasMaxLength(60)
                .HasColumnName("cidade");
            entity.Property(e => e.Complemento)
                .HasMaxLength(40)
                .HasColumnName("complemento");
            entity.Property(e => e.CondominioId).HasColumnName("condominio_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.FotoRosto)
                .HasColumnType("blob")
                .HasColumnName("foto_rosto");
            entity.Property(e => e.Identificador)
                .HasMaxLength(30)
                .HasColumnName("identificador");
            entity.Property(e => e.MoradorId).HasColumnName("morador_id");
            entity.Property(e => e.Numero)
                .HasMaxLength(10)
                .HasColumnName("numero");
            entity.Property(e => e.Rua)
                .HasMaxLength(80)
                .HasColumnName("rua");
            entity.Property(e => e.SindicoId).HasColumnName("sindico_id");
            entity.Property(e => e.TelefoneCelular)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("telefone_celular");
            entity.Property(e => e.TelefoneResidencial)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("telefone_residencial");
            entity.Property(e => e.Uf)
                .HasMaxLength(2)
                .IsFixedLength()
                .HasColumnName("uf");

            entity.HasOne(d => d.Condominio).WithMany(p => p.UnidadesResidenciais)
                .HasForeignKey(d => d.CondominioId)
                .HasConstraintName("fk_unidade_condominio");

            entity.HasOne(d => d.Morador).WithMany(p => p.UnidadesResidenciais)
                .HasForeignKey(d => d.MoradorId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_unidade_morador");

            entity.HasOne(d => d.Sindico).WithMany(p => p.UnidadesResidenciais)
                .HasForeignKey(d => d.SindicoId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_unidade_sindico");
        });

        modelBuilder.Entity<Visitantes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("visitantes");

            entity.HasIndex(e => e.MoradorId, "fk_visitante_morador");

            entity.HasIndex(e => e.PorteiroId, "fk_visitante_porteiro");

            entity.HasIndex(e => e.SindicoId, "fk_visitante_sindico");

            entity.HasIndex(e => e.UnidadeId, "fk_visitante_unidade");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("cpf");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DataHoraEntrada)
                .HasColumnType("datetime")
                .HasColumnName("data_hora_entrada");
            entity.Property(e => e.DataHoraSaida)
                .HasColumnType("datetime")
                .HasColumnName("data_hora_saida");
            entity.Property(e => e.MoradorId).HasColumnName("morador_id");
            entity.Property(e => e.Nome)
                .HasMaxLength(80)
                .HasColumnName("nome");
            entity.Property(e => e.Observacao)
                .HasMaxLength(200)
                .HasColumnName("observacao");
            entity.Property(e => e.PorteiroId).HasColumnName("porteiro_id");
            entity.Property(e => e.SindicoId).HasColumnName("sindico_id");
            entity.Property(e => e.Telefone)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("telefone");
            entity.Property(e => e.UnidadeId).HasColumnName("unidade_id");

            entity.HasOne(d => d.Morador).WithMany(p => p.Visitantes)
                .HasForeignKey(d => d.MoradorId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_visitante_morador");

            entity.HasOne(d => d.Porteiro).WithMany(p => p.Visitantes)
                .HasForeignKey(d => d.PorteiroId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_visitante_porteiro");

            entity.HasOne(d => d.Sindico).WithMany(p => p.Visitantes)
                .HasForeignKey(d => d.SindicoId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_visitante_sindico");

            entity.HasOne(d => d.Unidade).WithMany(p => p.Visitantes)
                .HasForeignKey(d => d.UnidadeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_visitante_unidade");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
