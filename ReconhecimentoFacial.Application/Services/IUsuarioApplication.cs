using Microsoft.AspNetCore.Http;
using ReconhecimentoFacial.Application.DTOs;
using ReconhecimentoFacial.Lib.Models;

namespace ReconhecimentoFacial.Application.Services
{
    public interface IUsuarioApplication
    {
        Task<Guid> CriarUsuario(UsuarioDTO usuarioDto);
        Task CadastrarImagem(Guid id, IFormFile imagem);
        Task<List<Usuario>> BuscarTodos();
        Task<Usuario> BuscarPorId(Guid id);
        Task LoginImagem(Guid id, IFormFile imagem);
        Task<Guid> LoginEmailSenha(string email, string senha);
        Task AtualizarEmailUsuarioPorId(Guid id, string email);
        Task DeletarUsuarioPorID(Guid id);

    }
}