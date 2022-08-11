using Microsoft.EntityFrameworkCore;
using ReconhecimentoFacial.Lib.Data.Repositorios.Interfaces;
using ReconhecimentoFacial.Lib.Models;

namespace ReconhecimentoFacial.Lib.Data.Repositorios
{
    public class UsuarioRepositorio : RepositorioBase<Usuario>, IUsuarioRepositorio
    { 
        public UsuarioRepositorio(ReconhecimentoFacialContext context) : base(context, context.Usuarios)
        {

        }
        public async Task AtualizarEmail(Guid id, string emailAtualizado)
        {
            _dbSet.Find(id).SetEmail(emailAtualizado);
            await _context.SaveChangesAsync();
        }
        public async Task AtualizarUrlImagemCadastro(Guid id, string urlAtualizado)
        {
            _dbSet.Find(id).SetUrlImagemCadastro(urlAtualizado);
            await _context.SaveChangesAsync();
        }
         public async Task<Usuario> BuscarUsuarioPorEmail(string email)
        {
            return await _dbSet.AsNoTracking().FirstAsync(x => x.Email == email);
        }
    }
}