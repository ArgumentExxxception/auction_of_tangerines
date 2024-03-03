using Core.DTO;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuctionOfTangerines.Pages;

public class SignIn : PageModel
{
    private readonly IRegistrationService _registrationService;

    public SignIn(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    public void OnGet()
    {
        
    }

    public async Task OnPostAsync(string email,string username,string password)
    {
        RegisterClientDto registerDto = new RegisterClientDto()
        {
            Email = email,
            Password = password,
            Username = username
        };
        try
        {
            await _registrationService.RegisterClient(registerDto);
        }
        catch (Exception ex)
        {
            //TODO log
        }
        
    }
}