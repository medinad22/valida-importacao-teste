using System.Text.RegularExpressions;

namespace genesis_valida_importacao_teste.valida_arquivo
{
    public class ValidaArquivo
    {

        public static void Valida(string nome, long tamanhoArquivo, Validadores validador)
        {
            ValidaNome(nome, validador.PadraoNome);
            ValidaTamanho(tamanhoArquivo, validador.TamanhoMaximo);
        }

        private static void ValidaNome(string nome, string pattern)
        { 
            if(!Regex.Match(nome, @$"{pattern}").Success)
            {
                throw new ArgumentException();    
            };
        }

        private static void ValidaTamanho(long tamanhoArquivo, long tamanhoMaximo)
        {
            if(tamanhoArquivo > tamanhoMaximo)
            {
                throw new ArgumentException();
            }
        }
    }
}
