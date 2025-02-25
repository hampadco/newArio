using System.Data.Common;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[Controller]/[Action]")]
public class TransactionController : ControllerBase
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
    public IActionResult ViewTransactions(int UserId)
    {
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
}