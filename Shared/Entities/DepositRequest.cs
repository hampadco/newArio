using System.ComponentModel.DataAnnotations;

public class DepositRequest
{
    [Key]
    public int Id { get; set; }
    public string ClientCardNumber { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public bool? IsValid { get; set; }
    public string Amount { get; set; } = null!;
    public int CardId { get; set; }
    public Cards Card { get; set; } = null!;
    public DateTime CreateDateTime { get; set; }
    public DateTime CheckTime { get; set; }
    public string? Description { get; set; }
}