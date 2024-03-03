using Core.Models;

namespace Infrastructure.Repository.Interfaces;

public interface ITangerineRepository
{
    Task<bool> AddTangerine(Tangerine tangerine);
    Task<bool> ClearTangerines();
    Task<bool> AddTangerineList(List<Tangerine> tangerines);

    Task<bool> UpdateTangerinePrice(int tangId,double sum);

    Task<IReadOnlyList<Tangerine>> GetAllTangerines();

    Task<Tangerine?> GetTangerineById(int id);
}