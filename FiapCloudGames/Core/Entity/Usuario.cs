using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    [Table("Usuario")]
    public class Usuario : EntityBase
    {

        [Required]
        [Column("Nome")]
        public required string Nome { get; set; }

        [Required]
        [Column("Email")]
        public required string Email { get; set; }

        [Required]
        [Column("Senha")]
        public required string Senha { get; set; }

        [Required]
        [Column("Nivel")]
        public char Nivel { get; set; } = 'U';

        public Usuario() { }
               
        public Usuario(string nome, string email, string senha)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
            DataCriacao = DateTime.Now;
            Nivel = 'U';
        }
        
        public bool ValidarSenha(string senhaInformada) => Senha == senhaInformada;
        
        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
