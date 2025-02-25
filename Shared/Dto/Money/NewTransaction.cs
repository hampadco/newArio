public class NewTransaction
{
    public int UserId { get; set; }
    public string Description { get; set; } = null!;
    public string Details { get; set; } = null!;
    public TransactionType Type { get; set; }
    public int Amount { get; set; }
}