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
        public UsuarioController(IUsuarioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpPost()]
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
        [HttpGet()]
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
        [HttpGet("Id")]
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
        [HttpPut()]
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