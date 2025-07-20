using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    [Table("Promocao")]
    public class Promocao : EntityBase
    {
        
        [Required]
        [Column("Nome")]
        public required string Nome { get; set; }

        [Required]
        [Column("Desconto")]
        public required int Desconto { get; set; }

        [Required]
        [Column("DataValidade")]
        public required DateTime DataValidade { get; set; }

        public Promocao() { }

        public Promocao(string nome, int desconto, DateTime dataValidade)
        {
            if (dataValidade <= DateTime.Today)
                throw new ArgumentException("Data de validade deve ser maior que hoje no momento da criação.");

            Nome = nome;
            Desconto = desconto;
            DataValidade = dataValidade;
            DataCriacao = DateTime.Now;
        }       

        public bool EhValida() => DateTime.Now <= DataValidade;

        public bool EhValidaNaData(DateTime data) => data >= DataCriacao && data <= DataValidade;

        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    }
}
