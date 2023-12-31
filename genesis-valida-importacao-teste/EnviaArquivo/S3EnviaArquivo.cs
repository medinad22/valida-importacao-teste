﻿using Amazon.S3;
using Amazon.S3.Model;
using genesis_valida_importacao_teste.valida_arquivo;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace genesis_valida_importacao_teste.EnviaArquivo
{
    public class S3EnviaArquivo(IOptions<S3Config> s3Config) : IEnviaArquivo
    {

        private readonly S3Config s3Config = s3Config.Value;

        public async Task EnviaArquivoFTP(byte[] file, Layouts validadores, string fileName)
        {
            var config = new AmazonS3Config()
            {
                ServiceURL = s3Config.ServiceUrl,

            };
            var s3client = new AmazonS3Client(config);
            using (MemoryStream ms = new MemoryStream(file))
            using (StreamReader sr = new StreamReader(ms))
            {
                var objectRequest = new PutObjectRequest
                {
                    BucketName = s3Config.ApolicesBucketName,
                    Key = $"{GetS3Folder(validadores, fileName)}/{fileName}",
                    InputStream = sr.BaseStream
                };

                await s3client.PutObjectAsync(objectRequest);
            }

        }

        private static string GetS3Folder(Layouts validadores, string fileName)
        {
            if (!validadores.CargaDiaria)
            {
                return validadores.S3Folder;
            }
            if (validadores.PadraoNomeDiario.Any(pattern => Regex.IsMatch(fileName, pattern)))
            {
                return validadores.S3FolderDiario;
            }
            if (validadores.PadraoNomeFull.Any(pattern => Regex.IsMatch(fileName, pattern)))
            {
                return validadores.S3FolderFull;
            }

            return validadores.S3Folder;

        }

        public void EnviaArquivoFTP2(byte[] file, Layouts validadores, string fileName)
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("myaccesskey", "mysecretkey");
            var config = new AmazonS3Config()
            {
                ServiceURL = s3Config.ServiceUrl,

            };
            var s3client = new AmazonS3Client(awsCredentials, config);
            using (MemoryStream ms = new MemoryStream(file))
            using (StreamReader sr = new StreamReader(ms))
            {
                var objectRequest = new PutObjectRequest
                {
                    BucketName = s3Config.ApolicesBucketName,
                    Key = $"{GetS3Folder(validadores, fileName)}/{fileName}",
                    InputStream = sr.BaseStream
                };

                s3client.PutObjectAsync(objectRequest).Wait();
            }
        }
    }
}
