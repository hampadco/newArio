using System.Data.Common;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[Controller]/[Action]")]
public class TransactionController : Controller
{
    private readonly Context db;
    public TransactionController(Context _db)
    {
        db = _db;
    }

    [HttpPost]
    public IActionResult AddTransaction(NewTransaction Transaction)
    {
        try
        {
            User user = db.Users.Include(x => x.Transactions).FirstOrDefault(x => x.Id == Transaction.UserId)!;

            if (Transaction.Type == TransactionType.Deposit)
            {
                user.Deposit(db, Transaction.Amount, Transaction.Description, Transaction.Details);
            }

            else
            {
                user.CheckBalance();
                user.Withdraw(db, Transaction.Amount, Transaction.Description, Transaction.Details);
            }
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public IActionResult ViewTransactions()
    {
        int UserId = Convert.ToInt32(User.FindFirstValue("id"));
        User user = db.Users.Include(x => x.Transactions).FirstOrDefault(x => x.Id == UserId)!;
        List<TraceHistory> histories = new List<TraceHistory>();
        PersianCalendar pc = new PersianCalendar();
        if (user.Transactions != null)
        {
            foreach (Transaction item in user.Transactions)
            {
                histories.Add(new TraceHistory
                {
                    Id = item.Id,
                    Amount = item.Amount,
                    Date = $"{pc.GetYear(item.CreateDateTime)}/{pc.GetMonth(item.CreateDateTime)}/{pc.GetDayOfMonth(item.CreateDateTime)}",
                    Title = item.Description,
                    type = item.Type
                });
            }
        }
        return Ok(new Traces
        {
            Balance = user.Balance,
            TraceHistories = histories
        });
    }

    [HttpPost]
    public IActionResult AddDepositRequest(c2cRequest c2c)
    {
        int UserId = Convert.ToInt32(User.FindFirstValue("id"));
        if (db.DepositRequests.Any(x => x.UserId == UserId && x.IsValid == null && x.CreateDateTime.AddHours(5) > DateTime.Now))
        {
            return BadRequest("شما در ساعات گذشته یک درخواست ارسال کردید. لطفا صبر کنید");
        }
        DepositRequest request = new DepositRequest
        {
            Amount = c2c.Price,
            CardId = c2c.CardId,
            ClientCardNumber = c2c.ClientCardNumber,
            CheckTime = DateTime.Now,
            CreateDateTime = DateTime.Now,
            IsValid = null,
            UserId = Convert.ToInt32(User.FindFirstValue("id"))
        };
        db.DepositRequests.Add(request);
        db.SaveChanges();
        return Ok("ثبت درخواست با موفقیت انجام شد.");
    }
}