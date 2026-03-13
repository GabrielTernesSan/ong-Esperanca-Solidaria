using Ong.Domain.Enums;

namespace Ong.Domain
{
    public class Campanha
    {
        public int Id { get; }
        public string Titulo { get; private set; }
        public int Descricao { get; private set; }
        public DateTimeOffset DataInicio { get; private set; }
        public DateTimeOffset DataFim { get; private set; }
        public decimal Meta { get; private set; }
        public EStatusCampanha Status { get; private set; }

        public Campanha(string titulo, int descricao, DateTimeOffset dataInicio, DateTimeOffset dataFim, decimal meta, EStatusCampanha status)
        {
            Titulo = titulo;
            Descricao = descricao;
            DataInicio = dataInicio;
            DataFim = dataFim;
            Meta = meta;
            Status = status;
        }

        public Campanha(int id, string titulo, int descricao, DateTimeOffset dataInicio, DateTimeOffset dataFim, decimal meta, EStatusCampanha status)
            : this (titulo, descricao, dataInicio, dataFim, meta, status)
        {
            Id = id;
        }
    }
}
