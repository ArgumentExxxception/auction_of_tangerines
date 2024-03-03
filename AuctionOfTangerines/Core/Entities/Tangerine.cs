using Core.Entities;

namespace Core.Models;

public class Tangerine
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double CurrentPrice { get; set; }

    public int BetId { get; set; }
    
    public List<Bet> Bets { get; set; }
}