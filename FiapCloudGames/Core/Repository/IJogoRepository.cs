using Core.Entity;

namespace Core.Repository
{
    public interface IJogoRepository : IRepository<Jogo>
    {
        void CadastrarEmMassa(IEnumerable<Jogo> usuarios);
    }
}
