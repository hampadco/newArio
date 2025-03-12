using System.ComponentModel.DataAnnotations;

public class User
{

    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "نام  و نام خانوادگی الزامی است")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "شماره تلفن الزامی است")]
    [Phone(ErrorMessage = "شماره تلفن معتبر نیست")]
    public string PhoneNumber { get; set; } = null!;

    [Required(ErrorMessage = "نام کاربری الزامی است")]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "رمز عبور الزامی است")]
    [MinLength(6, ErrorMessage = "رمز عبور باید حداقل 6 کاراکتر باشد")]
    public string Password { get; set; } = null!;
    public bool Status { get; set; }
    public List<Transaction>? Transactions { get; set; }
    public int Balance { get; set; }

    public void Deposit(Context db, int amount, string Description, string Details)
    {
        if (amount <= 0) throw new ArgumentException("مقدار واریزی باید بزرگتر از صفر باشد.");

        var transaction = new Transaction
        {
            UserId = this.Id,
            Type = TransactionType.Deposit,
            Amount = amount,
            CreateDateTime = DateTime.Now,
            Description = Description,
            Details = Details
        };

        Balance += amount;

        db.Transactions.Add(transaction);
        db.Users.Update(this);
        db.SaveChanges();
    }

    public void Withdraw(Context db, int amount, string Description, string Details)
    {
        if (amount <= 0) throw new ArgumentException("مقدار برداشت باید بزرگتر از صفر باشد.");
        if (Balance < amount) throw new InvalidOperationException("موجودی کافی نیست.");


        var transaction = new Transaction
        {
            UserId = this.Id,
            Type = TransactionType.Withdrawal,
            Amount = amount,
            CreateDateTime = DateTime.Now,
            Description = Description,
            Details = Details
        };

        Balance -= amount;

        db.Transactions.Add(transaction);
        db.Users.Update(this);
        db.SaveChanges();
    }

    public void CheckBalance()
    {
        if (Transactions != null)
        {
            var Deposits = Transactions.Where(x => x.Type == TransactionType.Deposit).Sum(x => x.Amount);
            var Withdrawals = Transactions.Where(x => x.Type == TransactionType.Withdrawal).Sum(x => x.Amount);
            Balance = Deposits - Withdrawals;
        }
    }

    public List<DepositRequest>? DepositRequests { get; set; }
}