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

        Task<bool> DoesUserNameExist(string username);

        Task<User> GetUserByNameOrEmail(string UserName);

        Task<bool> UpdateImage(string userId, string Image);

        Task<bool> UpdatePassword(string userId, string Password, string EncryptionKey);

        Task<bool> UpdateBio(string userId, string Bio, string JobTitle, List<Strength> strengths);

        Task<bool> DeactivateUserByEmail(string email);

        Task<bool> ActivateUserByEmail(string email);
    }
}
