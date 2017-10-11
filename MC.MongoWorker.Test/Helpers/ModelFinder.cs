using MC.Models;
using MC.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MC.MongoWorker.Test.Helpers
{

    interface IModelFinder<T> where T : BaseEntity, new() {
        T GetModel();
    }

    public static class ModelResolver {
        public static Func<Role> getRole = () =>  new Role { Name = "new role", RoleTypeId = RoleType.Participant } ;
        public static Func<PostType> getPostType = () => new PostType { Name = "new post type", CategoryTypeId = CategoryType.Personal };

    }

    public  class ModelFinder : IModelFinder<PostType>, IModelFinder<Role>
    {
        
          Role  IModelFinder<Role>.GetModel()
        {
            return ModelResolver.getRole();
        }

         PostType IModelFinder<PostType>.GetModel()
        {
            return ModelResolver.getPostType();
        }
    }
}
