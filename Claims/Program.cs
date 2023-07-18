using System.Text.Json.Serialization;
using Claims;
using FluentValidation;
using Infrastructure;
using Services;
using Services.Audit;
using Services.Claim;
using Services.Cover;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }
);

var cosmosDbConfig = new CosmosDbConfiguration();
builder.Configuration.Bind("CosmosDb", cosmosDbConfig);
var claimRepository = InitializeCosmosClientInstanceAsync<Claim>(
        cosmosDbConfig.DatabaseName, 
        cosmosDbConfig.ContainerName,
        cosmosDbConfig.Account, 
        cosmosDbConfig.Key)
    .GetAwaiter()
    .GetResult();
var coverRepository = InitializeCosmosClientInstanceAsync<Cover>(
        cosmosDbConfig.DatabaseName,
        "Cover",
        cosmosDbConfig.Account, 
        cosmosDbConfig.Key)
    .GetAwaiter()
    .GetResult();

builder.Services.AddSingleton<IClaimService>(new ClaimService(claimRepository));
builder.Services.AddSingleton<ICoverService>(new CoverService(coverRepository));

builder.Services.AddScoped<IAuditRepository, AuditEventGridRepository>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<IValidator<CoverRequestModel>, CoverValidator>();
builder.Services.AddScoped<IValidator<ClaimRequestModel>, ClaimValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    options.EnableAnnotations();
});

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

static async Task<IRepository<T>> InitializeCosmosClientInstanceAsync<T>(string dbName, string containerName, 
    string account, string key)
where T: class, IIdMarker
{
    Microsoft.Azure.Cosmos.CosmosClient client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
    var cosmosDbService = new Repository<T>(client, dbName, containerName);
    Microsoft.Azure.Cosmos.DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(dbName);
    await database.Database.CreateContainerIfNotExistsAsync(containerName, "/id");

    return cosmosDbService;
}

/// <summary>
/// Here so Program can be referenced by Claims.Tests
/// </summary>
public partial class Program { }