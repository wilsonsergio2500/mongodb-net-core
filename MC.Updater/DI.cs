using MC.AmazonStoreS3.Models;
using MC.AmazonStoreS3.Providers;
using MC.Interfaces.client;
using MC.Interfaces.Repository;
using MC.MongoWorker.client;
using MC.MongoWorker.Models;
using MC.MongoWorker.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MC.Updater
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DI : Attribute
    {
        public static IServiceProvider ServiceProvider { get; set; }
        public static IConfigurationRoot Configuration { get; set; }
        public DI()
        {
            string corePath = Directory.GetCurrentDirectory();
            string debug = Directory.GetParent(corePath).FullName;
            string bin = Directory.GetParent(debug).FullName;
            string Path = Directory.GetParent(bin).FullName;

            var builder = new ConfigurationBuilder()
            .SetBasePath(corePath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
            Configuration = builder.Build();
            ServiceProvider = RegisterServiceCollection();
        }

        public static IServiceProvider RegisterServiceCollection()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddOptions();

            #region AWS S3
            services.Configure<AmazonStoreS3Config>(Configuration.GetSection("AmazonStoreS3Config"));
            services.AddSingleton<IAmazonS3ImageProvider, AmazonS3ImageProvider>();
            #endregion

            services.Configure<MongoLabConfig>(Configuration.GetSection("MongLabConfig"));

            services.AddSingleton<IMongoClientContext, MongoLabClientContext>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IMilestoneRepository, MilestoneRepository>();



            return services.BuildServiceProvider();


        }

    }
}
