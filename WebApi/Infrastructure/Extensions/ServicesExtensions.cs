using AspNetCoreRateLimit;
using Entities.DataTransfertObjects.BookDtos;
using Entities.Models;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presentation.ActionFilters;
using Presentation.Controllers;
using Repositories.Contracts;
using Repositories.EFCore;
using Services.Contracts;
using Services.Models;
using System.Text;


namespace WebApi.Infrastructure.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(options =>
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
        public static void ConfigureDataShaper(this IServiceCollection services) {
            services.AddScoped<IDataShaper<BookDto>, DataShaper<BookDto>>();
        }
        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var systemTextJsonOutputFormatter = config
                .OutputFormatters
                .OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();
                systemTextJsonOutputFormatter?.SupportedMediaTypes
                                            .Add("application/vnd.btkakademi.hateoas+json");
                systemTextJsonOutputFormatter?.SupportedMediaTypes
                                            .Add("application/vnd.btkakademi.apiroot+json");
                var systemTextXmlOutputFormatter = config
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
                                    .HasApiVersion(new ApiVersion(1, 0));
                options.Conventions.Controller<BooksV2ioController>()
                                   .HasDeprecatedApiVersion(new ApiVersion(2, 0));
            });
            
        }
        public static void ConfigureResponseCaching(this IServiceCollection services) =>
            services.AddResponseCaching();
        public static void ConfigureHttpCacheHeaders(this IServiceCollection services) =>
            services.AddHttpCacheHeaders(expressionOptions =>
            {
                expressionOptions.CacheLocation = CacheLocation.Public;
                expressionOptions.MaxAge = 80;
            },
            validationOptions =>
            {
                validationOptions.MustRevalidate = false;// What this does?
            });
        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>()
            {
                new(){
                    Endpoint="*",
                    Limit=25,
                    Period="1m"
                }
            };
            services.Configure<IpRateLimitOptions>(options =>
            {
                options.GeneralRules = rateLimitRules;
            });
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

        }
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;//What does it mean?
                options.Password.RequireLowercase = false;

                options.User.RequireUniqueEmail = true;
              
               
                
            })
            .AddEntityFrameworkStores<RepositoryContext>()
            .AddDefaultTokenProviders();//For authentication via JWT
        }

        public static void ConfigureAuthManager(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationManager>();
        }

        public static void ConfigureJWToken(this IServiceCollection services,IConfiguration configuration)
        {
            var jwtsettings = configuration.GetSection("JwtSettings");
            string secretKey = jwtsettings["secretKey"]!;
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtsettings["validIssuer"],
                    ValidAudience = jwtsettings["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(secretKey))
                };
            });
            services.AddAuthorization();
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo() { Title = "BookStore API", Version = "v1" });
                s.SwaggerDoc("v2", new OpenApiInfo() { Title = "BookStore API", Version = "v2" });

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Place where to add JWT token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat="JWT"
                });
                s.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference=new OpenApiReference()
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            },
                            Name="Authorization",
                            Scheme="bearer",
                            In=ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });
        }
    }
}
