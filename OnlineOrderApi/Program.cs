using Microsoft.EntityFrameworkCore;
using OnlineOrderApi.Data;
using OnlineOrderApi;
using System.Text.Json.Serialization;
using OnlineOrderApi.Repository;
using OnlineOrderApi.Repository.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDataContext>(options =>
{
  options.UseSqlite("Data Source=OnlineOrder.db");
  //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
//The NuGet package AutoMapper.Extensions.Microsoft.DependencyInjection will add AutoMapper package too
builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddCors(options => options.AddPolicy(name: "OnlineOrderOrigins",
    policy =>
    {
      //policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
      policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    }));

var app = builder.Build();
// this seeding is only for the template to bootstrap the DB and users.
// in production you will likely want a different approach.
using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;
  DataSeeder.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors("OnlineOrderOrigins");


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
