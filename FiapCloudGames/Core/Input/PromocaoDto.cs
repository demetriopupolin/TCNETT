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
    public class PromocaoDto
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public string Nome { get; set; }
        public int Desconto { get; set; }
        public DateTime DataValidade { get; set; }
        public virtual ICollection<PedidoDto> Pedidos { get; set; } = new List<PedidoDto>();
    }
}
