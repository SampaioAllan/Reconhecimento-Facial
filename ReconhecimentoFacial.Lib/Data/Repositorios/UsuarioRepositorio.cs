using ReconhecimentoFacial.Lib.Data.Repositorios.Interfaces;
using ReconhecimentoFacial.Lib.Models;

namespace ReconhecimentoFacial.Lib.Data.Repositorios
{
    public class UsuarioRepositorio : RepositorioBase<Usuario>, IUsuarioRepositorio
    {
        public UsuarioRepositorio(ReconhecimentoFacialContext context) : base(context, context.Usuarios)
        {

        }
        public async Task AtualizarEmail(int id, string emailAtualizado)
        {
            _dbSet.Find(id).SetEmail(emailAtualizado);
            await _context.SaveChangesAsync();
        }
    }
}