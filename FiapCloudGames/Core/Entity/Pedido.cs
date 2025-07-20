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

        public Pedido() { }

        public Pedido(int usuarioId, int jogoId, int? promocaoId = null)
        {
            UsuarioId = usuarioId;
            JogoId = jogoId;
            PromocaoId = promocaoId;
            DataCriacao = DateTime.Now;
        }

        public virtual Usuario Usuario { get; set; }
        public virtual Jogo Jogo { get; set; }
        public virtual Promocao Promocao { get; set; }
    }
}
