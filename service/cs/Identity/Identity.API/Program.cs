﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

string API_VERSION = builder.Configuration["ApiVersion"];

string B2C_INSTANCE = builder.Configuration["AzureAdB2C:Instance"];
string B2C_DOMAIN = builder.Configuration["AzureAdB2C:Domain"];
string B2C_SIGNUP_AND_SIGNIN_POLICY_ID = builder.Configuration["AzureAdB2C:SignUpSignInPolicyId"];

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
                AuthorizationUrl = new Uri($"{B2C_INSTANCE}/{B2C_DOMAIN}/{B2C_SIGNUP_AND_SIGNIN_POLICY_ID}/oauth2/v2.0/authorize"),
                TokenUrl = new Uri($"{B2C_INSTANCE}/{B2C_DOMAIN}/{B2C_SIGNUP_AND_SIGNIN_POLICY_ID}/oauth2/v2.0/token")
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