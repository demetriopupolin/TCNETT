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

        public IEnumerable<Pedido> ObterPedidosPorEmailUsuario(string email)
        {
            var usuario = _usuarioRepository.ObterPorEmail(email);// pega usuário pelo email
            if (usuario == null)
                return Enumerable.Empty<Pedido>(); // retorna lista vazia se usuário não existe

            // retorna todos os pedidos com o UsuarioId do usuário encontrado
            return _context.Set<Pedido>()
                           .Where(p => p.UsuarioId == usuario.Id)
                           .ToList();
        }
    }
}
