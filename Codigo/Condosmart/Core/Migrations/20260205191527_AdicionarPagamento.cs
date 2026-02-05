using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarPagamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // --- CRIAÇÃO DA TABELA PAGAMENTOS ---
            migrationBuilder.CreateTable(
                name: "pagamentos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    morador_id = table.Column<int>(type: "int", nullable: true),
                    unidade_id = table.Column<int>(type: "int", nullable: true),
                    condominio_id = table.Column<int>(type: "int", nullable: false),
                    forma_pagamento = table.Column<string>(type: "enum('pix','cartao_credito','cartao_debito','boleto','dinheiro')", nullable: false, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "enum('pendente','pago','cancelado','estornado')", nullable: false, defaultValueSql: "'pendente'", collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    valor = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    data_pagamento = table.Column<DateTime>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_pagamento_condominio",
                        column: x => x.condominio_id,
                        principalTable: "condominios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_pagamento_morador",
                        column: x => x.morador_id,
                        principalTable: "moradores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_pagamento_unidade",
                        column: x => x.unidade_id,
                        principalTable: "unidades_residenciais",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateIndex(
                name: "ix_pagamentos_condominio",
                table: "pagamentos",
                column: "condominio_id");

            migrationBuilder.CreateIndex(
                name: "ix_pagamentos_morador",
                table: "pagamentos",
                column: "morador_id");

            migrationBuilder.CreateIndex(
                name: "ix_pagamentos_unidade",
                table: "pagamentos",
                column: "unidade_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pagamentos");
        }
    }
}