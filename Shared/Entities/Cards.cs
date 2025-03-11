using System.ComponentModel.DataAnnotations;

public class Cards
{
    [Key]
    public int Id { get; set; }
    public string CardNumber { get; set; } = null!;
    public string CardName { get; set; } = null!;
    public string CardBank { get; set; } = null!;
    public bool IsActive { get; set; }
    public List<DepositRequest>? DepositRequests { get; set; }
}