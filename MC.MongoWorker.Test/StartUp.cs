using MC.Interfaces.client;
using MC.Interfaces.Context;
using MC.Interfaces.Repository;
using MC.Interfaces.Repository.Base;
using MC.Models;
using MC.MongoWorker.client;
using MC.MongoWorker.Repository;
using MC.MongoWorker.Repository.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace MC.MongoWorker
{
    [TestClass]
    public class StartUp
    {
        public static IServiceProvider ServiceProvider { get; set; }

        [AssemblyInitialize]
        public static void Init(TestContext context) {
            ServiceProvider = RegisterServiceCollection();


        }

        public static IServiceProvider RegisterServiceCollection()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IMongoClientContext, MongoClientContext>();
            
            services.AddSingleton<IBaseRepository<PostType>, PostTypeRepository>();
            services.AddSingleton<IBaseRepository<Role>, RoleRepository>();
            services.AddSingleton<IBaseRepository<Like>, LikeRepository>();
            services.AddSingleton<IInviteRepository, InviteRepository>();

            services.AddSingleton<IBaseGridFsFileRepository, BaseGridFsFileRepository>();


            return services.BuildServiceProvider();


        }
    }
}
