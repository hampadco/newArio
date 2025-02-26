using Microsoft.EntityFrameworkCore;

public class Context : DbContext
{

    //ctor
    public Context(DbContextOptions<Context> options) : base(options)
    {

    }


    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Cards> Cards { get; set; }
    
    

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) // سایت
    // {
    //     optionsBuilder.UseSqlServer("Server=87.107.121.61,1430;Database=ariogame_db;User Id=ariogame_user;Password=12345@Iran;Trusted_Connection=False;MultipleActiveResultSets=True;TrustServerCertificate=True");
    // }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) // برای من
    // {
    //     optionsBuilder.UseSqlServer("server=.\\SQL2019;database=Ario;trusted_connection=true;MultipleActiveResultSets=True;TrustServerCertificate=True");
    // }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) // برای مهندس
    // {
    //     optionsBuilder.UseSqlServer("server=.;database=ariodb;trusted_connection=true;MultipleActiveResultSets=True;TrustServerCertificate=True");
    // }
}