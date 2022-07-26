namespace ReconhecimentoFacial.Lib.MinhasExceptions
{
    public class ValidacaoDeDados : Exception
    {   
        public ValidacaoDeDados()
        {

        }
        public  ValidacaoDeDados(string msg) : base (msg)
        {

        }
    }
}