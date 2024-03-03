using System.Security.Claims;
using Core.DTO;

namespace Core.Interfaces;

public interface IAuthenticationService
{
    Task AuthClient(ClientDto clientDto);

    Task<string?> GetUserId(ClaimsPrincipal claimsPrincipal);
}