using ReconhecimentoFacial.Services;
using Microsoft.AspNetCore.Http;
using ReconhecimentoFacial.Application.DTOs;
using ReconhecimentoFacial.Lib.Data.Repositorios.Interfaces;
using ReconhecimentoFacial.Lib.MinhasExceptions;
using ReconhecimentoFacial.Lib.Models;

namespace ReconhecimentoFacial.Application.Services
{
    public class UsuarioApplication : IUsuarioApplication
    {
        private readonly IUsuarioRepositorio _repositorio;
        private readonly AwsServices _awsServices;
        public UsuarioApplication(IUsuarioRepositorio repositorio, AwsServices awsServices)
        {
            _repositorio = repositorio;
            _awsServices = awsServices;
        }
        public async Task<Guid> CriarUsuario(UsuarioDTO usuarioDTO)
        {
            var usuario = new Usuario(usuarioDTO.Email, usuarioDTO.Cpf, usuarioDTO.DataNascimento,
                                    usuarioDTO.Nome, usuarioDTO.Senha, usuarioDTO.DataCriacao);
            await _repositorio.Adicionar(usuario);

            return (usuario.Id);
        }        
        public async Task CadastrarImagem(Guid id, IFormFile imagem)
        {
            var nomeArquivo = await _awsServices.SalvarNoS3(imagem);
            var imagemValida = await _awsServices.ValidarImagem(nomeArquivo);
            if (imagemValida)
            {
                await _repositorio.AtualizarUrlImagemCadastro(id, nomeArquivo);
            }
            else
            {
                await _awsServices.DeletarNoS3(nomeArquivo);
                throw new Exception("Imagem não foi salva!");
            } 
        }      
        public async Task<List<Usuario>> BuscarTodos()
        {
            return await _repositorio.BuscarTodos();
        }        
        public async Task<Usuario> BuscarPorId(Guid id)
        {
            return await _repositorio.BuscarPorId(id);
        }        
        public async Task LoginImagem(Guid id, IFormFile imagem)
        {
            var usuario = await _repositorio.BuscarPorId(id);
            var imagemConfirmada = await _awsServices.CompararImagem(usuario.UrlImagemCadastro, imagem);
            if (!imagemConfirmada)
                throw new ValidacaoDeDados("Acesso negado, Usuário incompatível!");     
        }        
        public async Task<Guid> LoginEmailSenha(string email, string senha)
        {
            var usuario = await _repositorio.BuscarUsuarioPorEmail(email);
            var verificacao = usuario.VerificarHash(senha, usuario.Senha);
            if (verificacao)
            {
                return (usuario.Id);
            }
            else
                throw new ValidacaoDeDados("Login e Senha são incompatíveis!");
        }        
        public async Task AtualizarEmailUsuarioPorId(Guid id, string email)
        {
            await _repositorio.AtualizarEmail(id, email);
        }
        public async Task DeletarUsuarioPorID(Guid id)
        {
            await _repositorio.DeletarItemDesejado(id);
        }
    }
}