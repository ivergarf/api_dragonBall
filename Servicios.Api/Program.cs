using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using Servicios.Api.Helpers;
using Servicios.Core.Interfaces;
using Servicios.Core.Services;
using Servicios.Infrastructure.Filters;
using Servicios.Infrastructure.Repositories;
using System.Text;

//Se inicializa el proveedor NLog y se lee la configuración
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().
GetCurrentClassLogger();
logger.Debug("init program");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Automapper
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // Referencias Circulares y Filtro de excepciones globales
    builder.Services.AddControllers(options =>
    {
        // Filto de excepciones globales
        options.Filters.Add<GlobalExceptionFilter>();
    }).AddNewtonsoftJson(options =>
    {
        // Eliminar referencias circulares
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.ContractResolver = null;
    });

    // config swagger
    builder.Services.AddSwaggerGen(c =>
    {
        c.EnableAnnotations();
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Api_DragonBall",
            Version = "v1",
            Description = "Api para consultar personajes y programar batallas DragonBall"
        });

        // config to show auth in swagger
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme."

        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });


    });

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //builder.Services.AddTransient<ITokenRepository, TokenRepository>();
    builder.Services.AddSingleton(new TokenService(builder.Configuration["Jwt:Key"], builder.Configuration["Jwt:Issuer"]));
    builder.Services.AddScoped<IEncryptHelper, EncryptHelper>(); // to use encrypt services
    builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();

    // Inyección de dependencia de Helpers
    builder.Services.AddScoped<IEncryptHelper, EncryptHelper>(); // to use encrypt servies
    builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
    builder.Services.AddControllers().AddXmlDataContractSerializerFormatters();


    builder.Services.AddTransient<IListarBatallasDragonBallRepository, ListarBatallasDragonBallRepository>();
    builder.Services.AddTransient<IListarBatallasDragonBallService, ListarBatallasDragonBallService>();



    //Configuración de autenticación JWT
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

    //Se remplaza el log nativo por NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    // Autenticación 
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();

}
catch (Exception ex)
{

    logger.Error(ex, "El programa se ha detenido debido a una excepción");
    throw;

}

finally
{
    //Apagamos Nlog cuando se apague la aplicación
    NLog.LogManager.Shutdown();
}
