

using System.ComponentModel.DataAnnotations;

public class LoginRequest
{
    [Required(ErrorMessage = "نام کاربری الزامی است")]
    public string Username { get; set; }


    
    [Required(ErrorMessage = "رمز عبور الزامی است")]
    [MinLength(6, ErrorMessage = "رمز عبور باید حداقل 6 کاراکتر باشد")]
    public string Password { get; set; }
}
    
