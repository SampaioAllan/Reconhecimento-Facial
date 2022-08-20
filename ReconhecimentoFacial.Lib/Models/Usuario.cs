using System.Text;
using Konscious.Security.Cryptography;
using ReconhecimentoFacial.Lib.MinhasExceptions;
namespace ReconhecimentoFacial.Lib.Models
{
    public class Usuario : ModelBase
    {
        public string Email { get; private set; }
        public string Cpf { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string Nome { get; private set; }
        public string Senha { get; private set; }
        public string? UrlImagemCadastro { get; private set; }
        protected Usuario()
        {
            
        }
        public Usuario(string email, string cpf, DateTime dataNascimento, string nome, string senha, DateTime dataCriacao) : base(Guid.NewGuid(), dataCriacao)
        {
            SetEmail(email);
            SetCpf(cpf);
            SetDataNascimento(dataNascimento);
            SetNome(nome);
            Setsenha(senha);
            SetDataCriacao(dataCriacao);
        }
        public void SetEmail(string email)
        {
            Email = ValidarEmail(email);            
        }
        public void SetCpf(string cpf)
        {
            Cpf = ValidarCpf(cpf);
        }
        public void SetDataNascimento(DateTime dataNascimento)
        {
            DataNascimento = ValidarDataNascimento(dataNascimento);
        }
        public void SetNome(string nome)
        {
            Nome = nome;
        }
        public void Setsenha(string senha)
        {
            Senha = CriarHash(ValidarSenha(senha));
        }
        public void SetUrlImagemCadastro(string urlImagemCadastro)
        {
            UrlImagemCadastro = urlImagemCadastro;
        }
        public string ValidarEmail(string email)
        {
            if (email.Contains("@"))
            {
                return email;
            }
            throw new ValidacaoDeDados("Email deve conter @!");
        }
        public string ValidarCpf(string cpf)
        {          
            if (cpf.Length == 11 && cpf.All(char.IsNumber))
            {
                return cpf;
            }
            throw new ValidacaoDeDados("cpf inválido!");
        }
        public DateTime ValidarDataNascimento(DateTime dataNascimento)
        {
            if (dataNascimento.Year <2010)
            {
                return dataNascimento;
            }
            throw new ValidacaoDeDados("Data de nascimento inválida!");
        }      
        public string ValidarSenha(string senha)
        {
            if (senha.Length >= 8)
            {
                return senha;
            }
            throw new ValidacaoDeDados("Senha deve ter no mínimo 8 caracteres!");
        }
        private string CriarHash(string senha)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(senha));

            argon2.DegreeOfParallelism = 8;
            argon2.Iterations = 4;
            argon2.MemorySize = 1024 * 1024;

            var hash = argon2.GetBytes(16);

            return Convert.ToBase64String(hash);
        }  
        public bool VerificarHash(string senhaInserida, string hashSalvo)
        {
            var hashLogin = CriarHash(senhaInserida);
            return hashSalvo.SequenceEqual(hashLogin);
        }
    }
    
}   