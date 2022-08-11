using ReconhecimentoFacial.Lib.Models;
namespace ReconhecimentoFacial.Lib.Data.Repositorios.Interfaces
{
    public interface IUsuarioRepositorio : IRepositorioBase<Usuario>
    {
        Task AtualizarEmail(Guid id, string novoEmail);
        Task AtualizarUrlImagemCadastro(Guid id, string novaUrl);
        Task<Usuario> BuscarUsuarioPorEmail(string email);
    }
}