using codetheory.BL;
using codetheory.BL.Mapping;
using codetheory.BL.Services.Impl;
using codetheory.BL.Services.Interfaces;
using codetheory.DAL;
using codetheory.DAL.Config;
using codetheory.DAL.Models;
using codetheory.DAL.Repositories.Impl;
using codetheory.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CodeTheoryContext>(options => options.UseNpgsql(ConfigManager.ConnectionString));

builder.Services.AddDalServices();
builder.Services.AddBlServices();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
