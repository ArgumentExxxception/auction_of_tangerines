using System.Collections;

namespace Core.Entities;

public class Client
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public double Balance { get; set; }
    
    public bool ValidatePassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password,Password);
    }
    
    public List<Bet> Bets { get; set; }
}