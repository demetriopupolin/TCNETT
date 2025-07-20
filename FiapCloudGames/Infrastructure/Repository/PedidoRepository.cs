using Core.Entity;
using Core.Repository;

namespace Infrastructure.Repository
{
    public class PedidoRepository : EFRepository<Pedido>, IPedidoRepository
    {
        public PedidoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void CadastrarEmMassa(IEnumerable<Pedido> pedidos)
        {
            _context.AddRange(pedidos);
            _context.SaveChanges();
        }
    }
}
