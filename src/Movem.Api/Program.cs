using AutoMapper;
using Movem.Api.Managers;
using Movem.Api.MappingProfiles;
using Movem.CacheService;
using Movem.Common.Interfaces;
using Movem.Db.Exrensions;
using Movem.Db.Repositories;
using Movem.FileStorage;

var builder = WebApplication.CreateBuilder(args);

var loggerFactory = LoggerFactory.Create(logging =>
{
    logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
    logging.AddConsole();
    logging.AddDebug();
});

builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.
builder.Services.AddLogging();
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddDistributedRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddMovemDb(builder.Configuration);

var mapperConfig = new MapperConfiguration((cfg) =>
{
    //cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
    cfg.AddProfile<DataProfile>();
}, loggerFactory);

mapperConfig.AssertConfigurationIsValid();
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

//builder.Services.AddScoped<IStorage, FileStorage>();
//builder.Services.AddScoped<IStorage, InMemoryStorage>();
//builder.Services.AddScoped<IStorage, RedisStorage>();
builder.Services.AddKeyedScoped<IStorage, FileRepository>("primary");
builder.Services.AddKeyedScoped<IStorage, InMemoryStorage>("secondary");
builder.Services.AddKeyedScoped<IStorage, FileStorage>("secondary");
builder.Services.AddKeyedScoped<IStorage, RedisStorage>("secondary");
builder.Services.AddScoped<FileStorage>();
builder.Services.AddScoped<InMemoryStorage>();
builder.Services.AddScoped<RedisStorage>();
builder.Services.AddScoped<IStorageManager, StorageManager>();

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
