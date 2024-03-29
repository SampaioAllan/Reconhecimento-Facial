using ReconhecimentoFacial.Lib.Models;

namespace ReconhecimentoFacial.Lib.Data.Repositorios.Interfaces
{
    public interface IRepositorioBase<T> where T : ModelBase
    {
         Task Adicionar(T item);
         Task<T> BuscarPorId(Guid id);
         Task<List<T>> BuscarTodos();
         Task DeletarItemDesejado(Guid id);
    }
}