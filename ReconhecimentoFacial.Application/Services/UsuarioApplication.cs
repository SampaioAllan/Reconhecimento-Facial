using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.S3;
using Amazon.S3.Model;
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
        private readonly IAmazonS3 _amazonS3;
        private readonly AmazonRekognitionClient _rekognitionClient;
        private static readonly List<string> _extensoesImagem =
        new List<string>() { "image/jpeg", "image/png", "image/jpg" };
        public UsuarioApplication(IUsuarioRepositorio repositorio, IAmazonS3 amazonS3, AmazonRekognitionClient rekognitionClient)
        {
            _repositorio = repositorio;
            _amazonS3 = amazonS3;
            _rekognitionClient = rekognitionClient;
        }
        public async Task<int> CriarUsuario(UsuarioDTO usuarioDTO)
        {
            try
            {
                var usuario = new Usuario(usuarioDTO.Id, usuarioDTO.Email, usuarioDTO.Cpf, usuarioDTO.DataNascimento,
                                      usuarioDTO.Nome, usuarioDTO.Senha, usuarioDTO.DataCriacao);
                await _repositorio.Adicionar(usuario);

                return (usuario.Id);
            }
            catch (ValidacaoDeDados ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> CadastrarImagem(int id, IFormFile imagem)
        {
            var nomeArquivo = await SalvarNoS3(imagem);
            var imagemValida = await ValidarImagem(nomeArquivo);
            if (imagemValida)
            {
                await _repositorio.AtualizarUrlImagemCadastro(id, nomeArquivo);
                return (true);
            }
            else
            {
                var resposta = await _amazonS3.DeleteObjectAsync("registro-facial", nomeArquivo);
                return (false);
            }
        }
            private async Task<string> SalvarNoS3(IFormFile imagem)
            {
                if (!_extensoesImagem.Contains(imagem.ContentType))
                    throw new Exception("Formato Inválido!");
                using (var streamDaImagem = new MemoryStream())
                {
                    await imagem.CopyToAsync(streamDaImagem);

                    var request = new PutObjectRequest();
                    request.Key = "reconhecimento" + imagem.FileName;
                    request.BucketName = "registro-facial";
                    request.InputStream = streamDaImagem;

                    var resposta = await _amazonS3.PutObjectAsync(request);
                    return request.Key;
                }
            }
            private async Task<bool> ValidarImagem(string nomeArquivo)
            {

                var entrada = new DetectFacesRequest();
                var imagem = new Image();
                var s3Object = new Amazon.Rekognition.Model.S3Object()
                {
                    Bucket = "registro-facial",
                    Name = nomeArquivo
                };

                imagem.S3Object = s3Object;
                entrada.Image = imagem;
                entrada.Attributes = new List<string>() { "ALL" };

                var resposta = await _rekognitionClient.DetectFacesAsync(entrada);

                if (resposta.FaceDetails.Count() == 1 && resposta.FaceDetails.First().Eyeglasses.Value == false)
                {
                    return true;
                }
                return false;
            }
        public async Task<List<Usuario>> BuscarTodos()
        {
            return await _repositorio.BuscarTodos();
        }
        public async Task<Usuario> BuscarPorId(int id)
        {
            return await _repositorio.BuscarPorId(id);
        }
        public async Task<bool> LoginImagem(int id, IFormFile imagem)
        {
            var usuario = await _repositorio.BuscarPorId(id);
            var imagemConfirmada = await CompararImagem(usuario.UrlImagemCadastro, imagem);
            if (imagemConfirmada)
            {
                return (true);
            }
            return (false);
        }
            private async Task<bool> CompararImagem(string nomeArquivoS3, IFormFile fotoLogin)
            {
                using (var memoryStream = new MemoryStream())
                {
                    var request = new CompareFacesRequest();
                    var requestSource = new Image()
                    {
                        S3Object = new Amazon.Rekognition.Model.S3Object()
                        {
                            Bucket = "registro-facial",
                            Name = nomeArquivoS3
                        }
                    };


                    await fotoLogin.CopyToAsync(memoryStream);
                    var requestTarget = new Image()
                    {
                        Bytes = memoryStream
                    };

                    request.SourceImage = requestSource;
                    request.TargetImage = requestTarget;

                    var response = await _rekognitionClient.CompareFacesAsync(request);
                    if (response.FaceMatches.Count == 1 && response.FaceMatches.First().Similarity >= 80)
                    {
                        return true;
                    }
                    return false;
                }
            }
        public async Task<int> LoginEmailSenha(string email, string senha)
        {
            var usuario = await _repositorio.BuscarUsuarioPorEmail(email);
            var verificacao = await ConferirSenha(usuario, senha);
            if (verificacao)
            {
                return (usuario.Id);
            }
            throw new ValidacaoDeDados("Login e Senha são incompatíveis!");
        }   
            private async Task<bool> ConferirSenha(Usuario usuario, string senha)
            {
                return usuario.Senha == senha;
            }
        public async Task<bool> AtualizarEmailUsuarioPorId(int id, string email)
        {
            try
            {
                await _repositorio.AtualizarEmail(id, email);
                return (true);
            }
            catch (ValidacaoDeDados ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeletarUsuarioPorID(int id)
        {
            try
            {
                await _repositorio.DeletarItemDesejado(id);
                return (true);
            }
            catch (ValidacaoDeDados ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}