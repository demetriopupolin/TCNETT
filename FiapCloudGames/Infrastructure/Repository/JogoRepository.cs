using Core.Entity;
using Core.Repository;

namespace Infrastructure.Repository
{
    public class JogoRepository : EFRepository<Jogo>, IJogoRepository
    {
        public JogoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void CadastrarEmMassa(IEnumerable<Jogo> jogos)
        {
            _context.AddRange(jogos);
            _context.SaveChanges();
        }

        public bool JogoTemPedidos(int jogoId)
        {
            return _context.Set<Pedido>().Any(p => p.JogoId == jogoId);
        }
    }
}
