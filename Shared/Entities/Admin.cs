using System.ComponentModel.DataAnnotations;

public class Admin
{
    [Key]
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public DateTime CreateDateTime { get; set; }
}