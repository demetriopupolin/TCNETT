using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Input
{
    public class PromocaoInput
    {
        public string Nome { get; set; }

        [Range(10, 90, ErrorMessage = "Desconto deve ser de 10% a 90%.")]
        public int Desconto { get; set; }

        public DateTime DataValidade { get; set; }

    }
}
