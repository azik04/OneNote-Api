using Domain.Models;

namespace DAL.Repositories.Folder;

public class FolderRepository : IBaseRepository<Folders>
{
    private readonly ApplicationDbContext _db;
    public FolderRepository(ApplicationDbContext db)
    {
        _db = db;
    }


    public async Task<bool> Create(Folders entity)
    {
        await _db.Folders.AddAsync(entity);
        await _db.SaveChangesAsync();
        return true;
    }


    public async Task<bool> Delete(Folders entity)
    {
        _db.Folders.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }


    public IQueryable<Folders> GetAll() => _db.Folders.AsQueryable();


    public async Task<Folders> Update(Folders entity)
    {
        _db.Folders.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
}
