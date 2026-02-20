using GestorOpiniones.Api.Models;
using MongoDB.Driver;

namespace GestorOpiniones.Api.Repositories;

public class UserRepository
{
    private readonly IMongoCollection<User> _collection;
    public UserRepository(IMongoDatabase db) => _collection = db.GetCollection<User>("users");

    public async Task<User?> GetByIdAsync(string id) => await _collection.Find(u => u.Id == id).FirstOrDefaultAsync();
    public async Task<User?> GetByEmailAsync(string email) => await _collection.Find(u => u.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();
    public async Task<User?> GetByUsernameAsync(string username) => await _collection.Find(u => u.Username.ToLower() == username.ToLower()).FirstOrDefaultAsync();

    public async Task CreateAsync(User user) => await _collection.InsertOneAsync(user);
}
