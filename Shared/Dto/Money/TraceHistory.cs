public class TraceHistory
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int Amount { get; set; }
    public string Date { get; set; } = null!;
    public TransactionType type { get; set; }
}
public class Traces
{
    public int Balance { get; set; }
    public List<TraceHistory> TraceHistories { get; set; }
}