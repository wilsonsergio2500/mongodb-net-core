using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MC.Models;
using Microsoft.AspNetCore.Authorization;

namespace MongoCoreNet.Controllers
{
    [Produces("application/json")]
    [Route("api/Role")]
    public class RoleController : Controller
    {
        [HttpGet("list")]
        [Authorize(Policy = Authorization.Policies.AUTHORIZATION_TOKEN)]
        public List<DTOs.Role> GetRoles() {

            return new List<DTOs.Role>() {
                new DTOs.Role(){ Id = (int)RoleType.Participant, Name = "Member" },
                new DTOs.Role(){ Id = (int)RoleType.Administrator, Name = "Administrator"},
                new DTOs.Role(){Id = (int)RoleType.Lead, Name = "Lead"}
            };
        }

    }
}