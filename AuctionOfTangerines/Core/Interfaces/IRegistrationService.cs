using Core.DTO;

namespace Core.Interfaces;

public interface IRegistrationService
{
    Task<bool> RegisterClient(RegisterClientDto dto);
}