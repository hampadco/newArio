using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Transaction
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; }
    public string Description { get; set; } = null!;
    public string Details { get; set; } = null!;
    public TransactionType Type { get; set; }
    public int Amount { get; set; }
    public DateTime CreateDateTime { get; set; }
}
public enum TransactionType
{
    Deposit, // واریزی
    Withdrawal//برداشت از حساب
}