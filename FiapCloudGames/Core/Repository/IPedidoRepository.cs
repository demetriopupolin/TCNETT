using Core.Entity;

namespace Core.Repository
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        void CadastrarEmMassa(IEnumerable<Pedido> pedidos);

        public IEnumerable<Pedido> ObterPedidosPorUsuario(int id);
        
    }
}