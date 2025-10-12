using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Input
{
    public class JogoDto
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public string Nome { get; set; }
        public int AnoLancamento { get; set; }
        public decimal PrecoBase { get; set; }
        public virtual ICollection<PedidoDto> Pedidos { get; set; } = new List<PedidoDto>();
    }
}
