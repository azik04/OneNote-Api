using Domain.Models;

namespace DAL.Repositories.Section;

public class SectionRepository : IBaseRepository<Sections>
{
    private readonly ApplicationDbContext _db;
    public SectionRepository(ApplicationDbContext db)
    {
        _db = db;
    }


    public async Task<bool> Create(Sections entity)
    {
        await _db.Sections.AddAsync(entity);
        await _db.SaveChangesAsync();
        return true;
    }


    public async Task<bool> Delete(Sections entity)
    {
        _db.Sections.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }


    public IQueryable<Sections> GetAll() => _db.Sections.AsQueryable();


    public async Task<Sections> Update(Sections entity)
    {
        _db.Sections.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
}
