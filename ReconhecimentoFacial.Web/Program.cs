using ReconhecimentoFacial.Injecao;
using ReconhecimentoFacial.Web.Middlewares;

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
app.UseMiddleware<MiddlewareReconhecimento>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
