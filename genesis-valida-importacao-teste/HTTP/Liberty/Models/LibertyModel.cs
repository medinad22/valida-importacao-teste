using System.ComponentModel.DataAnnotations;

namespace genesis_valida_importacao_teste.HTTP.Liberty.Models
{
    public class LibertyModel
    {
        [Required(ErrorMessage = "O código da seguradora é obrigatório")]
        public int? Seguradora { get; set; }
        [Required(ErrorMessage = "O código da situação é obrigatório")]
        public int? Situacao { get; set; }
        public DateTime DataRequisicao { get; set; }
        public long? Apolice { get; set; }
        public string? DescricaoFilial { get; set; }
        public Segurado Segurado { get; set; }
        public Produto Produto { get; set; }
        public Corretora Corretora { get; set; }
        public List<Item> Item { get; set; }
        public List<Critica> Criticas { get; set; }
    }
}
