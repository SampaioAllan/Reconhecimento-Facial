using System;
namespace ReconhecimentoFacial.Lib.Models
{
    public class ModelBase
    {
        public Guid Id { get; private set; }
        public DateTime DataCriacao { get; private set; }
        protected ModelBase()
        {

        }
        public ModelBase(Guid id, DateTime dataCriacao)
        {
            SetId(id);
            SetDataCriacao(dataCriacao);
        }
        public void SetId(Guid id)
        {
            Id = id;
        }
        public void SetDataCriacao(DateTime dataCriacao)
        {
            DataCriacao = dataCriacao;
        }
    }
}