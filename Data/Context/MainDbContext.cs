using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class MainDbContext : DbContext
{
    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {

    }

    #region Tables

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    #endregion

    #region ManageCascade-OnModelCreateing

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<User>()
        //    .HasQueryFilter(u => !u.IsDelete);
        //var cascades = modelBuilder.Model.GetEntityTypes()
        //    .SelectMany(t => t.GetForeignKeys())
        //    .Where(fk => fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        //foreach (var fk in cascades)
        //{
        //    fk.DeleteBehavior = DeleteBehavior.Restrict;
        //}
        modelBuilder.ApplyConfiguration(new UserConfig());
        modelBuilder.ApplyConfiguration(new RoleConfig());
        modelBuilder.ApplyConfiguration(new UserRoleConfig());

        base.OnModelCreating(modelBuilder);
    }

    #endregion
}