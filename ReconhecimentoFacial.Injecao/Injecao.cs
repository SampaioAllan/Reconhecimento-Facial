// dotnet add package EFCore.NamingConventions --version 6.0.0
using Amazon.Rekognition;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReconhecimentoFacial.Application.Services;
using ReconhecimentoFacial.Lib.Data.Repositorios;
using ReconhecimentoFacial.Lib.Data.Repositorios.Interfaces;
using ReconhecimentoFacial.Services;

namespace ReconhecimentoFacial.Injecao
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
            builder.Services.AddScoped<AwsServices>();

            var awsOptions = builder.Configuration.GetAWSOptions();
            awsOptions.Credentials = new EnvironmentVariablesAWSCredentials();
            builder.Services.AddDefaultAWSOptions(awsOptions);

            builder.Services.AddAWSService<IAmazonS3>();
            builder.Services.AddScoped<AmazonRekognitionClient>();


        }
    }
}