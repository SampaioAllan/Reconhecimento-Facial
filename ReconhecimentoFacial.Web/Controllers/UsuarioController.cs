using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CriarUsuario(UsuarioDTO usuarioDTO)
        {
            var response = await _application.CriarUsuario(usuarioDTO);
            return Ok(response);
        }
        [HttpPost("Imagem")]
        public async Task<IActionResult> CadastrarImagem(int id, IFormFile imagem)
        {
            await _application.CadastrarImagem(id, imagem);  
            return Ok();          
        }
        [HttpGet()]
        public async Task<IActionResult> BuscarUsuarios()
        {
            var listaUsuarios = await _application.BuscarTodos();
            return Ok(listaUsuarios);
            
        }
        [HttpGet("Id")]
        public async Task<IActionResult> BuscarUsuarioPorID(int id)
        {
            var usuario = await _application.BuscarPorId(id);
            return Ok(usuario);
        }
        [HttpPost("LoginImagem")]
        public async Task<IActionResult> LoginImagem(int id, IFormFile imagem)
        {
            await _application.LoginImagem(id, imagem);
            return Ok("Id visual confirmada!");
        }
        [HttpGet("LoginE-mail")]
        public async Task<IActionResult> LoginEmailSenha(string email, string senha)
        {
            return Ok(await _application.LoginEmailSenha(email, senha));
        }
        [HttpPut("E-mail")]
        public async Task<IActionResult> AtualizarEmailUsuarioPorId(int id, string email)
        {
            await _application.AtualizarEmailUsuarioPorId(id, email);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeletarUsuarioPorID(int id)
        {
            await _application.DeletarUsuarioPorID(id);
            return Ok();            
        }
    }
}