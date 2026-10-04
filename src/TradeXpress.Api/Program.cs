using Microsoft.EntityFrameworkCore;
using TradeXpress.Data;
using MediatR;
using TradeXpress.Business.Auth;
using TradeXpress.Business.Common;
using FluentValidation;
using TradeXpress.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<TradeXpressDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegisterIndividualCommand).Assembly));
builder.Services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<TradeXpressDbContext>());
builder.Services.AddValidatorsFromAssembly(typeof(RegisterIndividualCommand).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

