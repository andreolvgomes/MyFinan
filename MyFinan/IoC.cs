using Amazon;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using MyFinan.Infrastructure;
using MyFinan.Parsers;
using MyFinan.Repositories;
using MyFinan.Services;
using Npgsql;
using System.Data;

namespace MyFinan
{
    public static class IoC
    {
        public static void AddConfigs(this IServiceCollection services)
        {
            //SqlMapper.AddTypeHandler(new NullableDateTimeTypeHandler());

            DotNetEnv.Env.TraversePath().Load();

            services.AddScoped<ICategoriasRepository, CategoriasRepository>();
            services.AddScoped<ITransacoesRepository, TransacoesRepository>();
            services.AddScoped<S3StorageService>();
            services.AddScoped<FaturasInterParser>();
            services.AddScoped<FaturasService>();

            var cnnStr = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            services.AddSingleton<IDbConnection>(new NpgsqlConnection(cnnStr));

#if DEBUG
            var chain = new CredentialProfileStoreChain();
            if (chain.TryGetAWSCredentials("taskhere", out var credentials))
            {
                var s3Config = new AmazonS3Config
                {
                    RegionEndpoint = RegionEndpoint.SAEast1 // Altere para a região correta do seu S3 (ex: USEast1, SAEast1, etc.)
                };
                var s3 = new AmazonS3Client(credentials, s3Config);
                services.AddSingleton<IAmazonS3>(s3);
            }
#else
            services.AddSingleton<IAmazonS3, AmazonS3Client>();
#endif
        }
    }
}