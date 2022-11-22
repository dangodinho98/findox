using Findox.Application.Dto.User;
using Findox.Application.Services.Account;
using Findox.Application.Services.Authenticate;
using Findox.Shared;

namespace Findox.Infra.IoC
{
    using System.Reflection;
    using Findox.Application.Mappings;
    using Findox.Application.Services.Group;
    using Findox.Infra.Authentication;
    using Findox.Infra.Data.Repositories.Group;
    using Findox.Infra.Data.Repositories.User;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
    using Microsoft.OpenApi.Models;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddAuthentication(services, configuration);
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            services.AddAutoMapper(typeof(DomainToDtoMappingProfile));

            services.AddRepositories();
            services.AddServices();

            return services;
        }

        private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITokenManager, TokenManager>();

            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opts =>
            {
                opts.SaveToken = true;
                opts.RequireHttpsMetadata = false;
                
                var key = configuration.GetValue<string>("AppSettings:Secret") ??
                                                    throw new ApplicationException("Missing jwt configuration.");
                var keyBytes = Encoding.ASCII.GetBytes(key);

                opts.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                };
            });

            services.AddSwaggerGen(setup =>
            {
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                setup.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSecurityScheme, Array.Empty<string>() } });
            });
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<IAccountService, AccountService>();
        }
    }
}