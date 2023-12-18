namespace genesis_valida_importacao_teste.HTTP.Liberty.Models
{
    public class Plano
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicioVigencia { get; set; }
        public DateTime DataFimVigencia { get; set; }
        public string TipoMovimento { get; set; }
    }
}
