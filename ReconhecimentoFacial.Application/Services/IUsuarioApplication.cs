using Microsoft.AspNetCore.Http;
using ReconhecimentoFacial.Application.DTOs;
using ReconhecimentoFacial.Lib.Models;

namespace ReconhecimentoFacial.Application.Services
{
    public interface IUsuarioApplication
    {
        Task<int> CriarUsuario(UsuarioDTO usuarioDto);
        Task<bool> CadastrarImagem(int id, IFormFile imagem);
        Task<List<Usuario>> BuscarTodos();
        Task<Usuario> BuscarPorId(int id);
        Task<bool> LoginImagem(int id, IFormFile imagem);
        Task<int> LoginEmailSenha(string email, string senha);
        Task<bool> AtualizarEmailUsuarioPorId(int id, string email);
        Task<bool> DeletarUsuarioPorID(int id);

    }
}