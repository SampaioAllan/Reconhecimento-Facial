using System;
namespace ReconhecimentoFacial.Lib.Models
{
    public class ModelBase
    {
        public int Id { get; private set; }
        public DateTime DataCriacao { get; private set; }
        protected ModelBase()
        {

        }
        public ModelBase(int id, DateTime dataCriacao)
        {
            SetId(id);
            SetDataCriacao(dataCriacao);
        }
        public void SetId(int id)
        {
            Id = id;
        }
        public void SetDataCriacao(DateTime dataCriacao)
        {
            DataCriacao = dataCriacao;
        }
    }
}