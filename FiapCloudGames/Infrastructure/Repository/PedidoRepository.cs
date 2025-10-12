using Core.Entity;
using Core.Repository;

namespace Infrastructure.Repository
{
    public class PedidoRepository : EFRepository<Pedido>, IPedidoRepository
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public PedidoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void CadastrarEmMassa(IEnumerable<Pedido> pedidos)
        {
            _context.AddRange(pedidos);
            _context.SaveChanges();
        }
          


        public IEnumerable<Pedido> ObterPedidosPorUsuario(int usuarioId)
        {
           
           
            // retorna todos os pedidos com o UsuarioId passado
            return _context.Set<Pedido>()
                           .Where(p => p.UsuarioId == usuarioId)
                           .ToList();
        }

    }
}
