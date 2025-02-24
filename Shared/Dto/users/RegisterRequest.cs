using System.ComponentModel.DataAnnotations;

public class RegisterRequest
{
    [Required(ErrorMessage = "نام  و نام خانوادگی الزامی است")]
    public string Name { get; set; }

    [Required(ErrorMessage = "شماره تلفن الزامی است")]
    [Phone(ErrorMessage = "شماره تلفن معتبر نیست")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "نام کاربری الزامی است")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "رمز عبور الزامی است")]
    [MinLength(6, ErrorMessage = "رمز عبور باید حداقل 6 کاراکتر باشد")]
    public string Password { get; set; }
    public string PasswordCheck { get; set; }

    [AllowedValues(true,ErrorMessage = "لطفا با شرایط و ضوابط موافقت کنید.")]
    public bool Agree { get; set; }
}