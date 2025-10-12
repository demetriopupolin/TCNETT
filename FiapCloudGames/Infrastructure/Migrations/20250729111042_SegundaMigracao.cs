using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SegundaMigracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE VIEW VW_PEDIDOS AS
SELECT PEDIDO_ID          = PEDIDO.ID
      ,PEDIDO_DATACRIACAO = PEDIDO.DATACRIACAO
      ,PEDIDO_VLPEDIDO    = PEDIDO.VLPEDIDO
      ,PEDIDO_VLDESCONTO  = PEDIDO.VLDESCONTO
      ,PEDIDO_VLPAGO      = PEDIDO.VLPAGO
      ,USUARIO_ID         = USUARIO.ID
      ,USUARIO_NOME       = USUARIO.NOME
      ,JOGO_ID            = JOGO.ID
      ,JOGO_NOME          = JOGO.NOME
      ,PROMOCAO_ID        = PROMOCAO.ID
      ,PROMOCAO_NOME      = PROMOCAO.NOME
FROM PEDIDO
INNER JOIN USUARIO
  ON USUARIO.ID = PEDIDO.USUARIOID
INNER JOIN JOGO
  ON JOGO.ID = PEDIDO.JOGOID
LEFT JOIN PROMOCAO
  ON PROMOCAO.ID = PEDIDO.PROMOCAOID;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP VIEW IF EXISTS VW_PEDIDOS;
            ");
        }
    }
}
