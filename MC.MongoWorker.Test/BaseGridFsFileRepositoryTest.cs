using MC.Interfaces.Repository.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace MC.MongoWorker.Test
{
    [TestClass]
    public class BaseGridFsFileRepositoryTest
    {
        [TestMethod]
        public async Task Add() {

            IServiceProvider sp = StartUp.ServiceProvider;
            IBaseGridFsFileRepository repo = sp.GetService<IBaseGridFsFileRepository>();

            string Id = null;
            using (var s = File.OpenRead(@"c:\img.txt")) {
               Id =  await repo.Add("base64.txt", s);
            }

            Assert.IsNotNull(Id);
        }
    }
}
