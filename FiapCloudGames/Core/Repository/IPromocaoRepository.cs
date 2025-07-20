using Core.Entity;

namespace Core.Repository
{
    public interface IPromocaoRepository : IRepository<Promocao>
    {

        void CadastrarEmMassa(IEnumerable<Promocao> promocoes );

        Promocao ObterPorNome(string nome);
    }
}
