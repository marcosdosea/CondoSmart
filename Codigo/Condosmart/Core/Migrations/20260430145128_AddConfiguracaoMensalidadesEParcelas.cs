using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class AddConfiguracaoMensalidadesEParcelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "configuracao_mensalidades",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    condominio_id = table.Column<int>(type: "int", nullable: false),
                    valor_mensalidade = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    dia_vencimento = table.Column<int>(type: "int", nullable: false),
                    quantidade_parcelas_padrao = table.Column<int>(type: "int", nullable: false, defaultValue: 12),
                    ativa = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_config_mensalidade_condominio",
                        column: x => x.condominio_id,
                        principalTable: "condominios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "data_pagamento",
                table: "mensalidades",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "valor_final",
                table: "mensalidades",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "valor_original",
                table: "mensalidades",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "mensalidades",
                type: "enum('pendente','pago','atrasado','cancelada')",
                nullable: false,
                defaultValueSql: "'pendente'",
                oldClrType: typeof(string),
                oldType: "enum('pendente','pago','vencida','cancelada')",
                oldDefaultValueSql: "'pendente'")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.Sql(
                @"UPDATE mensalidades
                  SET valor_original = valor,
                      valor_final = valor
                  WHERE valor_original = 0 AND valor_final = 0;");

            migrationBuilder.Sql(
                @"UPDATE mensalidades
                  SET data_pagamento = created_at
                  WHERE status = 'pago' AND data_pagamento IS NULL;");

            migrationBuilder.Sql(
                @"UPDATE mensalidades
                  SET status = 'atrasado'
                  WHERE status = 'vencida';");

            migrationBuilder.CreateIndex(
                name: "ux_config_mensalidade_condominio",
                table: "configuracao_mensalidades",
                column: "condominio_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "configuracao_mensalidades");

            migrationBuilder.DropColumn(
                name: "data_pagamento",
                table: "mensalidades");

            migrationBuilder.DropColumn(
                name: "valor_final",
                table: "mensalidades");

            migrationBuilder.DropColumn(
                name: "valor_original",
                table: "mensalidades");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "mensalidades",
                type: "enum('pendente','pago','vencida','cancelada')",
                nullable: false,
                defaultValueSql: "'pendente'",
                oldClrType: typeof(string),
                oldType: "enum('pendente','pago','atrasado','cancelada')",
                oldDefaultValueSql: "'pendente'")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.Sql(
                @"UPDATE mensalidades
                  SET status = 'vencida'
                  WHERE status = 'atrasado';");
        }
    }
}
