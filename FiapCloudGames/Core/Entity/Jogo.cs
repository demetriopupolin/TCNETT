using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    [Table("Jogo")]
    public class Jogo : EntityBase
    {
        private string _nome;
        private int _anoLancamento;
        private decimal _precoBase;

        [Required]
        [Column("Nome")]
        public string Nome
        {
            get => _nome;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nome é obrigatório.");
                _nome = value;
            }
        }

        [Required]
        [Column("AnoLancamento")]
        public int AnoLancamento
        {
            get => _anoLancamento;
            set
            {
                int anoCriacao = DataCriacao.Year;
                int anoAtual = DateTime.Now.Year;

                if (value < anoCriacao)
                    throw new ArgumentException("Ano de lançamento não pode ser inferior ao ano de criação do registro.");

                if (value > anoAtual)
                    throw new ArgumentException("Ano de lançamento não pode ser superior ao ano corrente.");

                _anoLancamento = value;
            }
        }

        [Required]
        [Column("PrecoBase")]
        public decimal PrecoBase
        {
            get => _precoBase;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Preço base deve ser maior que zero.");
                _precoBase = value;
            }
        }

        // Construtores
        public Jogo() { }

        public Jogo(string nome, int anoLancamento, decimal precoBase)
        {
            Nome = nome;
            AnoLancamento = anoLancamento;
            PrecoBase = precoBase;
        }

        // Propriedade de navegação para Pedidos, se precisar
        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
