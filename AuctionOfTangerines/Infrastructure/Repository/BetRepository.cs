using Core.Entities;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class BetRepository: IBetRepository
{
    public BetRepository(Context dbContext)
    {
        DbContext = dbContext;
    }

    public Context DbContext  { get; set; }
    
    public async Task<bool> BetAdd(Bet bet)
    {
        await DbContext.AddAsync(bet);
        await DbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<Bet?>> GetAllBets()
    {
        return await DbContext.Bets.Take(DbContext.Set<Bet>().Count()).ToListAsync();
    }

    public async Task<Bet?> GetBetById(int id)
    {
        var bet = await DbContext.Bets.FindAsync(id);
        if (bet!= null)
            return bet;
        return null;
    }

    public async Task<Bet?> GetBetByTangerineId(int id)
    {
        return await DbContext.Bets.FirstOrDefaultAsync(b=> b.TangerineId == id);
    }

    public async Task UpdateBet(double sum, Bet betByTangerineId, int id)
    {
        betByTangerineId.CurrentPrice = sum;
        betByTangerineId.ClientId = id;
        DbContext.Entry(betByTangerineId).State = EntityState.Modified;
        await DbContext.SaveChangesAsync();
    }

    public async Task DeleteBet()
    {
        DbContext.Bets.RemoveRange(DbContext.Bets.Take(await DbContext.Bets.CountAsync()));
    }
}