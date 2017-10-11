using MC.Interfaces.Repository.Base;
using MC.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MC.Interfaces.Repository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> DoesEmailExist(string email);

        Task<User> GetUserByNameOrEmail(string UserName);

        Task<bool> UpdateImage(string userId, string Image);

        Task<bool> UpdatePassword(string userId, string Password, string EncryptionKey);

        Task<bool> UpdateBio(string userId, string Bio);
    }
}
