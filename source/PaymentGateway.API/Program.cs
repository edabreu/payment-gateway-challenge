using FluentValidation.AspNetCore;
using PaymentGateway.API.Dtos.Validators;
using PaymentGateway.API.Services;

var builder = WebApplication.CreateBuilder(
    new WebApplicationOptions
    {
        Args = args,
        ContentRootPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
    });
// Add services to the container.

builder.Services.AddControllers()
    .AddFluentValidation(
        config =>
        {
            config.RegisterValidatorsFromAssemblyContaining<PaymentRequestValidator>();
            config.ImplicitlyValidateChildProperties = true;
        });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPaymentService, PaymentService>()
    .AddGateways(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

