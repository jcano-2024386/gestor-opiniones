using GestorOpiniones.Api.Models;
using MongoDB.Driver;

namespace GestorOpiniones.Api.Repositories;

public class PostRepository
{
    private readonly IMongoCollection<Post> _collection;
    public PostRepository(IMongoDatabase db) => _collection = db.GetCollection<Post>("posts");

    public async Task<Post> CreateAsync(Post p) { await _collection.InsertOneAsync(p); return p; }
    public async Task<List<Post>> ListAsync() => await _collection.Find(_ => true).SortByDescending(p => p.CreatedAt).ToListAsync();
    public async Task<Post?> GetByIdAsync(string id) => await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
    public async Task UpdateAsync(Post p) => await _collection.ReplaceOneAsync(x => x.Id == p.Id, p);
    public async Task DeleteAsync(string id) => await _collection.DeleteOneAsync(p => p.Id == id);
}
