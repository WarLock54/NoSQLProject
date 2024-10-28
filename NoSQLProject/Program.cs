using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Neo4jClient;
using NoSQLProject.DataAccess;
using NoSQLProject.Repository;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//mongo connection
var settings = MongoClientSettings.FromConnectionString(builder.Configuration.GetConnectionString("MongoDb"));
settings.ServerApi = new ServerApi(ServerApiVersion.V1);
var client = new MongoClient(settings);
//
//neo4j
var clientNeo = new BoltGraphClient(new Uri("bolt+s://4f93cdbe.databases.neo4j.io:7687"),"neo4j","root");
clientNeo.ConnectAsync();
builder.Services.AddSingleton<IBoltGraphClient>(clientNeo);
//
//builder.Services.AddScoped<INeo4jDataAccess, Neo4jDataAccess>();
builder.Services.AddSingleton<IMongoClient>(client);
//
builder.Services.AddTransient<INeoV2Repository, NeoV2Repository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<INeoRepository, NeoRepository>();

var app = builder.Build();

// This is the registration for your domain repository class


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();

