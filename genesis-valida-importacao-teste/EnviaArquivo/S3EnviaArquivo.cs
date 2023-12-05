using Amazon.S3;
using Amazon.S3.Model;

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
    }
}
