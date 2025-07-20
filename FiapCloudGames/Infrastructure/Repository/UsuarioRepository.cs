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

        public Usuario? ObterPorEmail(string email)
        {
            return _context.Set<Usuario>().FirstOrDefault(u => u.Email == email);
        }
    }
}
