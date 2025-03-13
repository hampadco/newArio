using System.Data.Common;
using System.Globalization;
using System.Security.Claims;
using Extensions;
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

    [HttpGet]
    public IActionResult ViewTransactions()
    {
        int UserId = Convert.ToInt32(User.FindFirstValue("id"));
        User user = db.Users.Include(x => x.Transactions).FirstOrDefault(x => x.Id == UserId)!;
        List<TraceHistory> histories = new List<TraceHistory>();
        PersianCalendar pc = new PersianCalendar();
        if (user.Transactions != null)
        {
            foreach (Transaction item in user.Transactions.OrderByDescending(x=>x.Id))
            {
                histories.Add(new TraceHistory
                {
                    Id = item.Id,
                    Amount = item.Amount,
                    Date = $"{pc.GetYear(item.CreateDateTime)}/{pc.GetMonth(item.CreateDateTime)}/{pc.GetDayOfMonth(item.CreateDateTime)}",
                    Title = item.Description,
                    Description = item.Details,
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

    [HttpPost]
    public IActionResult AddWithdrawalRequest(c2cRequest c2c)
    {
        int UserId = Convert.ToInt32(User.FindFirstValue("id"));
        int AmountNum = Convert.ToInt32(c2c.Price);

        User user = db.Users.Include(x => x.Transactions).FirstOrDefault(x => x.Id == UserId)!;
        user.CheckBalance();

        if (user.Balance < AmountNum)
        {
            db.Users.Update(user);
            db.SaveChanges();
            return BadRequest("مبلغ درخواستی بیشتر از کیف پول میباشد.");
        }
        else if (AmountNum < 100000)
        {
            db.Users.Update(user);
            db.SaveChanges();
            return BadRequest("مبلغ درخواستی کمتر از صد هزار تومان میباشد.");
        }

        NewTransaction transaction = new NewTransaction
        {
            Amount = AmountNum,
            Description = "کاهش موجودی به موجب برداشت وجه",
            Details = $"کاربر {user.Name} با نام کاربری {user.UserName} در تاریخ {PersianDate.ToPersianDateString(DateTime.Now)} ساعت {(DateTime.Now.Hour.ToString().Count() == 1 ? $"0{DateTime.Now.Hour}" : DateTime.Now.Hour)}:{(DateTime.Now.Minute.ToString().Count() == 1 ? $"0{DateTime.Now.Minute}" : DateTime.Now.Minute)} مبلغ {AmountNum} تومان از {user.Balance} تومان موجودی خویش را درخواست کرد. مبلغ ذکر شده به حسابی با شماره {c2c.ClientCardNumber} واریز خواهد شد. ",
            Type = TransactionType.Withdrawal,
            UserId = UserId
        };

        var result = AddTransaction(transaction);

        if (result is not OkResult)
        {
            var badRequestResult = result as BadRequestObjectResult;
            TempData["Error"] = badRequestResult?.Value?.ToString() ?? "خطایی رخ داده است.";
            return RedirectToAction("Index");
        }
        WithdrawalRequest withdrawalRequest = new WithdrawalRequest
        {
            Amount = c2c.Price,
            ClientCardNumber = c2c.ClientCardNumber,
            CreateDateTime = DateTime.Now,
            CheckTime = DateTime.Now,
            UserId = UserId,
            IsValid = false,
            Description = transaction.Details,
            TransactionId = user.Transactions!.OrderByDescending(x=>x.Id).FirstOrDefault()!.Id
        };
        db.WithdrawalRequests.Add(withdrawalRequest);
        db.SaveChanges();
        return Ok("با موفقیت انجام شد");
    }
    private IActionResult AddTransaction(NewTransaction Transaction)
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
}