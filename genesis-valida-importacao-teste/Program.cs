using genesis_valida_importacao_teste.consome_ftp;
using genesis_valida_importacao_teste.EnviaArquivo;
using genesis_valida_importacao_teste.Exceptions;
using genesis_valida_importacao_teste.Exceptions.ValidationModel;
using genesis_valida_importacao_teste.Interfaces;
using genesis_valida_importacao_teste.valida_arquivo;
using System.Net.Mime;

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
    options.Validadores = builder.Configuration.GetRequiredSection("Layouts").Get<List<Layouts>>();
});

builder.Services.Configure<FtpConfig>(
    builder.Configuration.GetSection("FTPServer"));
builder.Services.Configure<S3Config>(
    builder.Configuration.GetSection("S3Config"));
builder.Services.AddSingleton<IEnviaArquivo, S3EnviaArquivo>();

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var result = new ValidationFailedResult(context.ModelState);
        result.ContentTypes.Add(MediaTypeNames.Application.Json);
        result.ContentTypes.Add(MediaTypeNames.Application.Xml);

        return result;
    };
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new CustomExceptionFilter());
}
);
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
