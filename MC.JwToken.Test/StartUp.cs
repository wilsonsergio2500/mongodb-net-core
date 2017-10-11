using MC.Encryptor;
using MC.JwToken.Models;
using MC.JwToken.ModelToken;
using MC.JwToken.Providers;
using MC.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MC.JwToken.Test
{
    [TestClass]
    public class StartUp
    {
        public static IServiceProvider ServiceProvider { get; set; }

        [AssemblyInitialize]
        public static void Init(TestContext context)
        {
            string corePath = Directory.GetCurrentDirectory();
            string debug = Directory.GetParent(corePath).FullName;
            string bin = Directory.GetParent(debug).FullName;
            string Path = Directory.GetParent(bin).FullName;

                var builder = new ConfigurationBuilder()
                .SetBasePath(Path)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            ServiceProvider = RegisterServiceCollection();


        }
        public static IConfigurationRoot Configuration { get; set; }

        public static IServiceProvider RegisterServiceCollection()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddOptions();

            
            services.Configure<AuthConfig>(Configuration.GetSection("AuthConfig"));
            services.AddSingleton<IModelTokenGenerator<User>, ModelTokenGenerator<User>>();
            services.AddSingleton<IDecryptProvider, DecryptProvider>();
            services.AddSingleton<IJwtSecurityProvider, JwtSecurityProvider>();
            services.AddSingleton<ITokenEncryptionProvider, TokenEncryptionProvider>();

            return services.BuildServiceProvider();


        }
    }
}
