﻿using Domain.Utils.Constants;
using Domain.Utils.Helpers;
using Infra.CrossCutting.Security;
using Infra.Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presentation.Web.Filters;
using Presentation.Web.NativeInjector;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

#region Routes

builder.Services.AddEndpointsApiExplorer();

#endregion

#region Action Filter

builder.Services.AddMvc(opts =>
{
    opts.Filters.Add<ActionFilter>();
});

#endregion

#region Swagger

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Estuda Fácil API", Version = "V1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = @"JWT Authorization header using the Bearer scheme.
            \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.
        \r\n\r\nExample: Bearer 12345abcdef",
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                } ,
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            Array.Empty<string>()
        }
    });
});

#endregion

#region AutoMapper

builder.Services.AddAutoMapper(typeof(Application.AutoMapper.AutoMapper));

#endregion

#region Logging

//Log.Logger = new LoggerConfiguration()
//    .WriteTo.File("Logs/MainLog.txt", rollingInterval: RollingInterval.Day)
//    .CreateLogger();

#endregion

#region Necessary Properties

var isDevelopment = builder.Environment.IsDevelopment();
var encryptionKey = isDevelopment ? Pwd.DevEncryptionKey : Pwd.PrdEncryptionKey;

var encryptionService = new EncryptionService();

#endregion

#region Mysql Connection

var mysqlConnection = builder.Configuration.GetConnectionString("MainDb").ToSafeValue();
mysqlConnection = encryptionService.DecryptDynamic(mysqlConnection, encryptionKey);

var serverVersion = builder.Configuration.GetSection("ServerVersion").Get<string>()!;

builder.Services.AddDbContext<SqlContext>(opt => opt.UseMySql(
    mysqlConnection, ServerVersion.Parse(serverVersion)));

#endregion

#region JWT Authentication

var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>()!;
TokenInfo.JwtKey = encryptionService.DecryptDynamic(jwtKey, encryptionKey);

var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>()!;
TokenInfo.JwtIssuer = encryptionService.DecryptDynamic(jwtIssuer, encryptionKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = TokenInfo.JwtIssuer,
         ValidAudience = TokenInfo.JwtIssuer,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenInfo.JwtKey))
     };
 });

#endregion

#region Rate Limit

builder.Services.AddRateLimiter(opt =>
{
    opt.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    opt.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(http =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: http.User.Identity?.Name ??
                          http.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 50,
                QueueLimit = 5,
                Window = TimeSpan.FromMinutes(1)
            }));

    opt.OnRejected = async (context, token) =>
     {
         context.HttpContext.Response.StatusCode = 429;

         await context.HttpContext.Response.WriteAsync("Limite de requisições por minuto atingido", cancellationToken: token);
     };
});

#endregion

#region Dependency Injector

NativeInjector.RegisterServices(builder.Services);

#endregion

#region DataProtectionKeys

if (!isDevelopment)
    builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"/root/.aspnet/DataProtection-Keys"))
        .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
        {
            EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
            ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
        });

#endregion

var app = builder.Build();

# region Docker environment variables

if (isDevelopment)
    builder.Configuration.AddEnvironmentVariables();

#endregion

#region Database Creation

IApplicationBuilder applicationBuilder = app;

using var serviceScope = applicationBuilder.ApplicationServices.CreateAsyncScope();

var configContext = serviceScope.ServiceProvider.GetService<SqlContext>()
    ?? throw new InvalidOperationException("NoContextFound");

await configContext.Database.EnsureCreatedAsync();

#endregion

#region Swagger

if (isDevelopment)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#endregion

#region Endpoint 

app.UseHttpsRedirection();

#endregion

#region Authentication

app.UseAuthentication();
app.UseAuthorization();

#endregion

#region Rate Limit

app.UseRateLimiter();

#endregion

app.MapControllers();

app.Run();

