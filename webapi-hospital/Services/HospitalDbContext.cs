using Microsoft.EntityFrameworkCore;
using webapi_hospital.Models;

namespace webapi_hospital.Services;

public sealed class HospitalDbContext : DbContext
{
    public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
    {
        Database.EnsureCreated();
        // Database.EnsureDeleted();
    }

    public DbSet<User?> Users { get; set; } = null!;
}