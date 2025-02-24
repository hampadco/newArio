using Microsoft.EntityFrameworkCore;

public class Context:DbContext
{

    //ctor
    public Context(DbContextOptions<Context> options):base(options)
    {
        
    }


    public DbSet<User> Users { get; set; }

    
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlServer("server=.;database=ariodb;trusted_connection=true;MultipleActiveResultSets=True;TrustServerCertificate=True");
    // }
}