using INDT.Viagens.Application.CommandHandler.ResultadoMelhorRotaCommand;
using INDT.Viagens.Application.CommandHandler.ViagemCommand;
using INDT.Viagens.Application.Interfaces;
using INDT.Viagens.Infra;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); 
    cfg.RegisterServicesFromAssembly(typeof(ViagemCommand).Assembly); 
    cfg.RegisterServicesFromAssembly(typeof(ResultadoMelhorRotaCommand).Assembly); 
});

builder.Services.AddTransient<IRotaRepository, Service>(); 

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
