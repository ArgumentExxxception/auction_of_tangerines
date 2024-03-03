using System.Security.Claims;
using Core.Entities;
using Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuctionOfTangerines.Pages;

public class UserPage : PageModel
{
    protected IClientRepository ClientRepository { get; set; }
    
    public UserPage(IClientRepository clientRepository)
    {
        ClientRepository = clientRepository;
    }

    public Client? Client { get; set; }
    public async Task OnGet()
    {
        var userId = int.Parse(User.Claims.First().Value);
        var client = await ClientRepository.GetClientById(userId);
        Client = client;
    }

    public async Task OnPostAsync(int sum)
    {
        if (sum <= 0)
        {
            throw new Exception();
        }
        //TODO логика пополнения баланса
    }
}