using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Data;

public partial class CondosmartContext
{
    public virtual DbSet<ConfiguracaoMensalidade> ConfiguracaoMensalidades { get; set; }
    public virtual DbSet<NotificacaoSistema> NotificacoesSistema { get; set; }

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

        modelBuilder.Entity<NotificacaoSistema>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("notificacoes_sistema");

            entity.HasIndex(e => e.CondominioId, "ix_notificacoes_condominio");
            entity.HasIndex(e => e.CreatedAt, "ix_notificacoes_created_at");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CondominioId).HasColumnName("condominio_id");
            entity.Property(e => e.UsuarioEmail)
                .HasMaxLength(120)
                .HasColumnName("usuario_email");
            entity.Property(e => e.UsuarioNome)
                .HasMaxLength(120)
                .HasColumnName("usuario_nome");
            entity.Property(e => e.Titulo)
                .HasMaxLength(120)
                .HasColumnName("titulo");
            entity.Property(e => e.Mensagem)
                .HasMaxLength(500)
                .HasColumnName("mensagem");
            entity.Property(e => e.Tipo)
                .HasMaxLength(30)
                .HasColumnName("tipo");
            entity.Property(e => e.UrlDestino)
                .HasMaxLength(250)
                .HasColumnName("url_destino");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");

            entity.HasOne(d => d.Condominio).WithMany(p => p.NotificacoesSistema)
                .HasForeignKey(d => d.CondominioId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_notificacao_condominio");
        });

        modelBuilder.Entity<Ata>(entity =>
        {
            entity.Property(e => e.ArquivoNomeOriginal)
                .HasMaxLength(255)
                .HasColumnName("arquivo_nome_original");

            entity.Property(e => e.ArquivoCaminho)
                .HasMaxLength(255)
                .HasColumnName("arquivo_caminho");
        });

        modelBuilder.Entity<AreaDeLazer>(entity =>
        {
            entity.Property(e => e.ImagemNomeOriginal)
                .HasMaxLength(255)
                .HasColumnName("imagem_nome_original");

            entity.Property(e => e.ImagemCaminho)
                .HasMaxLength(255)
                .HasColumnName("imagem_caminho");
        });
    }
}
