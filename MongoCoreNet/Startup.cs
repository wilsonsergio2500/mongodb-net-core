using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.ResponseCompression;

using MC.Interfaces.client;
using MC.MongoWorker.client;
using MC.Interfaces.Context;
using MC.Interfaces.Repository;
using MC.MongoWorker.Repository;
using AutoMapper;
using MC.MongoWorker.Models;
using MC.Interfaces.Repository.Base;
using MC.Models;
using MC.JwToken.ModelToken;
using MC.Encryptor;
using MC.JwToken;
using MC.JwToken.Models;
using Microsoft.AspNetCore.Authorization;
using MongoCoreNet.Authorization;
using MC.Cache;
using MC.Email.Models;
using MC.Email;
using MongoCoreNet.Helpers;

namespace MongoCoreNet
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        const string CorsPolicy = "CorsGlobalPolicy";
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddCors(o => o.AddPolicy(CorsPolicy, builder => {
                builder.WithOrigins(new string[] { "http://localhost:9000", "http://landmarkapp-dev.us-west-2.elasticbeanstalk.com" }).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            }));

            services.AddOptions();
            services.AddMemoryCache();
            services.AddSingleton<ICacheProvider, CacheProvider>();

            #region JwToken
            services.Configure<AuthConfig>(Configuration.GetSection("AuthConfig"));
            services.AddScoped<IModelTokenGenerator<User>, ModelTokenGenerator<User>>();
           
            #endregion

            #region Encryption Provider
            services.AddSingleton<IDecryptProvider, DecryptProvider>();
            services.AddScoped<IEncryptionKeyGeneratorProvider, EncryptionKeyGeneratorProvider>();
            services.AddSingleton<IJwtSecurityProvider, JwtSecurityProvider>();
            services.AddSingleton<IAuthtenticationCurrentContext, AuthenticationCurrentContext>();
            services.AddSingleton<IAuthorizationHandler, Authorization.AuthorizationTokenPresent>();
            

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Authorization.Policies.AUTHORIZATION_TOKEN, policy =>
                policy.Requirements.Add(new Authorization.AuthorizationTokenRequirement()));
            });
            #endregion

            services.Configure<MongoLabConfig>(Configuration.GetSection("MongLabConfig"));
            services.Configure<EmailClientConfig>(Configuration.GetSection("EmailClientConfig"));


            services.AddSingleton<IMongoClientContext, MongoLabClientContext>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<ILikeRepository, LikeRepository>();
            services.AddSingleton<IInviteRepository, InviteRepository>();
            services.AddSingleton<ICategoryRepository, CategoryRepository>();
            services.AddSingleton<IMilestoneRepository, MilestoneRepository>();

            services.AddSingleton<IEmailProvider, EmailProvider>();
            services.AddSingleton<IUserCache, UserCache>();

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression();

            services.AddMvc();
            services.AddAutoMapper();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            app.UseResponseCompression();
            app.UseCors(CorsPolicy);
            //app.UseStaticFiles();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
