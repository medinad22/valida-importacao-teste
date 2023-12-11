using FluentFTP;
using FluentFTP.Helpers;
using genesis_valida_importacao_teste.EnviaArquivo;
using genesis_valida_importacao_teste.Interfaces;
using genesis_valida_importacao_teste.valida_arquivo;
using Microsoft.Extensions.Options;
using System.IO.Compression;

namespace genesis_valida_importacao_teste.consome_ftp
{
    public class ConsumoFtp(IOptions<FtpConfig> ftpService,
        IOptions<ValidadoresOptions> validadoresOptions,
        IEnviaArquivo enviaArquivo) : IConsumidor
    {
        private readonly FtpConfig ftpConfig = ftpService.Value;
        private readonly ValidadoresOptions validadoresConfig = validadoresOptions.Value;
        private readonly IEnviaArquivo enviaArquivo = enviaArquivo;

        public async void IniciaConsumo()
        {
            var token = new CancellationToken();
            var ftpClient = new AsyncFtpClient(ftpConfig.Host, ftpConfig.User, ftpConfig.Password, ftpConfig.Port);

            await ftpClient.AutoConnect();

            List<Layouts> validadores = [.. validadoresConfig.Validadores];
            foreach (var validador in validadores)
            {
                FtpListItem[] items = await ftpClient.GetListing(validador.Diretorio);

                foreach (var item in items)
                {
                    switch (item.Type)
                    {

                        case FtpObjectType.File:
                            var bufferSize = 419430400;
                            ftpClient.Config.LocalFileBufferSize = bufferSize;
                            ftpClient.Config.DownloadRateLimit = 0;
                            ValidaArquivo.Valida(item.Name, item.Size, validador);
                            Console.WriteLine($"Arquivo {item.Name} iniciando: {DateTime.Now}");
                            byte[] bytes = await ftpClient.DownloadBytes(item.FullName, token);
                            Console.WriteLine($"Arquivo {item.Name} baixado: {DateTime.Now}");
                            await enviaArquivo.EnviaArquivoFTP(bytes, validador, item.Name);
                            Console.WriteLine($"Arquivo {item.Name} enviado: {DateTime.Now}");
                            break;

                        case FtpObjectType.Link:
                            break;
                    }
                };
            }
        }


        public async void IniciaConsumo2()
        {

            var inicio = DateTime.Now;
            List<Layouts> validadores = validadoresConfig.Validadores;
            foreach (var validador in validadores)
            {
                var token = new CancellationToken();
                var ftpClient = new AsyncFtpClient(ftpConfig.Host, ftpConfig.User, ftpConfig.Password, ftpConfig.Port);

                await ftpClient.AutoConnect();

                FtpListItem[] items = await ftpClient.GetListing(validador.Diretorio);

                foreach (var item in items)
                {
                    switch (item.Type)
                    {
                        case FtpObjectType.File:
                            if (item.Name.ContainsCI(".zip"))
                            {
                                await HandleZip(validador, ftpClient, item, token);
                            }
                            else
                            {
                                await HandleFile(validador, ftpClient, item, token);
                            }
                            break;

                        case FtpObjectType.Link:
                            break;
                    }
                };
                ftpClient.Dispose();
            }

            var fim = DateTime.Now;
            Console.WriteLine(fim.Subtract(inicio).ToString());
        }

        private async Task HandleFile(Layouts validador, AsyncFtpClient ftpClient, FtpListItem item, CancellationToken token)
        {
            Console.WriteLine(item.Name);
            ValidaArquivo.Valida(item.Name, item.Size, validador);
            byte[] bytes = await ftpClient.DownloadBytes(item.FullName, token);
            await enviaArquivo.EnviaArquivoFTP(bytes, validador, item.Name);
        }

        private async Task HandleZip(Layouts validador, AsyncFtpClient ftpClient, FtpListItem item, CancellationToken token)
        {
            Console.WriteLine(item.Name);
            ValidaArquivo.Valida(item.Name, item.Size, validador);
            MemoryStream ms = new MemoryStream();
            await ftpClient.DownloadStream(ms, item.FullName);
            ZipArchive zipArchives = new ZipArchive(ms, ZipArchiveMode.Read, true);
            foreach (var item1 in zipArchives.Entries)
            {

                Console.WriteLine(item1.Name);
                var stream = item1.Open();
                var ms2 = new MemoryStream();

                stream.CopyTo(ms2);
                byte[] bytes = ms2.ToArray();
                ms2.Dispose();
                ms2.Close();
                await enviaArquivo.EnviaArquivoFTP(bytes, validador, item1.Name);

            }
        }


        private static byte[] GetBytesFromZipEntry(ZipArchiveEntry entry)
        {
            using var entryStream = entry.Open();
            using MemoryStream memoryStream = new();
            entryStream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        public void IniciaConsumoParalelo()
        {
            List<Layouts> validadores = [.. validadoresConfig.Validadores];
            Parallel.ForEach(validadores, async validador =>
            {
                Console.WriteLine($"****************Inicio {validador.S3Folder}, {DateTime.UtcNow}");
                var token = new CancellationToken();
                var ftpClient = new AsyncFtpClient(ftpConfig.Host, ftpConfig.User, ftpConfig.Password, ftpConfig.Port);

                await ftpClient.AutoConnect();

                FtpListItem[] items = await ftpClient.GetListing(validador.Diretorio);

                foreach (var item in items)
                {
                    switch (item.Type)
                    {
                        case FtpObjectType.File:
                            if (item.Name.ContainsCI(".zip"))
                            {
                                await HandleZip(validador, ftpClient, item, token);
                            }
                            else
                            {
                                await HandleFile(validador, ftpClient, item, token);
                            }
                            break;

                        case FtpObjectType.Link:
                            break;
                    }
                };
                ftpClient.Dispose();

                Console.WriteLine($"****************Fim {validador.S3Folder}, {DateTime.UtcNow}");
            });

            
        }

    }
}
