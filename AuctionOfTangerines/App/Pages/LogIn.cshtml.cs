using Core.DTO;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuctionOfTangerines.Pages;

public class LogIn : PageModel
{
    private readonly IAuthenticationService _authenticationService;

    public LogIn(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public void OnGet()
    {
        
    }

    public async Task OnPostAsync(string email,string password)
    {
        ClientDto dto = new ClientDto()
        {
            Email = email,
            Password = password
        };
        try
        {
            await _authenticationService.AuthClient(dto);
        }
        catch (Exception e)
        {
            throw new Exception();
        }
    }
}