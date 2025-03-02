using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("[Controller]/[Action]")]
public class CardController : ControllerBase
{
    private readonly Context db;
    public CardController(Context _db)
    {
        db = _db;
    }

    [HttpPost]
    public IActionResult AddCard(NewCard newCard)
    {
        db.Cards.Add(new Cards
        {
            CardBank = newCard.CardBank,
            CardName = newCard.CardName,
            CardNumber = newCard.CardNumber
        });
        db.SaveChanges();
        return Ok();
    }
    [HttpGet]
    public IActionResult ShowCards()
    {
        List<IncomingCard> results = new List<IncomingCard>();
        List<Cards> cards = db.Cards.ToList();
        foreach (Cards card in cards)
        {
            results.Add(new IncomingCard()
            {
                CardBank = card.CardBank,
                CardName = card.CardName,
                CardNumber = card.CardNumber,
                Id = card.Id
            });
        }
        return Ok(results);
    }
}