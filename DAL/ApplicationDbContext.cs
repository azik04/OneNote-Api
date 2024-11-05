using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }


    public DbSet<Users> Users { get; set; }
    public DbSet<Notes> Notes { get; set; }
    public DbSet<Folders> Folders { get; set; }
    public DbSet<Sections> Sections { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasData(new Users
            {
                UserName = "Admin",
                Password = "Admin123",
                Id = 1,
                Role = Domain.Enum.Role.Admin
            });
        });

        modelBuilder.Entity<Users>(opt =>
        {
            opt.HasKey(opt => opt.Id);
        });
        modelBuilder.Entity<Notes>(opt => 
        {
            opt.HasKey(opt => opt.Id);

            opt.HasOne(x => x.Folder).WithMany(x => x.Notes).HasForeignKey(x => x.FolderId).OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Folders>(opt =>
        {
            opt.HasKey(opt => opt.Id);
            
            opt.HasOne(X => X.Section).WithMany(x => x.Folders).HasForeignKey(x => x.SectionId).OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Sections>(opt =>
        {
            opt.HasKey(opt => opt.Id);
        });
    }
}
