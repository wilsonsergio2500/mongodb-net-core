using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MC.Interfaces.Repository.Base;
using Mdls = MC.Models;
using MongoCoreNet.Models;
using MC.Interfaces.Repository;

namespace MongoCoreNet.Controllers
{
    [Produces("application/json")]
    [Route("api/Category")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        public CategoryController(ICategoryRepository categoryRepo)
        {
            categoryRepository = categoryRepo;
        }

        [HttpPost("new/{name}")]
        public async Task<ActionResponse> Post(string name) {

            Mdls.Category category = new Mdls.Category
            {
                Name = name
            };

            string categoryId = await categoryRepository.Add(category);

            return new ActionResponse { State = !String.IsNullOrEmpty(categoryId) };


        }

        [HttpGet("records/{skip}/{take}")]
        public async Task<ListResponse<Mdls.Category>> GetCategories(int skip, int take) {

            long count = await categoryRepository.GetTotal();

            List<Mdls.Category> items = await categoryRepository.Get(skip, take);

            return new ListResponse<Mdls.Category>
            {
                Count = count,
                Result = items

            };
        }

        [HttpGet("profile/tabs/views")]
        public async Task<List<Mdls.Category>> GetForProfileDisplay() {
            string[] Ids = new string[] { "59d2b33994d8e8dfd4460ff9", "59d2b3af94d8e8dfd4460ffb" };

            List<Mdls.Category> tabs = new List<Mdls.Category>();

            foreach (string categoryId in Ids) {
                Mdls.Category item = await categoryRepository.Get(categoryId);
                tabs.Add(item);
            }

            return tabs;
        }


        [HttpGet("exist/{name}")]
        public async Task<ActionResponse> DoesCategoryNameExist(string name) {

            bool categoryNameExist = await categoryRepository.DoesNameExist(name);

            return new ActionResponse
            {
                State = categoryNameExist
            };
        }

        [HttpGet("match/{keyword}")]
        public async Task<List<Mdls.Category>> MatchName(string keyword)
        {
            List<Mdls.Category> records = await categoryRepository.MatchRecordsByName(keyword);
            return records;
        }
    }
}