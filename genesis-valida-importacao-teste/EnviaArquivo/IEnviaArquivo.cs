using FluentFTP;
using genesis_valida_importacao_teste.valida_arquivo;

namespace genesis_valida_importacao_teste.EnviaArquivo
{
    public interface IEnviaArquivo
    {
        Task EnviaArquivoFTP(byte[] file, Layouts validadores, string fileName);
    }
}
