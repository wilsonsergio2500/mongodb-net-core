﻿using MC.Models;
using MC.MongoWorker.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using MC.Interfaces.client;
using MC.Interfaces.Repository;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MC.MongoWorker.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IMongoClientContext mongoClientContext) : base(mongoClientContext)
        {
        }

        public async Task<bool> DoesEmailExist(string email)
        {
            User user;
            try
            {
                FilterDefinition<User> query = Builders<User>.Filter.Eq(q => q.Email, email);
                user = await Items.Find(query).SingleAsync();
                return user != null;
            }
            catch (Exception e) {
                return false;
            }
        }

        public async Task<User> GetUserByNameOrEmail(string userName) {

            User user;

            try
            {
                FilterDefinition<User> queryUser = Builders<User>.Filter.Eq(q => q.UserName, userName);
                FilterDefinition<User> queryEmail = Builders<User>.Filter.Eq(q => q.Email, userName);

                FilterDefinition<User> querys = Builders<User>.Filter.Or(queryUser, queryUser);
                user = await Items.Find(querys).SingleAsync();

                return user;
            }
            catch
            {

                return null;
            }

        }

        public async Task<bool> UpdateBio(string userId, string Bio)
        {
            try
            {
                FilterDefinition<User> query = Builders<User>.Filter.Eq(g => g.id, userId);
                UpdateDefinition<User> update = Builders<User>.Update.Set(g => g.Bio, Bio);
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
