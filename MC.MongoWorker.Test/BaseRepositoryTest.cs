using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using MC.Interfaces.Context;
using MC.MongoWorker.Repository;
using MC.Models;
using Microsoft.Extensions.DependencyInjection;
using MC.Interfaces.client;
using MC.MongoWorker.client;
using MC.Interfaces.Repository.Base;
using MC.MongoWorker.Test.Helpers;
using MC.Models.Base;
using System.Text;

namespace MC.MongoWorker.Test
{
    [TestClass]
    public class BaseRepositoryTest
    {

        [TestMethod]
        public async Task Add() {


            string Id =  await Add<PostType>();
            PostType postType = await Get<PostType>(Id);
            await Delete<PostType>(Id);
        }


        public async Task<string> Add<T>() where T: BaseEntity, new() {

            IServiceProvider sp = StartUp.ServiceProvider;
            IBaseRepository<T> repo = sp.GetService<IBaseRepository<T>>();
            IModelFinder<T> modelFinder = new ModelFinder() as IModelFinder<T>;
            T model = modelFinder.GetModel();
            string Id = await repo.Add(model);
            Assert.IsNotNull(Id);
            return Id;
        }

        public async Task Delete<T>(string Id) where T : BaseEntity, new() {
            IServiceProvider sp = StartUp.ServiceProvider;
            IBaseRepository<T> repo = sp.GetService<IBaseRepository<T>>();

            bool deleted = await repo.Delete(Id);
            Assert.IsTrue(deleted);


        }

        public async Task<T> Get<T>(string Id) where T : BaseEntity, new() {

            IServiceProvider sp = StartUp.ServiceProvider;
            IBaseRepository<T> repo = sp.GetService<IBaseRepository<T>>();

            T item = await repo.Get(Id);
            Assert.IsNull(item);

            return item;

        } 


    }
}
