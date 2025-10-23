using AutoMapper;
using Movem.Api.DTOs;
using Movem.Api.MappingProfiles;
using Movem.Common.Interfaces;
using Movem.Api.Services;
using Movem.Db.Exrensions;

var builder = WebApplication.CreateBuilder(args);

var loggerFactory = LoggerFactory.Create(logging =>
{
    logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
    logging.AddConsole();
    logging.AddDebug();
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddMovemDb(builder.Configuration);

var mapperConfig = new MapperConfiguration((cfg) =>
{
    //cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
    cfg.AddProfile<DataProfile>();
}, loggerFactory);

mapperConfig.AssertConfigurationIsValid();
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddScoped<IStorageFactoryService<DataDto>, StorageFactoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
