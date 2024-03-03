using Core.Models;

namespace Core.Entities;

public class Bet
{
    public int Id { get; set; }
    public int TangerineId { get; set; }
    public  int ClientId{ get; set; }
    public double CurrentPrice { get; set; }
    
    public Client Client { get; set; }
    
    public Tangerine Tangerine { get; set; } 
    
}