namespace genesis_valida_importacao_teste.HTTP.Liberty.Models
{
    public class Veiculo
    {
        public string Modelo { get; set; }
        public string Chassi { get; set; }
        public string Placa { get; set; }
        public string Montadora { get; set; }
        public object Combustivel { get; set; }
        public string CategoriaVeiculo { get; set; }
        public int AnoModelo { get; set; }
        public int AnoFabricacao { get; set; }
        public bool ZeroKM { get; set; }
        public string NaturezaItem { get; set; }
    }
}
