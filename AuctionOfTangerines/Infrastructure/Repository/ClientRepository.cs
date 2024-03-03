using Core.DTO;
using Core.Entities;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace Infrastructure.Repository;

public class ClientRepository: IClientRepository
{
    private readonly Context _dbContext;

    public ClientRepository(Context dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Client?> GetClientById(int id)
    {
        return await _dbContext.Clients.FindAsync(id);
    }

    public async Task<bool> AddClient(Client client)
    {
        client.Password = BCrypt.Net.BCrypt.HashPassword(client.Password);
        await _dbContext.Clients.AddAsync(client);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<Client>> GetAllClients()
    {
        var clients = _dbContext.Clients.Take(_dbContext.Set<Client>().Count()).ToList();
        return clients;
    }

    public async Task<bool> UpdateClient(Client client,double sum)
    {
        client.Balance -= sum;
        _dbContext.Entry(client).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return true;
    }
    
}