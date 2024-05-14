using Fiit_passport.Models;
using Microsoft.EntityFrameworkCore;

namespace Fiit_passport.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Admin> Admins { get; init; }
    public DbSet<Passport> Passports { get; init; }
    public DbSet<ConnectId> ConnectIds { get; init; }
    public DbSet<SessionNumber> SessionNumbers { get; init; }
}