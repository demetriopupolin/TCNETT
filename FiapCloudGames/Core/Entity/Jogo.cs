using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    [Table("Jogo")]
    public class Jogo : EntityBase
    {

        [Required]
        [Column("Nome")]
        public required string Nome { get; set; }

        [Required]
        [Column("AnoLancamento")]
        public required int AnoLancamento { get; set; }

        [Required]
        [Column("PrecoBase")]
        public required decimal PrecoBase { get; set; }

        public Jogo() { }

        public Jogo(string nome, int anoLancamento, decimal precoBase)
        {
            Nome = nome;
            AnoLancamento = anoLancamento;
            PrecoBase = precoBase;
            DataCriacao = DateTime.Now;
        }

        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    }
}
