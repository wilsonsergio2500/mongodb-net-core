using MC.Models;
using MC.MongoWorker.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using MC.Interfaces.client;
using MC.Interfaces.Repository;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using System.Linq;
using MC.MongoWorker.Helpers;

namespace MC.MongoWorker.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IMongoClientContext mongoClientContext) : base(mongoClientContext)
        {
        }

        public async Task<User> GetUserByEmail(string email) {
            User user;
            try
            {
                FilterDefinition<User> queryEmail = Builders<User>.Filter.Eq(x => x.Email, email.ToLower());
                user = await Items.Find(queryEmail).SingleAsync();
                return user;
            }
            catch {
                return null;
            }
        }

        public override async Task<string> Add(User entity)
        {
            try
            {
                if (String.IsNullOrEmpty(entity.id))
                {
                    entity.CreatedDate = JsDateConverter.Convert(DateTime.UtcNow);
                    entity.Active = true;

                    await Items.InsertOneAsync(entity); ;
                    return entity.id.ToString();
                }

                entity.UserName = entity.UserName.ToLower();
                entity.Email = entity.Email.ToLower();
                

                var query = Builders<User>.Filter.Eq(g => g.id, entity.id);
                await Items.ReplaceOneAsync(query, entity);
                return entity.id;

            }
            catch
            {
                return null;
            }
        }


        public async Task<bool> ActivateUserByEmail(string email)
        {
            try
            {
                FilterDefinition<User> query = Builders<User>.Filter.Eq(g => g.Email, email);
                User user = await Items.Find(query).SingleAsync();
                if (user != null) {
                    return await this.Activate(user.id);
                }

            }
            catch {
                return false;
            }

            return false;
        }

        public async Task<bool> DeactivateUserByEmail(string email)
        {
            try
            {
                FilterDefinition<User> query = Builders<User>.Filter.Eq(g => g.Email, email);
                User user = await Items.Find(query).SingleAsync();
                if (user != null)
                {
                    return await this.Deactivate(user.id);
                }

            }
            catch
            {
                return false;
            }

            return false;
        }

        public async Task<bool> SetRoleByEmail(string email, int roleId) {
            try
            {
                FilterDefinition<User> query = Builders<User>.Filter.Eq(g => g.Email, email);
                User user = await Items.Find(query).SingleAsync();
                if (user != null)
                {
                    FilterDefinition<User> q = Builders<User>.Filter.Eq(g => g.id, user.id);
                    UpdateDefinition<User> update = Builders<User>.Update.Set(g => g.Role, (RoleType)roleId);
                    await Items.UpdateOneAsync(q, update);
                    return true;
                }
            }
            catch {
                return false;
            }
            return false;
        }

        public async Task<bool> DoesEmailExist(string email)
        {
            User user;
            try
            {
                FilterDefinition<User> query = Builders<User>.Filter.Regex(x => x.Email, BsonRegularExpression.Create(new Regex(email, RegexOptions.IgnoreCase)));
                user = await Items.Find(query).SingleAsync();
                return user != null;
            }
            catch  {
                return false;
            }
        }

        public async Task<bool> DoesUserNameExist(string username) {
            try
            {
                FilterDefinition<User> query = Builders<User>.Filter.Regex(x => x.UserName, BsonRegularExpression.Create(new Regex(username, RegexOptions.IgnoreCase)));
                User user = await Items.Find(query).SingleAsync();
                return user != null;
            }
            catch  {
                return false;
            }
        }


        public async Task<User> GetUserByNameOrEmail(string userName) {

            User user;

            try
            {

                FilterDefinition<User> queryUser = Builders<User>.Filter.Eq(x => x.UserName, userName.ToLower() );
                FilterDefinition<User> queryEmail = Builders<User>.Filter.Eq(x => x.Email, userName.ToLower());


                FilterDefinition<User> querys = Builders<User>.Filter.Or(queryUser, queryUser);
                user = await Items.Find(querys).SingleAsync();

                return user;
            }
            catch (Exception e)
            {

                return null;
            }

        }

        public async Task<bool> UpdateBio(string userId, string Bio, string JobTitle, List<Strength> strengths)
        {
            try
            {
                FilterDefinition<User> query = Builders<User>.Filter.Eq(g => g.id, userId);
                UpdateDefinition<User> update = Builders<User>.Update.Set(g => g.Bio, Bio )
                    .Set(g => g.JobTitle, JobTitle)
                    .Set(g => g.Strengths, strengths);
                await Items.UpdateOneAsync(query, update);
                return true;
            }
            catch {
                return false;
            }
        }

        public async Task<bool> UpdateImage(string userId, string Image) {

            try
            {
                FilterDefinition<User> query = Builders<User>.Filter.Eq(g => g.id, userId);
                UpdateDefinition<User> update = Builders<User>.Update.Set(g => g.Image, Image);
                await Items.UpdateOneAsync(query, update);
                return true;
            }
            catch {
                return false;
            }


        }

        public async Task<bool> UpdatePassword(string userId, string Password, string EncryptionKey) {
            try
            {
                FilterDefinition<User> query = Builders<User>.Filter.Eq(g => g.id, userId);
                UpdateDefinition<User> update = Builders<User>.Update.Set(g => g.Password, Password).Set(g => g.EncryptionKey, EncryptionKey);
                await Items.UpdateOneAsync(query, update);
                return true;
            }
            catch
            {
                return false;
            }

        }

        
    }
}
