using Core.Entities;

namespace Infrastructure.Repository.Interfaces;

public interface IBetRepository
{ 
    Task<bool> BetAdd(Bet bet);
    Task<List<Bet?>> GetAllBets();
    Task<Bet?> GetBetById(int id);

    Task<Bet?> GetBetByTangerineId(int id);

    Task UpdateBet(double sum, Bet betByTangerineId,int id);
    Task DeleteBet();
}