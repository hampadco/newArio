using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("[Controller]/[Action]")]
public class UserController : ControllerBase
{
    private readonly Context db;
    public UserController(Context _db)
    {
        db = _db;
    }

    [HttpPost]
    public IActionResult AddUsers(User user)
    {
        if (db.Users.Any(u => u.UserName == user.UserName))
        {
            return BadRequest("نام کاربری قبلاً استفاده شده است.");
        }

        if (db.Users.Any(u => u.PhoneNumber == user.PhoneNumber))
        {
            return BadRequest("شماره تلفن قبلاً ثبت شده است.");
        }

        db.Users.Add(user);
        db.SaveChanges();
        return Ok("کاربر با موفقیت ثبت شد.");
    }


    [HttpGet]
    public List<User> GetUsers()
    {
        return db.Users.OrderByDescending(x => x.Id).ToList();
    }

    [HttpPost]
    public IActionResult CheckLogin([FromBody] LoginRequest loginRequest)
    {
        // بررسی اینکه نام کاربری و رمز عبور وجود داشته باشند
        var user = db.Users.FirstOrDefault(u => u.UserName == loginRequest.Username && u.Password == loginRequest.Password);

        if (user != null)
        {
            // اگر کاربر پیدا شد، ورود موفقیت‌آمیز است
            return Ok(new tokenJwt { Token = GenerateToken(user) });
        }
        else
        {
            // اگر کاربر پیدا نشد، ورود ناموفق است
            return Unauthorized("نام کاربری یا رمز عبور اشتباه است");
        }
    }

    [Authorize]
    [HttpGet]
    public IActionResult Getuser()
    {
        int UserId = Convert.ToInt32(User.FindFirstValue("id"));
        var user = db.Users.Find(UserId);

        if (user == null)
        {
            return NotFound("پیدا نشد");
        }
        return Ok(user);
    }

    [Authorize]
    [HttpPut]
    public IActionResult UpdateUser(User newuser)
    {
        var find = db.Users.Find(newuser.Id);
        find.Name = newuser.Name;
        find.Password = newuser.Password;
        db.Users.Update(find);
        db.SaveChanges();
        return Ok("موفقیت امیز بود");

    }

    private string GenerateToken(User user)
    {
        var claims = new[]
       {
            new Claim("id", user.Id.ToString()), // Id
            new Claim("name", user.Name), // name
            new Claim("Username",user.UserName) //Username
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345hampadco5656hastalavista"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "Ario",
            audience: "Client",
            claims: claims,
            expires: DateTime.Now.AddDays(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
        // return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }

}