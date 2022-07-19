using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Concessionaria.Lib.MinhasExceptions;
using Microsoft.AspNetCore.Mvc;
using ReconhecimentoFacial.Lib.Data.Repositorios.Interfaces;
using ReconhecimentoFacial.Lib.Models;
using ReconhecimentoFacial.Web.DTOs;

namespace ReconhecimentoFacial.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepositorio _repositorio;
        private readonly IAmazonS3 _amazonS3;
        private readonly AmazonRekognitionClient _rekognitionClient;
        private static readonly List<string> _extensoesImagem =
        new List<string>() { "image/jpeg", "image/png", "image/jpg"};
        public UsuarioController(IUsuarioRepositorio repositorio, IAmazonS3 amazonS3,
                                 AmazonRekognitionClient rekognitionClient)
        {
            _repositorio = repositorio;
            _amazonS3 = amazonS3;
            _rekognitionClient = rekognitionClient;

        }

        [HttpPost("Criar Usuário")]
        public async Task<IActionResult> CriarUsuario(UsuarioDTO usuarioDTO)
        {
            try
            {
                var usuario = new Usuario(usuarioDTO.Id, usuarioDTO.Email, usuarioDTO.Cpf, usuarioDTO.DataNascimento,
                                      usuarioDTO.Nome, usuarioDTO.Senha, usuarioDTO.DataCriacao);
                await _repositorio.Adicionar(usuario);

                return Ok();
            }
            catch (ValidacaoDeDados ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Cadastrar Imagem")]
        public async Task<IActionResult> CadastrarImagem(int id, IFormFile imagem)
        {
            var fileName = await SalvarNoS3(imagem);
            var imagemValida = await ValidarImagem(fileName);
            if (imagemValida)
            {
                await _repositorio.AtualizarUrlImagemCadastro(id, fileName);
                return Ok();
            }
            else
            {
                var resposta = await _amazonS3.DeleteObjectAsync("registro-facial", fileName);
                return BadRequest("Imagem Inválida!");
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



        [HttpGet("Todos os Usuários")]
        public async Task<IActionResult> BuscarUsuarios()
        {
            try
            {
                return Ok(await _repositorio.BuscarTodos());
            }
            catch (ValidacaoDeDados ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("Buscar por Id")]
        public async Task<IActionResult> BuscarUsuarioPorID(int id)
        {
            try
            {
                return Ok(await _repositorio.BuscarPorId(id));
            }
            catch (ValidacaoDeDados ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("LoginImagem")]
        public async Task<IActionResult> LoginImagem(int id, IFormFile imagem)
        {
            var usuario = await _repositorio.BuscarPorId(id);
            var imagemConfirmada = await CompararImagem(usuario.UrlImagemCadastro, imagem);
            if (imagemConfirmada)
            {
                return Ok("Id visual confirmada!");
            }
            return BadRequest("Acesso negado, Usuário incompatível!");
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
        [HttpGet("Login E-mail/Senha")]
        public async Task<IActionResult> LoginEmailSenha(string email, string senha)
        {
            var usuario = await _repositorio.BuscarUsuarioPorEmail(email);
            var verificacao = await ConferirSenha(usuario, senha);
            if (verificacao)
            {
                return Ok(usuario.Id);
            }
            return BadRequest("Login e Senha são incompatíveis!");
        }
        private async Task<bool> ConferirSenha(Usuario usuario, string senha)
        {
            if (usuario.Senha == senha)
            {
                return true;
            }
            return false;
        }
        [HttpPut("Atualizar E-mail")]
        public async Task<IActionResult> AtualizarEmailUsuarioPorId(int id, string email)
        {
            try
            {
                await _repositorio.AtualizarEmail(id, email);
                return Ok();
            }
            catch (ValidacaoDeDados ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete()]
        public async Task<IActionResult> DeletarUsuarioPorID(int id)
        {
            try
            {
                await _repositorio.DeletarItemDesejado(id);
                return Ok();
            }
            catch (ValidacaoDeDados ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}