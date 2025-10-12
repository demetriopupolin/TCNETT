using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    [Table("Promocao")]
    public class Promocao : EntityBase
    {

        // Campos privados
        private string _nome;
        private int _desconto;
        private DateTime _dataValidade;

        [Required]
        [Column("Nome")]
        public string Nome
        {
            get => _nome;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nome é obrigatório.");
                _nome = value;
            }
        }


        [Required]
        [Column("Desconto")]
        public int Desconto
        {
            get => _desconto;
            private set
            {
                if (value < 10 || value > 90)
                    throw new ArgumentException("Desconto deve estar entre 10% e 90%.");
                _desconto = value;
            }
        }

        [Required]
        [Column("DataValidade")]
        public DateTime DataValidade
        {
            get => _dataValidade;
            private set
            {
                if (value <= DateTime.Today)
                    throw new ArgumentException("Data de validade deve ser posterior à data atual.");
                _dataValidade = value;
            }
        }

        public Promocao() { }

        public Promocao(string nome, int desconto, DateTime dataValidade)
        {   
            Nome = nome;
            Desconto = desconto;
            DataValidade = dataValidade;            
        }       

        public bool EhValida() => DateTime.Now <= DataValidade;

        public bool EhValidaNaData(DateTime data) => data >= DataCriacao && data <= DataValidade;

        public void AtualizarDesconto(int novoDesconto)
        {
            if (novoDesconto < 10 || novoDesconto > 90)
                throw new ArgumentException("Desconto inválido.");
            _desconto = novoDesconto;
        }

        public void AtualizarValidade(DateTime novaData)
        {
            
            if (novaData < DataCriacao)
                throw new ArgumentException("Data de validade inferior a data de criação");

            _dataValidade = novaData;
        }

        public void AtualizarNome(string novoNome)
        {
            if (string.IsNullOrWhiteSpace(novoNome))
                throw new ArgumentException("Nome é obrigatório.");
            _nome = novoNome;
        }

        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    }
}
