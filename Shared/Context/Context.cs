using Microsoft.EntityFrameworkCore;

public class Context : DbContext
{

    //ctor
    public Context(DbContextOptions<Context> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WithdrawalRequest>()
            .HasOne(w => w.Transaction)
            .WithOne()
            .HasForeignKey<WithdrawalRequest>(w => w.TransactionId)
            .OnDelete(DeleteBehavior.Restrict);
    }


    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Cards> Cards { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<DepositRequest> DepositRequests { get; set; }
    public DbSet<WithdrawalRequest> WithdrawalRequests { get; set; }



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