using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Identity.Data;
using Identity.API.Configurations;
using Identity.API.Models.Request;
using Identity.Data.Repositories;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

var builder = WebApplication.CreateBuilder(args);

string API_VERSION = builder.Configuration["ApiVersion"];

AzureAdB2CSection azureB2CSection = builder.Configuration.GetSection("AzureAdB2C").Get<AzureAdB2CSection>();
LocalMSSqlSection localMsSqlSection = builder.Configuration.GetSection("LocalMSSql").Get<LocalMSSqlSection>();

//login access config vars
string API_PERMISSION = builder.Configuration["Scope"];
//the client id is currently for the FE app, possibly create a new app registration for the swagger client
string LOGIN_CLIENT_ID = builder.Configuration["ClientId"];

// Add services to the container.

// Adds Microsoft Identity platform (Azure AD B2C) support to protect this Api
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(options =>
        {
            builder.Configuration.Bind("AzureAdB2C", options);

            options.TokenValidationParameters.NameClaimType = "name";
        },
        options => { builder.Configuration.Bind("AzureAdB2C", options); });
// End of the Microsoft Identity platform block

builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(Int32.Parse(API_VERSION), 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new HeaderApiVersionReader("X-Api-Version");
});

builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    options.UseSqlServer(
            $"Server={localMsSqlSection.Server};User Id={localMsSqlSection.Username};Password={localMsSqlSection.Password};Database={localMsSqlSection.DatabaseName}"
        );
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

//repos
builder.Services.AddTransient<IUserRepository, UserRepository>();

//validation
builder.Services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc($"v{API_VERSION}", new OpenApiInfo
    {
        Title = "Godoor Identity",
        Version = $"v{API_VERSION}"
    });
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "OAuth2.0 Auth code with PCKE",
        Name = "oauth2",
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows()
        {
            AuthorizationCode = new OpenApiOAuthFlow()
            {
                Scopes = new Dictionary<string, string>
                {
                    { API_PERMISSION, "Access the identity API" }
                },
                AuthorizationUrl = new Uri($"{azureB2CSection.Instance}/{azureB2CSection.Domain}/{azureB2CSection.SignUpSignInPolicyId}/oauth2/v2.0/authorize"),
                TokenUrl = new Uri($"{azureB2CSection.Instance}/{azureB2CSection.Domain}/{azureB2CSection.SignUpSignInPolicyId}/oauth2/v2.0/token")
            }
        }
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme, Id = "oauth2"
                }
            },
            new[]
            {
                API_PERMISSION
            }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", $"Godoor Identity v{API_VERSION}");
        c.OAuthClientId(LOGIN_CLIENT_ID);
        c.OAuthUsePkce();
        c.OAuthScopeSeparator(" ");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();