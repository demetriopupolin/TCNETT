using Core.Entity;

namespace Core.Repository
{
    public interface IUsuarioRepository : IRepository<Usuario> 
    {
        void CadastrarEmMassa(IEnumerable<Usuario> usuarios);

        Usuario ObterPorEmail (string email);

        public bool UsuarioTemPedidos(int usuarioId);

    }
}
