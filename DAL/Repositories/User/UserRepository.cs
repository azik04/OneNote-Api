using Domain.Models;

namespace DAL.Repositories.User;

public class UserRepository : IBaseRepository<Users>
{
    private readonly ApplicationDbContext _db;
    public UserRepository(ApplicationDbContext db)
    {
        _db = db; 
    }


    public async Task<bool> Create(Users entity)
    {
        await _db.Users.AddAsync(entity);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(Users entity)
    {
        _db.Users.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }

    public IQueryable<Users> GetAll() => _db.Users.AsQueryable();

    public async Task<Users> Update(Users entity)
    {
        _db.Users.Update(entity);
        await _db.SaveChangesAsync();
        return entity;
    }
}
