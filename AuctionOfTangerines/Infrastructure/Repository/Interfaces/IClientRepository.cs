using Core.DTO;
using Core.Entities;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace Infrastructure.Repository.Interfaces;

public interface IClientRepository
{
    Task<Client?> GetClientById(int id);
    Task<bool> AddClient(Client client);
    Task<List<Client>> GetAllClients();

    Task<bool> UpdateClient(Client client, double sum);
}