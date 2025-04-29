using System;
using MongoDB.Driver;
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

    public List<Todo> GetAll() => _todos.Find(todo => true).ToList();

    public Todo GetById(string id) => _todos.Find(todo => todo.Id == id).FirstOrDefault();

    public void Add(Todo todo) => _todos.InsertOne(todo);

    public void Update(string id, Todo updatedTodo) =>
        _todos.ReplaceOne(todo => todo.Id == id, updatedTodo);

    public void Delete(string id) =>
        _todos.DeleteOne(todo => todo.Id == id);
}
