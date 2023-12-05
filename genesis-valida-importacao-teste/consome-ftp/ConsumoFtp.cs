using FluentFTP;
using genesis_valida_importacao_teste.EnviaArquivo;
using genesis_valida_importacao_teste.Interfaces;
using genesis_valida_importacao_teste.valida_arquivo;
using Microsoft.Extensions.Options;

namespace genesis_valida_importacao_teste.consome_ftp
{
    public class ConsumoFtp : IConsumidor
    {
        private readonly FtpConfig ftpConfig;
        private readonly ValidadoresOptions validadoresConfig;
        private readonly IEnviaArquivo enviaArquivo;

        public ConsumoFtp(IOptions<FtpConfig> ftpService, IOptions<ValidadoresOptions> validadoresOptions, IEnviaArquivo enviaArquivo)
        {
            ftpConfig = ftpService.Value;
            validadoresConfig = validadoresOptions.Value;
            this.enviaArquivo = enviaArquivo;
        }

        public async void IniciaConsumo()
        {
            var token = new CancellationToken();
            var ftpClient = new AsyncFtpClient(ftpConfig.Host, ftpConfig.User, ftpConfig.Password, ftpConfig.Port);

            await ftpClient.AutoConnect();

            List<Validadores> validadores = validadoresConfig.Validadores.ToList();
            
            foreach (var validador in validadores)
            {
                FtpListItem[] items = await ftpClient.GetListing(validador.Diretorio);

                foreach (var item in items)
                {
                    switch (item.Type)
                    {
                        case FtpObjectType.File:
                            ValidaArquivo.Valida(item.Name, item.Size, validador);
                            byte[] bytes = await ftpClient.DownloadBytes(item.FullName, token);
                            await enviaArquivo.EnviaArquivoFTP(bytes, validador.S3Folder, item.Name);
                            break;

                        case FtpObjectType.Link:
                            break;
                    }
                };
            }
        }
            
    }
}
