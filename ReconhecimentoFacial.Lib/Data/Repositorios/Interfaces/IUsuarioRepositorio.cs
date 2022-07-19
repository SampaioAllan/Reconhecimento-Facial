using ReconhecimentoFacial.Lib.Models;
namespace ReconhecimentoFacial.Lib.Data.Repositorios.Interfaces
{
    public interface IUsuarioRepositorio : IRepositorioBase<Usuario>
    {
        Task AtualizarEmail(int id, string emailAtualizado);
        Task AtualizarUrlImagemCadastro(int id, string emailAtualizado);
        Task<Usuario> BuscarUsuarioPorEmail(string email);
    }
}