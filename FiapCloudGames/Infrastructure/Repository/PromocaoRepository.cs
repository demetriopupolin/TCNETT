using Core.Entity;
using Core.Repository;

namespace Infrastructure.Repository
{
    public class PromocaoRepository : EFRepository<Promocao>, IPromocaoRepository
    {
        public PromocaoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void CadastrarEmMassa(IEnumerable<Promocao> promocoes)
        {
            _context.AddRange(promocoes);
            _context.SaveChanges();
        }

        public Promocao? ObterPorNome(string nome)
        {
            return _context.Set<Promocao>().FirstOrDefault(p => p.Nome == nome);
        }

        public bool PromocaoTemPedidos(int promocaoId)
        {
            return _context.Set<Pedido>().Any(p => p.PromocaoId == promocaoId);
        }
    }
}
