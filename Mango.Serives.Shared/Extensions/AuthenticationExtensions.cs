using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;


namespace Mango.Serives.Shared.Extensions
{
    public static class AuthenticationExtensions
    {
        public static WebApplicationBuilder AddJwtAuthentication(this WebApplicationBuilder builder,
            string secretConfigKey = "ApiSettings:JwtOptions:Secret",
            string issuerConfigKey = "ApiSettings:JwtOptions:Issuer",
            string audienceConfigKey = "ApiSettings:JwtOptions:Audience")
        
        
        {

            var secret = builder.Configuration.GetValue<string>(secretConfigKey);
            var issuer = builder.Configuration.GetValue<string>(issuerConfigKey);
            var audience = builder.Configuration.GetValue<string>(audienceConfigKey);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                    };
                });
            return builder;

        }
    }
}
