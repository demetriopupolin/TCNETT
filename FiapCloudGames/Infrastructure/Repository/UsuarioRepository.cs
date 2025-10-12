using Core.Entity;
using Core.Repository;

namespace Infrastructure.Repository
{
    public class UsuarioRepository : EFRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
        }
        public void CadastrarEmMassa(IEnumerable<Usuario> usuarios)
        {
            _context.AddRange(usuarios);
            _context.SaveChanges();
        }

        public Usuario ObterPorEmail(string email)
           => _dbSet.FirstOrDefault(entity => entity.Email == email);

        public bool UsuarioTemPedidos(int usuarioId)
        {
            return _context.Set<Pedido>().Any(p => p.UsuarioId == usuarioId);
        }

    }
}
