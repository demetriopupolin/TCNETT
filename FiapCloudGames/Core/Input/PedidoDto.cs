using Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Input
{
    public class PedidoDto
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public int UsuarioId { get; set; }
        public int JogoId { get; set; }
        public int? PromocaoId { get; set; }
        public decimal VlPedido { get; set; }
        public decimal VlDesconto { get; set; }
        public decimal VlPago { get; set; }

    }
}
