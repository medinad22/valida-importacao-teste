using genesis_valida_importacao_teste;
using genesis_valida_importacao_teste.consome_ftp;
using genesis_valida_importacao_teste.EnviaArquivo;
using genesis_valida_importacao_teste.Interfaces;
using genesis_valida_importacao_teste.valida_arquivo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConsumidor, ConsumoFtp>();
// Configuração da seção "Validadores"
builder.Services.Configure<ValidadoresOptions>(options =>
{
    options.Validadores = builder.Configuration.GetRequiredSection("Validadores").Get<List<Validadores>>();
});

builder.Services.Configure<FtpConfig>(
    builder.Configuration.GetSection("FTPServer"));
builder.Services.AddSingleton<IEnviaArquivo, S3EnviaArquivo>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Production")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
