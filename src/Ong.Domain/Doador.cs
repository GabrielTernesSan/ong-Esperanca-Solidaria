namespace Ong.Domain
{
    public class Doador
    {
        public int Id { get; }
        public string NomeCompleto { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; private set; }
        public string Senha { get; private set; }

        public Doador(string nomeCompleto, string email, string cpf, string senha)
        {
            NomeCompleto = nomeCompleto;
            Email = email;
            Cpf = cpf;
            Senha = senha;
        }

        public Doador(int id, string nomeCompleto, string email, string cpf, string senha)
            : this (nomeCompleto, email, cpf, senha)
        {
            Id = id;
        }
    }
}
