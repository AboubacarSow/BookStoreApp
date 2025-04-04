using Entities.DataTransfertObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Presentation.ActionFilters;
using Presentation.Controllers;
using Repositories.Contracts;
using Repositories.EFCore;
using Services.Contracts;
using Services.Models;
using System.Net;

namespace WebApi.Infrastructure.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(options=>
            options.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
           
        }
        public static void ConfigureRepositoryManager(this IServiceCollection services)
        { 
            services.AddScoped<IRepositoryManager, RepositoryManager>();    
        
        }
        public static void ConfigureServiceManager(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
        }
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerService, LoggerManager>();
        }
        public static void ConfigureActionFiltersAttribute(this IServiceCollection services)
        {
            services.AddScoped<ValidationFilterAttribute>();
            services.AddSingleton<LogFilterAttribute>();
            services.AddScoped<ValidationMediaTypeAttribute>();
        }
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .WithExposedHeaders("X-Pagination");
                });
            });
        }
        public static void ConfigureDataShaper(this IServiceCollection services){
            services.AddScoped<IDataShaper<BookDto>, DataShaper<BookDto>>();
        }
        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config=>
            {
                var systemTextJsonOutputFormatter=config
                .OutputFormatters
                .OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();
                systemTextJsonOutputFormatter?.SupportedMediaTypes
                                            .Add("application/vnd.btkakademi.hateoas+json");
                systemTextJsonOutputFormatter?.SupportedMediaTypes
                                            .Add("application/vnd.btkakademi.apiroot+json");
                var systemTextXmlOutputFormatter=config
                       .OutputFormatters
                       .OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();
                systemTextXmlOutputFormatter?.SupportedMediaTypes
                                            .Add("application/vnd.btkakademi.hateoas+xml");
                systemTextXmlOutputFormatter?.SupportedMediaTypes
                                            .Add("application/vnd.btkakademi.apiroot+xml");

            });
        }
        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");
                options.Conventions.Controller<BooksController>()
                                    .HasApiVersion(new ApiVersion(1,0));
                options.Conventions.Controller<BooksV2Controller>()
                                   .HasDeprecatedApiVersion(new ApiVersion(2, 0));
            });
        }
    }
}
