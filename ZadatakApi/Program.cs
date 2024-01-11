using AutoMapper;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using Services;
using Services.EntityProfiler;
using ServicesContracts;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mapperConfiguration = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(typeof(AppMappingProfile));
});
var mapper = mapperConfiguration.CreateMapper();

//Add services into IoC container
builder.Services.AddScoped<IUserRepository, UsersRepository>();
builder.Services.AddScoped<IContactRepository, ContactsRepository>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddSingleton(mapper);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});



var app = builder.Build();

if(builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}


app.UseRouting();

app.MapControllers();

app.Run();


// making the auto-generatet Program accessible programmatically for testing
public partial class Program { }
