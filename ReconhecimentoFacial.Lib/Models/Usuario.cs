using Concessionaria.Lib.MinhasExceptions;
namespace ReconhecimentoFacial.Lib.Models
{
    public class Usuario : ModelBase
    {
        public int Id { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string Nome { get; private set; }
        public string Senha { get; private set; }
        public string? UrlImagemCadastro { get; private set; }
        public DateTime DataCriacao { get; private set; }
        protected Usuario()
        {
            
        }
        public Usuario(int id, string email, string cpf, DateTime dataNascimento, string nome, string senha, DateTime dataCriacao)
        {
            SetId(id);
            SetEmail(email);
            SetCpf(cpf);
            SetDataNascimento(dataNascimento);
            SetNome(nome);
            Setsenha(senha);
            SetDataCriacao(dataCriacao);
        }
        public void SetId(int id)
        {
            Id = id;
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
            Senha = senha;
        }
        public void SetUrlImagemCadastro(string urlImagemCadastro)
        {
            UrlImagemCadastro = urlImagemCadastro;
        }
        public void SetDataCriacao(DateTime dataCriacao)
        {
            DataCriacao = dataCriacao;
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
    }
    
}   