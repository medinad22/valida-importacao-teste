namespace genesis_valida_importacao_teste.valida_arquivo
{
    public class Validadores
    {
        public string Diretorio { get; set; }
        public List<string> PadraoNome { get; set; }
        public List<string> PadraoNomeDiario { get; set; }
        public List<string> PadraoNomeFull { get; set; }
        public long TamanhoMaximo { get; set; }
        public string S3Folder { get; set; }
        public bool CargaDiaria { get; set; }
        public string S3FolderDiario { get; set; }
        public string S3FolderFull { get; set; }

    }
}
