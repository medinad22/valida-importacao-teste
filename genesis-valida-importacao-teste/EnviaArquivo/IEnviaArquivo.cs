using FluentFTP;

namespace genesis_valida_importacao_teste.EnviaArquivo
{
    public interface IEnviaArquivo
    {
        Task EnviaArquivoFTP(byte[] file, string filePath, string fileName);
    }
}
