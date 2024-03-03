using System.Security.Claims;
using Core.Entities;

namespace Core.Interfaces;

public interface IBetHandler
{
    Task Bet(int id, double newBet, int TangerineId);

    Task HandleNewBet(int id, double newBet, int tangerineId);

    Task HandleOverridingBet(Bet bet, int tangerineId, double newBet, int id);
}