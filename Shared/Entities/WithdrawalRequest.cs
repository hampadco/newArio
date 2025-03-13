using System.ComponentModel.DataAnnotations;

public class WithdrawalRequest
{
    [Key]
    public int Id { get; set; }
    public string ClientCardNumber { get; set; } = null!;
    public string Amount { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreateDateTime { get; set; }
    public DateTime CheckTime { get; set; }
    public bool? IsValid { get; set; }
    public int? TransactionId { get; set; }
    public Transaction? Transaction { get; set; }

}