using AspNetCoreRateLimit;
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
                config.CacheProfiles.Add("3mins", new CacheProfile() { Duration = 180 });
            })
              .AddCustomCsvFormatter()
              .AddXmlDataContractSerializerFormatters()
              .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly)
              .AddNewtonsoftJson();//Without NewtonsoftJson the binding does not work properly

            // Configure Authentication
            builder.Services.ConfigureJWToken(builder.Configuration);
            builder.Services.ConfigureIdentity();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;

            });
            builder.Services.ConfigureApiVersioning();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.ConfigureSwagger();
            //In case to extend this IServiceCollection we need to use IConfiguration as parameter of our extention  method:Here 
            //ConfigureSqlConnection
            builder.Services.ConfigureSqlContext(builder.Configuration);

            //Configure Inversion of Control IoC
            builder.Services.ConfigureRepositoryManager();
            builder.Services.ConfigureServiceManager();
            builder.Services.ConfigureLoggerService();
            builder.Services.ConfigureAuthManager();
            //Action Filters
            builder.Services.ConfigureActionFiltersAttribute();
            //Content Negotiation & Hypdermedia
            builder.Services.AddCustomMediaTypes();
            builder.Services.ConfigureDataShaper();
            builder.Services.AddScoped<IBookLinks,BookLinks>();
            // Caching Process
            builder.Services.ConfigureResponsCaching();
            builder.Services.ConfigureHttpCacheHeaders();
            //Rate Limitation
            builder.Services.AddMemoryCache();
            builder.Services.ConfigureRateLimitingOptions();
            builder.Services.AddHttpContextAccessor();

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
                app.UseSwaggerUI(s =>
                {
                    s.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStore API V1");
                    s.SwaggerEndpoint("/swagger/v2/swagger.json", "BookStore API V2");
                });
            }
            if(app.Environment.IsProduction())
            {
                app.UseHsts();  
            }
            app.UseHttpsRedirection();
            app.UseIpRateLimiting();//This method has to be called before Cors()
            //Here we allow our api's consumers to access it
            app.UseCors(policyName: "CorsPolicy");
            app.UseResponseCaching();//The UseResponseCaching() has to be called just after Cors()
            app.UseHttpCacheHeaders();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
