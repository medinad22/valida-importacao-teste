namespace genesis_valida_importacao_teste.HTTP.Liberty.Models
{
    public class Segurado
    {
        public string NomeSegurado { get; set; }
        public long CpfCnpj { get; set; }
        public string TipoPessoa { get; set; }
        public string Sexo { get; set; }
        public DateTime DataNascimento { get; set; }
        public Endereco Endereco { get; set; }
        public List<string> Email { get; set; }
        public List<Telefone> Telefone { get; set; }
    }
}
