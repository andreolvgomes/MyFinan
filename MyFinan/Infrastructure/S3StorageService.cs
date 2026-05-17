using Amazon.S3.Model;
using Amazon.S3;

namespace MyFinan.Infrastructure
{
    public class S3StorageService
    {
        private readonly IAmazonS3 _s3Client;

        public S3StorageService(IAmazonS3 s3Client)
        {
            _s3Client = s3Client ?? throw new ArgumentNullException(nameof(s3Client));
        }

        public async Task<MemoryStream> ReadFile(string nomeBucket, string nomeArquivoKey)
        {
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = nomeBucket,
                    Key = nomeArquivoKey
                };

                using (GetObjectResponse response = await _s3Client.GetObjectAsync(request))
                {
                    var memoryStream = new MemoryStream();

                    await response.ResponseStream.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    return memoryStream;
                }
            }
            catch (AmazonS3Exception e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}