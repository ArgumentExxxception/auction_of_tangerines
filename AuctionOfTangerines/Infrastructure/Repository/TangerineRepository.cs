using Core.Models;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class TangerineRepository: ITangerineRepository
{
    protected readonly Context _context;

    public TangerineRepository(Context context)
    {
        _context = context;
    }

    public async Task<bool> AddTangerine(Tangerine tangerine)
    {
        await _context.Tangerines.AddAsync(tangerine);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddTangerineList(List<Tangerine> tangerines)
    {
        await _context.AddRangeAsync(tangerines);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateTangerinePrice(int tangId, double sum)
    {
        var tang = await GetTangerineById(tangId);
        if (tang == null)
            return false;
        tang.CurrentPrice = sum;
        _context.Entry(tang).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Tangerine?> GetTangerineById(int id)
    {
        return await _context.Tangerines.FindAsync(id);
    }

    public async Task<IReadOnlyList<Tangerine>> GetAllTangerines()
    {
        return await _context.Tangerines.Take(_context.Tangerines.Count()).ToListAsync();
    }

    public async Task<bool> ClearTangerines()
    {
        _context.Tangerines.RemoveRange(_context.Tangerines);
        await _context.SaveChangesAsync();
        return true;
    }

}