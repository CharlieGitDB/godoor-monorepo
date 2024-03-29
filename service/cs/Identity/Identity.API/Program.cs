﻿using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Identity.Data;
using Identity.API.Configurations;
using Identity.Data.Repositories;
using Identity.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using AutoWrapper;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authorization;
using Identity.API.Filters;
using Identity.API.Models.Request;

var builder = WebApplication.CreateBuilder(args);

string API_VERSION = builder.Configuration["ApiVersion"];

AzureAdB2CSection azureB2CSection = builder.Configuration.GetSection("AzureAdB2C").Get<AzureAdB2CSection>();
CosmosDbSection cosmosDbSection =
    builder.Configuration.GetSection("CosmosDB").Get<CosmosDbSection>();
ApiConnectorRequirement apiConnectorSection = builder.Configuration.GetSection("ApiConnector").Get<ApiConnectorRequirement>();

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

builder.Services.AddControllers()
    .AddNewtonsoftJson();
    
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true; // TODO: revisit this, we may not want this
    options.DefaultApiVersion = new ApiVersion(Int32.Parse(API_VERSION), 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new HeaderApiVersionReader("X-Api-Version");
});

builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    options.UseCosmos(
        accountEndpoint: cosmosDbSection.EndPointUri,
        accountKey: cosmosDbSection.PrimaryKey,
        databaseName: cosmosDbSection.DbName
    );
});

builder.Services.Configure<ApiBehaviorOptions>(o =>
{
    o.SuppressModelStateInvalidFilter = true;
});

//repos
builder.Services.AddTransient<IUserRepository, UserRepository>();

//auth handlers
builder.Services.AddSingleton<IAuthorizationHandler, ApiConnectorHandler>();

//policies
builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("ApiConnectorBasicAuth", policy => policy.Requirements.Add(apiConnectorSection as ApiConnectorRequirement));
});

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

builder.Services.AddProblemDetails(o =>
{
    o.IncludeExceptionDetails = (ctx, env) => builder.Environment.IsDevelopment() || builder.Environment.IsProduction();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", $"Godoor Identity v{API_VERSION}");
        o.OAuthClientId(LOGIN_CLIENT_ID);
        o.OAuthUsePkce();
        o.OAuthScopeSeparator(" ");
    });
}

app.UseProblemDetails();
app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions { UseApiProblemDetailsException = true }); 

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();