using Amazon.Runtime;
using Amazon.S3;
using Amazon.Rekognition;
using Microsoft.EntityFrameworkCore;
using ReconhecimentoFacial.Lib.Data.Repositorios;
using ReconhecimentoFacial.Lib.Data.Repositorios.Interfaces;
using ReconhecimentoFacial.Application.Services;

var builder = WebApplication.CreateBuilder(args);

var injecao = new Injecao();
injecao.AdicionarServicos(builder);

builder.Services.AddControllers()
                            .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling =
                            Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
