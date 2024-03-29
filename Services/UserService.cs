﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using ClickerAPI.Models;

namespace ClickerAPI.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;
        public UserService(IClickerDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public List<User> Get() =>
            _users.Find(user => true).ToList();

        public User Get(string id) =>
            _users.Find<User>(user => user.Id == id).FirstOrDefault();

        public User GetByUsername(string username) =>
            _users.Find<User>(user => user.Username == username).FirstOrDefault();

        public User Create(User user)
        {

            _users.InsertOne(user);
            return user;
        }
        public void Update(string username, User userIn)
        {
            _users.ReplaceOne(user => user.Username == username, userIn);
        }   

        public void Remove(User userIn) =>
            _users.DeleteOne(user => user.Id == userIn.Id);

        public void Remove(string id) =>
            _users.DeleteOne(user => user.Id == id);
    }
}
