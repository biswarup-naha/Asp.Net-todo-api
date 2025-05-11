using System;
using MongoDB.Driver;
using TodoApi.Config;
using TodoApi.Models;

namespace TodoApi.Services;

public class TodoService
{
    private readonly IMongoCollection<Todo> _todos;

    public TodoService(DatabaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _todos = database.GetCollection<Todo>(settings.TodoCollectionName);
    }

    public async Task<List<Todo>> GetAll() => await _todos.Find(todo => true).ToListAsync();

    public async Task<Todo> GetById(string id) => await _todos.Find(todo => todo.Id == id).FirstOrDefaultAsync();

    public async Task Add(Todo todo) => await _todos.InsertOneAsync(todo);

    public async Task Update(string id, Todo updatedTodo) =>
        await _todos.ReplaceOneAsync(todo => todo.Id == id, updatedTodo);

    public async Task Delete(string id) =>
        await _todos.DeleteOneAsync(todo => todo.Id == id);
}
