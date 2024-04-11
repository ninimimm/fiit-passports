using System.ComponentModel.DataAnnotations.Schema;
using Fiit_passport.Models;
using Microsoft.EntityFrameworkCore;

namespace Fiit_passport.Databased;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Admin> Admins { get; set; }
    public DbSet<Passport> Passports { get; set; }
    public DbSet<ConnectId> ConnectIds { get; set; }
    public DbSet<ConnectSession> ConnectSessions { get; set; }
}