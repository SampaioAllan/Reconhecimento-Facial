namespace ReconhecimentoFacial.Web.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string Nome { get; private set; }
        public string Senha { get; private set; }
        public string? UrlImagemCadastro { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public UsuarioDTO(int id, string email, string cpf, string dataNascimento, string nome, string senha, DateTime dataCriacao)
        {
            
        }
    }
}