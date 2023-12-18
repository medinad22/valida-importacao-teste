namespace genesis_valida_importacao_teste.HTTP.Liberty.Models
{
    public class Corretora
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public List<string> Email { get; set; }
        public string CodigoSusep { get; set; }
        public string CodigoCorretor { get; set; }
        public string Nome { get; set; }
        public List<Telefone> Telefone { get; set; }
    }
}
