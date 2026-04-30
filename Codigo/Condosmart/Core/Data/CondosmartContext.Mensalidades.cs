using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Data;

public partial class CondosmartContext
{
    public virtual DbSet<ConfiguracaoMensalidade> ConfiguracaoMensalidades { get; set; }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConfiguracaoMensalidade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("configuracao_mensalidades");

            entity.HasIndex(e => e.CondominioId, "ux_config_mensalidade_condominio")
                .IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ativa)
                .HasDefaultValue(true)
                .HasColumnName("ativa");
            entity.Property(e => e.CondominioId).HasColumnName("condominio_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DiaVencimento).HasColumnName("dia_vencimento");
            entity.Property(e => e.QuantidadeParcelasPadrao)
                .HasDefaultValue(12)
                .HasColumnName("quantidade_parcelas_padrao");
            entity.Property(e => e.ValorMensalidade)
                .HasPrecision(10, 2)
                .HasColumnName("valor_mensalidade");

            entity.HasOne(d => d.Condominio).WithMany(p => p.ConfiguracoesMensalidade)
                .HasForeignKey(d => d.CondominioId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_config_mensalidade_condominio");
        });

        modelBuilder.Entity<Mensalidade>(entity =>
        {
            entity.Property(e => e.DataPagamento)
                .HasColumnType("date")
                .HasColumnName("data_pagamento");

            entity.Property(e => e.Status)
                .HasColumnType("enum('pendente','pago','atrasado','cancelada')")
                .HasDefaultValueSql("'pendente'")
                .HasColumnName("status");

            entity.Property(e => e.ValorFinal)
                .HasPrecision(10, 2)
                .HasColumnName("valor_final");

            entity.Property(e => e.ValorOriginal)
                .HasPrecision(10, 2)
                .HasColumnName("valor_original");
        });
    }
}
