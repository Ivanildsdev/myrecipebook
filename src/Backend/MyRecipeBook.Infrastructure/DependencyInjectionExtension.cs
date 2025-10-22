﻿using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Contracts.Repositories.User;
using MyRecipeBook.Domain.Security.Criptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Security.Criptography;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Generator;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Validator;
using MyRecipeBook.Infrastructure.Services.LoggedUser;
using System.Reflection;

namespace MyRecipeBook.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddRepositories(services);
            AddLoggedUser(services);
            AddTokens(services, configuration);
            AddEncripter(services, configuration);

            if (configuration.IsUnitTestEnviroment())
                return;

            var connectionString = configuration.ConnectionString();

            AddDbContext(services, connectionString);
            AddFluentMigrator(services, configuration);
        }

        private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
            
        }

        private static void AddDbContext(IServiceCollection services, string connectionString)
        {
            //var connectionString = Environment.GetEnvironmentVariable("ConnectionStringSql"); 

            services.AddDbContext<MyRecipeBookDbContext>(dbOptions =>
            {
                dbOptions.UseSqlServer(
                        connectionString,
                        sqlServerOptions =>
                        {
                            sqlServerOptions.EnableRetryOnFailure(
                                maxRetryCount: 5,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null);
                        }
                );
            });
        }

        private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();

            services.AddFluentMigratorCore()
                .ConfigureRunner(options => options
                .AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("MyRecipeBook.Infrastructure")).For.All());
                //.ScanIn(typeof(DependencyInjectionExtension).Assembly).For.All());
            //.AddLogging(lb => lb.AddFluentMigratorConsole());
        }

        private static void AddTokens(IServiceCollection services, IConfiguration configuration)
        {
            var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");
            var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationMinutes");
            
            services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey));
            services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signingKey!));

        }

        private static void AddEncripter(IServiceCollection services, IConfiguration configuration)
        {
            var additionalKey = configuration.GetValue<string>("Settings:Password:AditionalKey");
            services.AddScoped<IPasswordEncripter>(options => new Sha512Encripter(additionalKey));
        }
    }
}
