using GestorOpiniones.Api.Models;
using MongoDB.Driver;

namespace GestorOpiniones.Api.Repositories;

public class CommentRepository
{
    private readonly IMongoCollection<Comment> _collection;
    public CommentRepository(IMongoDatabase db) => _collection = db.GetCollection<Comment>("comments");

    public async Task<Comment> CreateAsync(Comment c) { await _collection.InsertOneAsync(c); return c; }
    public async Task<List<Comment>> ListByPostAsync(string postId) => await _collection.Find(c => c.PostId == postId).SortByDescending(c => c.CreatedAt).ToListAsync();
    public async Task<Comment?> GetByIdAsync(string id) => await _collection.Find(c => c.Id == id).FirstOrDefaultAsync();
    public async Task UpdateAsync(Comment c) => await _collection.ReplaceOneAsync(x => x.Id == c.Id, c);
    public async Task DeleteAsync(string id) => await _collection.DeleteOneAsync(c => c.Id == id);
}
