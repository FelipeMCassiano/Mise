using Microsoft.EntityFrameworkCore;
using Mise.Database;
using Mise.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connString = builder.Configuration.GetConnectionString("Mysql");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connString, ServerVersion.AutoDetect(connString)));


builder.Services.AddScoped<ProductsService>();
builder.Services.AddScoped<TagsService>();
builder.Services.AddScoped<CatalogService>();

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
