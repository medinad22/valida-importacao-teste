namespace genesis_valida_importacao_teste.HTTP.Liberty.Models
{
    public class Item
    {
        public int Codigo { get; set; }
        public DateTime DataReferencia { get; set; }
        public DateTime DataEmissao { get; set; }
        public RegiaoRisco RegiaoRisco { get; set; }
        public List<Cobertura> Cobertura { get; set; }
        public Veiculo Veiculo { get; set; }
        public Endosso Endosso { get; set; }
        public List<Franquia> Franquias { get; set; }
    }
}
