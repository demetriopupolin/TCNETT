using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Core.Entity
{
    [Table("Usuario")]
    public class Usuario : EntityBase
    {
        private string _nome;
        private string _email;
        private string _senha;

        [Required]
        [Column("Nome")]
        public required string Nome
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
        [Column("Email")]
        public string Email
        {
            get => _email;
            set
            {
                if (!EmailEhValido(value))
                    throw new ArgumentException("E-mail inválido.");
                _email = value;
            }
        }

     

        [Required]
        [Column("Senha")]
        public string Senha
        {
            get => _senha;
            set
            {
                if (!SenhaEhForte(value))
                    throw new ArgumentException("A senha deve conter no mínimo 8 caracteres, incluindo letras, números e caracteres especiais.");
                _senha = value;
            }
        }

        [Required]
        [Column("Nivel")]
        public char Nivel { get; set; } = 'U';

        public Usuario() { }

        public Usuario(string nome, string email, string senha)
        {
           
            Nome = nome;     // Usa o setter com validação
            Email = email;   // Usa o setter com validação
            Senha = senha;   // Usa o setter com validação
            Nivel = 'U';
        }

        private static bool EmailEhValido(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            // Verifica se contém letras maiúsculas
            if (value.Any(char.IsUpper))
                return false;

            return Regex.IsMatch(value, @"^[\w\-\.]+@([\w\-]+\.)+[\w\-]{2,4}$");
        }

        private static bool SenhaEhForte(string senha)
        {
            
            if (string.IsNullOrWhiteSpace(senha))
                return false;

            

            // Pelo menos 8 caracteres, 1 letra, 1 número e 1 caractere especial
            return Regex.IsMatch(senha, @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[\W_]).{8,}$");
        }

        public bool ValidarSenha(string senhaInformada) => Senha == senhaInformada;

        public void AtualizarNome(string novoNome)
        {
            if (string.IsNullOrWhiteSpace(novoNome))
                throw new ArgumentException("Nome é obrigatório.");
            _nome = novoNome;
        }

        public void AtualizarSenha(string novaSenha)
        {
            if (!SenhaEhForte(novaSenha))
                throw new ArgumentException("A senha deve conter no mínimo 8 caracteres, incluindo letras, números e caracteres especiais.");
            _senha = novaSenha;
        }

        public void AtualizarEmail(string novoEmail)
        {
            if (!EmailEhValido(novoEmail))
                throw new ArgumentException("E-mail inválido.");
            _email = novoEmail;
        }

        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
