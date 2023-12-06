using Amazon.S3;
using Amazon.S3.Model;
using genesis_valida_importacao_teste.valida_arquivo;
using System.Text.RegularExpressions;

namespace genesis_valida_importacao_teste.EnviaArquivo
{
    public class S3EnviaArquivo : IEnviaArquivo
    {
        public async Task EnviaArquivoFTP(byte[] file, string folder, string fileName)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(file, 0, file.Length);
            
            StreamReader sr = new StreamReader(ms);
            

            var config = new AmazonS3Config()
            {
                ServiceURL = "http://s3.localhost.localstack.cloud:4566/"
            };
            var s3client = new AmazonS3Client(config);
            var objectRequest = new PutObjectRequest()
            {
                BucketName = "maxpar-apolicies-transient",
                Key = $"{folder}/{fileName}",
                InputStream = sr.BaseStream,
                
            };

            Console.WriteLine(objectRequest.Key);
            await s3client.PutObjectAsync(objectRequest);
            await ms.FlushAsync();
            ms.Close();
            sr.Close();
        }


        public async Task EnviaArquivoFTP(byte[] file, Validadores validadores, string fileName)
        {

            var folder = CriaPasta(validadores, fileName);
            Console.WriteLine($"{folder}");
            MemoryStream ms = new MemoryStream();
            ms.Write(file, 0, file.Length);

            StreamReader sr = new StreamReader(ms);


            var config = new AmazonS3Config()
            {
                ServiceURL = "http://s3.localhost.localstack.cloud:4566/"
            };
            var s3client = new AmazonS3Client(config);
            var objectRequest = new PutObjectRequest()
            {
                BucketName = "maxpar-apolicies-transient",
                Key = $"{folder}/{fileName}",
                InputStream = sr.BaseStream,

            };

            Console.WriteLine(objectRequest.Key);
            await s3client.PutObjectAsync(objectRequest);
            await ms.FlushAsync();
            ms.Close();
            sr.Close();
        }

        private string CriaPasta(Validadores validadores, string fileName)
        {
            if (!validadores.CargaDiaria)
            {
                return validadores.S3Folder;
            }
            if(validadores.PadraoNomeDiario.Any(pattern => Regex.IsMatch(fileName, pattern)))
            {
                return validadores.S3FolderDiario;
            }
            if (validadores.PadraoNomeFull.Any(pattern => Regex.IsMatch(fileName, pattern)))
            {
                return validadores.S3FolderFull;
            }


            return validadores.S3Folder;

        }
    }
}
