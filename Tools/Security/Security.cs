using System.Text; 
using System.Reflection; 
using System.IdentityModel.Tokens.Jwt;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens; 
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace coer91.Tools
{
    public class Security
    {
        public static string ProjectName { get; private set; } = Assembly.GetEntryAssembly()?.GetName()?.Name;

        private readonly IServiceCollection _services;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _envirotment;

        public Security(
            IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment envirotment
        )
        {
            _services = services;
            _configuration = configuration;
            _envirotment = envirotment;
        }


        public void AddSwaggerConfiguration(string title = "", string version = "")
        {
            if (string.IsNullOrWhiteSpace(title))
                title = ProjectName;

            else
                ProjectName = title;

            if (string.IsNullOrWhiteSpace(version))
                version = _configuration.GetSection("Version").Get<string>() ?? string.Empty; 

            if (_envirotment.IsDevelopment())
                title += " - Development";

            else if (_envirotment.IsStaging())
                title += " - Staging";

            _services.AddSwaggerGen(config =>
            { 
                config.SwaggerDoc("v1", new OpenApiInfo { Title = title, Version = version });
                config.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });

                config.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        }, Array.Empty<string>()
                    }
                });
            });
        }


        public void AddBearerConfiguration(string secretKey = null)
        {
            secretKey ??= _configuration["Security:SecretKey"];

            if (string.IsNullOrEmpty(secretKey))
                return;

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            _services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero
                });

            _services.AddControllers(config =>
            {
                AuthorizationPolicy policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                AuthorizeFilter filter = new(policy);
                config.Filters.Add(filter);
            });
        }


        public void AddCorsConfiguration(string policy = "coer91Policy", CorsPolicyBuilder policyBuilder = null)
        {
            _services.AddCors(options =>
            {
                var builder = policyBuilder ?? new CorsPolicyBuilder()
                   .SetIsOriginAllowed(origin => true)
                   .AllowCredentials()
                   .AllowAnyMethod()
                   .AllowAnyHeader();

                options.AddPolicy(policy, builder.Build());
            });
        } 


        public static string GeneratePassword()
        {
            const int length = 4;
            string LOWER = "abcdefghijklmnopqrstuvwxyz";
            string UPPER = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string NUMBERS = "1234567890";
            string SPECIALS = "@$#_!=*";

            char randomChar;
            Random random = new();
            StringBuilder passwordBuilder = new();

            for (int i = 0; i < length; i++)
            {
                randomChar = LOWER[random.Next(LOWER.Length)];
                passwordBuilder.Append(randomChar);
                LOWER = LOWER.Replace(randomChar.ToString(), "");
            }

            for (int i = 0; i < length; i++)
            {
                randomChar = UPPER[random.Next(UPPER.Length)];
                passwordBuilder.Append(randomChar);
                UPPER = UPPER.Replace(randomChar.ToString(), "");
            }

            for (int i = 0; i < length; i++)
            {
                randomChar = NUMBERS[random.Next(NUMBERS.Length)];
                passwordBuilder.Append(randomChar);
                NUMBERS = NUMBERS.Replace(randomChar.ToString(), "");
            }

            for (int i = 0; i < length; i++)
            {
                randomChar = SPECIALS[random.Next(SPECIALS.Length)];
                passwordBuilder.Append(randomChar);
                SPECIALS = SPECIALS.Replace(randomChar.ToString(), "");
            }

            _ = new Random();
            char[] caracters = passwordBuilder.ToString().ToCharArray();

            //Fisher-Yates
            for (int i = caracters.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (caracters[j], caracters[i]) = (caracters[i], caracters[j]);
            }

            return new string(caracters);
        }
    }
} 