using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    [Table("Pedido")]
    public class Pedido : EntityBase
    {
        [Required]
        [Column("UsuarioId")]
        public required int UsuarioId { get; set; }

        [Required]
        [Column("JogoId")]
        public required int JogoId { get; set; }

        [Column("PromocaoId")]
        public int? PromocaoId { get; set; }

        [Required]
        [Column("VlPedido")]
        public decimal VlPedido { get; private set; }

        [Required]
        [Column("VlDesconto")]
        public decimal VlDesconto { get; private set; }

        [Required]
        [Column("VlPago")]
        public decimal VlPago { get; private set; }

        // Propriedades de navegação (podem ser setadas pelo ORM)
        public virtual Usuario Usuario { get; set; }
        public virtual Jogo Jogo { get; set; }
        public virtual Promocao Promocao { get; set; }

        public Pedido() { }

        public Pedido(int usuarioId, int jogoId, int? promocaoId = null)
        {
            if (usuarioId <= 0)
                throw new ArgumentException("Usuário inválido.");

            if (jogoId <= 0)
                throw new ArgumentException("Jogo inválido.");

            UsuarioId = usuarioId;
            JogoId = jogoId;
            PromocaoId = promocaoId;
         
        }

        /// <summary>
        /// Recebe as entidades associadas e valida o pedido conforme as regras, calculando os valores.
        /// </summary>
        public void ValidarECalcularPedido(Usuario usuario, Jogo jogo, Promocao? promocao = null)
        {
            if (usuario == null || usuario.Id != UsuarioId)
                throw new ArgumentException("Usuário inválido ou não corresponde ao ID informado.");

            if (jogo == null || jogo.Id != JogoId)
                throw new ArgumentException("Jogo inválido ou não corresponde ao ID informado.");

            if (jogo.PrecoBase <= 0)
                throw new ArgumentException("Preço base do jogo inválido.");

            if (promocao != null)
            {
                if (promocao.Id != PromocaoId)
                    throw new ArgumentException("Promoção não corresponde ao ID informado.");

                if (promocao.DataValidade < DataCriacao)
                    throw new ArgumentException("Promoção expirada para a data do pedido.");

                if (promocao.Desconto < 10 || promocao.Desconto > 90)
                    throw new ArgumentException("Percentual de desconto da promoção inválido.");
            }

            Usuario = usuario;
            Jogo = jogo;
            Promocao = promocao;

            VlPedido = jogo.PrecoBase;
            VlDesconto = promocao != null ? VlPedido * promocao.Desconto / 100m : 0m;
            VlPago = VlPedido - VlDesconto;

            if (VlPago < 0)
                throw new ArgumentException("Valor pago não pode ser negativo.");
        }
    }
}
