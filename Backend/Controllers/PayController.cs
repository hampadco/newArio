using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class PayController : Controller
{
    private readonly Context db;
    public PayController(Context _db)
    {
        db = _db;
    }
    public IActionResult UserReports(int UserId, int page = 1, int pageSize = 20, int? Type = null)
    {
        var user = db.Users.Include(x => x.Transactions).FirstOrDefault(x => x.Id == UserId);
        if (user == null)
        {
            TempData["Error"] = "کاربر پیدا نشد";
            return RedirectToAction("index", "home");
        }

        var q = user.Transactions!.OrderByDescending(x => x.Id).ToList();
        if (Type == 0) q = q.Where(x => x.Type == TransactionType.Deposit).ToList();
        else if (Type == 1) q = q.Where(x => x.Type == TransactionType.Withdrawal).ToList();
        var totalTransactions = q.Count;
        var pagedTransactions = q.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var pagination = new Pagination<Transaction>(pagedTransactions, totalTransactions, pageSize, page);
        ViewBag.pagination = pagination;

        ViewBag.walet = user.Balance;

        return View();
    }
    public IActionResult Index()
    {
        var q = db.DepositRequests.Include(x => x.Card).Include(x => x.User).ToList();
        var pagination = new Pagination<DepositRequest>(q, q.Count, q.Count, 1);
        ViewBag.pagination = pagination;
        return View();
    }
}