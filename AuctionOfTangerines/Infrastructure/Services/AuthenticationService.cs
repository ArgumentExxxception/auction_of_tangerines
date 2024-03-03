using System.Diagnostics;
using System.Security.Claims;
using Core.DTO;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using IAuthenticationService = Core.Interfaces.IAuthenticationService;

namespace Infrastructure.Services;

public class AuthenticationService: IAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IClientRepository _clientRepository;
    public AuthenticationService(IClientRepository clientRepository, IHttpContextAccessor httpContextAccessor)
    {
        _clientRepository = clientRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task AuthClient(ClientDto clientDto)
    {
        var clients =await _clientRepository.GetAllClients();
        var client = clients.FirstOrDefault(c => c.Email == clientDto.Email);
        if (client != null && client.ValidatePassword(clientDto.Password))
        {
            var claims = new List<Claim>()
            {
                new (ClaimTypes.NameIdentifier, client.Id.ToString()),
            };
            var claimsIdentity = new ClaimsIdentity(claims,"Cookies");
            var claimPrincipal = new ClaimsPrincipal(claimsIdentity);
            if (_httpContextAccessor.HttpContext != null)
                await _httpContextAccessor.HttpContext.SignInAsync(claimPrincipal);
        }
    }
    
    public async Task<string?> GetUserId(ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}