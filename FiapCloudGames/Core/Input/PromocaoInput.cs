using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Input
{
    public class PromocaoInput
    {
        public string Nome { get; set; }

        public int Desconto { get; set; }

        public DateTime DataValidade { get; set; }

    }
}
