using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PrimeiraMigracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JOGO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOME = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    ANOLANCAMENTO = table.Column<int>(type: "INT", nullable: false),
                    PRECOBASE = table.Column<decimal>(type: "DECIMAL(18,0)", nullable: false),
                    DATACRIACAO = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JOGO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PROMOCAO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOME = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    DESCONTO = table.Column<int>(type: "INT", nullable: false),
                    DATAVALIDADE = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    DATACRIACAO = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROMOCAO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "USUARIO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOME = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    EMAIL = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    SENHA = table.Column<string>(type: "VARCHAR(8)", nullable: false),
                    DATACRIACAO = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PEDIDO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    USUARIOID = table.Column<int>(type: "INT", nullable: false),
                    JOGOID = table.Column<int>(type: "INT", nullable: false),
                    PROMOCAOID = table.Column<int>(type: "INT", nullable: true),
                    DATACRIACAO = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PEDIDO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PEDIDO_JOGO_JOGOID",
                        column: x => x.JOGOID,
                        principalTable: "JOGO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PEDIDO_PROMOCAO_PROMOCAOID",
                        column: x => x.PROMOCAOID,
                        principalTable: "PROMOCAO",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PEDIDO_USUARIO_USUARIOID",
                        column: x => x.USUARIOID,
                        principalTable: "USUARIO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PEDIDO_JOGOID",
                table: "PEDIDO",
                column: "JOGOID");

            migrationBuilder.CreateIndex(
                name: "IX_PEDIDO_PROMOCAOID",
                table: "PEDIDO",
                column: "PROMOCAOID");

            migrationBuilder.CreateIndex(
                name: "IX_PEDIDO_USUARIOID",
                table: "PEDIDO",
                column: "USUARIOID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PEDIDO");

            migrationBuilder.DropTable(
                name: "JOGO");

            migrationBuilder.DropTable(
                name: "PROMOCAO");

            migrationBuilder.DropTable(
                name: "USUARIO");
        }
    }
}
