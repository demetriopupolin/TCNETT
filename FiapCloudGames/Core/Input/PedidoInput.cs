using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Input
{
    public class PedidoInput
    {
        public int? UsuarioId { get; set; }
        public int? JogoId { get; set; }
        public int? PromocaoId { get; set; }

    }
}
