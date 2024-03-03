using Core.DTO;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Repository.Interfaces;

namespace Infrastructure.Services;

public class RegistrationService: IRegistrationService
{
    protected readonly IClientRepository _clientRepository;
    public RegistrationService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<bool> RegisterClient(RegisterClientDto dto)
    {
        var clients = await _clientRepository.GetAllClients();
        if (clients.FirstOrDefault(u => u.Email == dto.Email)!= null)
        {
            throw new Exception("Пользователь существует");
        }

        var client = new Client()
        {
            Balance = 0,
            Email = dto.Email,
            Id = new int(),
            Password = dto.Password,
            Username = dto.Username
        };
        var result = await _clientRepository.AddClient(client);
        if (!result)
        {
            throw new Exception("Что то пошло не так");
        }

        return true;
    }
}