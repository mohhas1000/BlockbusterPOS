using BlockBusterPOS.Extensions;
using Newtonsoft.Json.Converters;
using BlockBusterPOS.Validators;
using BlockBusterPOS.Dto;
using FluentValidation;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

var configuration = builder.Configuration;

services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
    options.SerializerSettings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ssZ";
});

services.AddOpenApiServices(configuration);
services.RegisterTransactionServices(configuration);
services.AddAutoMapperServices();
services.AddScoped<IValidator<CreateCustomerTransactionDto>, CreateCustomerTransactionDtoValidator>();

var app = builder.Build();

IWebHostEnvironment env = app.Environment;

if (env.IsDevelopment())
{
    app.UseExceptionHandler("/error-development");
}

app.UseOpenApi();
app.UseSwaggerUi();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program
{
}
