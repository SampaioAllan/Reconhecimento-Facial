// dotnet add package EFCore.NamingConventions --version 6.0.0
using Amazon.Rekognition;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReconhecimentoFacial.Lib.Data.Repositorios;
using ReconhecimentoFacial.Lib.Data.Repositorios.Interfaces;

namespace ReconhecimentoFacial.Application.Services
{
    public class Injecao
    {
        public void AdicionarServicos(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ReconhecimentoFacialContext>(
            conn => conn.UseNpgsql(builder.Configuration.GetConnectionString("ReconhecimentoFacialDB"))
                    .UseSnakeCaseNamingConvention()
                    );            

            builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            builder.Services.AddScoped<IUsuarioApplication, UsuarioApplication>();

            var awsOptions = builder.Configuration.GetAWSOptions();
            awsOptions.Credentials = new EnvironmentVariablesAWSCredentials();
            builder.Services.AddDefaultAWSOptions(awsOptions);

            builder.Services.AddAWSService<IAmazonS3>();
            builder.Services.AddScoped<AmazonRekognitionClient>();


        }
    }
}