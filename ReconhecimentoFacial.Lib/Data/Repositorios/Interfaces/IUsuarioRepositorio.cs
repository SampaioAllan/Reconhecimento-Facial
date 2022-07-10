using ReconhecimentoFacial.Lib.Models;
namespace ReconhecimentoFacial.Lib.Data.Repositorios.Interfaces
{
    public interface IUsuarioRepositorio : IRepositorioBase<Usuario>
    {
        Task AtualizarEmail(int id, string emailAtualizado);
    }
}