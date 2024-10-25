using Domain.Models;

namespace DAL.Repositories.Note;

public class NoteRepository : IBaseRepository<Notes>
{
    private readonly ApplicationDbContext _db;
    public NoteRepository(ApplicationDbContext db)
    {
        _db = db;
    }


    public async Task<bool> Create(Notes entity)
    {
        await _db.Notes.AddAsync(entity);
        await _db.SaveChangesAsync();
        return true;
    }


    public async Task<bool> Delete(Notes entity)
    {
        _db.Notes.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }


    public IQueryable<Notes> GetAll() => _db.Notes.AsQueryable();


    public async Task<Notes> Update(Notes entity)
    {
        _db.Notes.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
}
