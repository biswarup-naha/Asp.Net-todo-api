using System;
using MongoDB.Driver;
using TodoApi.Models;

namespace TodoApi.Services;

public class UserService
{
    private readonly IMongoCollection<User> _users;

    public UserService(DatabaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _users = database.GetCollection<User>(settings.UserCollectionName);
    }

    public async Task<List<User>> GetAll() => await _users.Find(user => true).ToListAsync();

    public async Task<User> GetById(string id) => await _users.Find(user => user.Id == id).FirstOrDefaultAsync();

    public async Task Add(User user){
        user.Password= BCrypt.Net.BCrypt.HashPassword(user.Password, BCrypt.Net.BCrypt.GenerateSalt(10));

        await _users.InsertOneAsync(user);
    }

    public async Task Update(string id, User updatedUser) =>
        await _users.ReplaceOneAsync(user => user.Id == id, updatedUser);

    public async Task Delete(string id) =>
        await _users.DeleteOneAsync(user => user.Id == id);

    public async Task<User> GetByEmail(string email) =>
        await _users.Find(user => user.Email == email).FirstOrDefaultAsync();

    public bool CheckPassword(User user, string password)=>
        BCrypt.Net.BCrypt.Verify(password, user.Password);

}
