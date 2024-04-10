using Microsoft.EntityFrameworkCore;

namespace Fiit_passport.Databased;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
}