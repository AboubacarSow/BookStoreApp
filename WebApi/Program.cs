using Microsoft.AspNetCore.Mvc;
using NLog;
using Presentation.ActionFilters;
using Services.Contracts;
using Services.Models;
using WebApi.Infrastructure.Extensions;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            LogManager.Setup().LoadConfigurationFromFile(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            // Add services to the container.

            builder.Services.AddControllers(config =>
            {
                //Content Negociation Part
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
            })
              .AddCustomCsvFormatter()
              .AddXmlDataContractSerializerFormatters()
              .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
              


            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;

            });
            builder.Services.ConfigureApiVersioning();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //In case to extend this IServiceCollection we need to use IConfiguration as parameter of our extention  method:Here 
            //ConfigureSqlConnection
            builder.Services.ConfigureSqlContext(builder.Configuration);
            builder.Services.ConfigureRepositoryManager();
            builder.Services.ConfigureServiceManager();
            builder.Services.ConfigureLoggerService();
            builder.Services.AddCustomMediaTypes();
            builder.Services.ConfigureActionFiltersAttribute();
            builder.Services.ConfigureDataShaper();
            builder.Services.AddScoped<IBookLinks,BookLinks>();

            //We configure our api policy here by expoxing the X-Pagination header as well
            builder.Services.ConfigureCors();
            //builder.Services.AddAutoMapper(typeof(Services.AssemblyReference).Assembly);
            builder.Services.AddAutoMapper(typeof(Program));
           
            
             
            var app = builder.Build();

            var logger = app.Services.GetRequiredService<ILoggerService>();

            app.ConfigureExceptionHandler(logger);
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            if(app.Environment.IsProduction())
            {
                app.UseHsts();  
            }
            app.UseHttpsRedirection();
            //Here we allow our api's consumers to access it
            app.UseCors(policyName: "CorsPolicy");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
