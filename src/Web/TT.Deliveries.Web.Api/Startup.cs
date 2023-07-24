using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TT.Deliveries.DataAccess.EF;
using TT.Deliveries.Domain.Contracts;
using TT.Deliveries.DataAccess.EF.Repositories;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TT.Deliveries.DataAccess.EF.SeedData;
using Serilog;
using TT.Deliveries.Web.Api.Services;
using TT.Deliveries.Web.Api.HostedServices;
using Microsoft.EntityFrameworkCore;

namespace TT.Deliveries.Web.Api;

public class Startup
{
    #region Ctor
    public Startup(IWebHostEnvironment env)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        this.Configuration = builder.Build();

        // Setup Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(path: "./logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
    #endregion

    public IConfiguration Configuration { get; }

    #region Public Methods
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();

        SetupSwagger(services);
        SetupAuth(services);

        var path = Path.Join(AppContext.BaseDirectory, "db");
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        var dbPath = Path.Join(path, "deliveries.db");

        // Transient used here for ability to use in DeliveryExpirationHostedService
        services.AddDbContext<DeliveriesContext>(
            opt => opt.UseSqlite($"Data Source={dbPath}"),
            ServiceLifetime.Transient, ServiceLifetime.Transient);

        // Add services to the container.
        // Transient used here for ability to use in DeliveryExpirationHostedService
        services.AddTransient<IDeliveryRepository, DeliveryRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IRecipientRepository, RecipientRepository>();
        services.AddScoped<DeliveriesStateService, DeliveriesStateService>();

        // Register Hosted services
        services.AddHostedService<DeliveryExpirationHostedService>();

        // Register AutoMapper
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // Seed data for demonstrating purposes
        var serviceProvider = services.BuildServiceProvider();
        DeliveriesContextSeedData.Seed(serviceProvider);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // Enable Swagger for both dev and prod for demo purposes
        app.UseSwagger();
        app.UseSwaggerUI(opt => {
            opt.SwaggerEndpoint("/swagger/v1/swagger.json", "TT.Deliveries.Web.Api v1");
        });

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(e => e.MapControllers());        
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Setup Swagger
    /// </summary>
    /// <param name="services"></param>
    private void SetupSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "TT.Deliveries.Web.Api", Version = "v1" });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new string[] {}
                }
            });
            
            // Expose xml comments to Swagger
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            opt.IncludeXmlComments(xmlPath);
        });
    }

    /// <summary>
    /// Setup auth
    /// </summary>
    /// <param name="services"></param>
    private void SetupAuth(IServiceCollection services)
    {
        services.AddAuthentication(opt => {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(opt =>
            {
                opt.IncludeErrorDetails = true;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("DeliveriesSecurityKey"))
                };
            });
    }
    #endregion
}
