namespace ReconhecimentoFacial.Application.DTOs
{
    public class UsuarioDTO
    {
        public string Email { get;  set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}