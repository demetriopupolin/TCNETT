﻿namespace Core.Repository
{
    public interface IRepository<T>
    {
        IList<T> ObterTodos();        
        T ObterPorId (int id);
        void Cadastrar(T entidade);
        void Alterar(T entidade);
        void Deletar(int id);

    }
}
