using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Repository.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuctionOfTangerines.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IAuthenticationService _authenticationService;
    private readonly ITangerineRepository _tangerineRepository;
    private readonly IBetHandler _betHandler;
    public IReadOnlyList<Tangerine> Tangerines { get; set; }
    public IndexModel(ILogger<IndexModel> logger, IBetHandler betHandler, IAuthenticationService authenticationService, ITangerineRepository tangerineRepository)
    {
        _logger = logger;
        _betHandler = betHandler;
        _authenticationService = authenticationService;
        _tangerineRepository = tangerineRepository;
    }

    public async Task OnGet()
    {
        Tangerines = await _tangerineRepository.GetAllTangerines();
    }

    public async Task OnPostAsync(double newSum,double oldSum,int tangId)
    {
        if (HttpContext.User.Identity is not { IsAuthenticated: true })
        {
            throw new UnauthorizedAccessException();
        }
        if (newSum<oldSum)
        {
            throw new Exception("");
        }
        
        var userId = await _authenticationService.GetUserId(HttpContext.User);
        if (userId != null) 
            await _betHandler.Bet(int.Parse(userId), newSum, tangId);
        Tangerines = await _tangerineRepository.GetAllTangerines();

    }
}