using Microsoft.AspNetCore.Mvc;
using ReconhecimentoFacial.Lib.MinhasExceptions;
using ReconhecimentoFacial.Application.DTOs;
using ReconhecimentoFacial.Application.Services;

namespace ReconhecimentoFacial.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioApplication _application;
        public UsuarioController(IUsuarioApplication application)
        {
            _application = application;
        }

        [HttpPost]
        public async Task<int> CriarUsuario(UsuarioDTO usuarioDTO)
        {
            return await _application.CriarUsuario(usuarioDTO);
        }
        [HttpPost("Imagem")]
        public async Task<IActionResult> CadastrarImagem(int id, IFormFile imagem)
        {
            var imagemCadastrada = await _application.CadastrarImagem(id, imagem);
            if (imagemCadastrada)
                return Ok();            
            else
                return BadRequest("Imagem Inválida!");
            
        }
        [HttpGet()]
        public async Task<IActionResult> BuscarUsuarios()
        {
            try
            {
                return Ok(await _application.BuscarTodos());
            }
            catch (ValidacaoDeDados ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("Id")]
        public async Task<IActionResult> BuscarUsuarioPorID(int id)
        {
            try
            {
                return Ok(await _application.BuscarPorId(id));
            }
            catch (ValidacaoDeDados ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("LoginImagem")]
        public async Task<IActionResult> LoginImagem(int id, IFormFile imagem)
        {
            var imagemConfirmada = await _application.LoginImagem(id, imagem);
            if (imagemConfirmada)
                return Ok("Id visual confirmada!");
            else
                return BadRequest("Acesso negado, Usuário incompatível!");
        }
        [HttpGet("LoginE-mail")]
        public async Task<IActionResult> LoginEmailSenha(string email, string senha)
        {
            try
            {
                return Ok(await _application.LoginEmailSenha(email, senha));
            }
            catch (ValidacaoDeDados ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("Atualizar E-mail")]
        public async Task<IActionResult> AtualizarEmailUsuarioPorId(int id, string email)
        {
            var emailAtualizado = await _application.AtualizarEmailUsuarioPorId(id, email);
            if(emailAtualizado)
                return Ok();
            else
                return BadRequest();
        }
        [HttpDelete()]
        public async Task<IActionResult> DeletarUsuarioPorID(int id)
        {
            var usuarioDeletado = await _application.DeletarUsuarioPorID(id);
            if(usuarioDeletado)
                return Ok();
            else
                return BadRequest();            
        }
    }
}