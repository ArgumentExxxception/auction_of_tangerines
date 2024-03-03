using Core.Entities;
using Core.Interfaces;
using Hangfire;
using Infrastructure.Repository.Interfaces;

namespace Infrastructure.Services;

public class BetHandler: IBetHandler
{
    private readonly IBetRepository _betRepository;
    private readonly IClientRepository _clientRepository;
    private readonly ITangerineRepository _tangerineRepository;
    private readonly INotificationService _notificationService;

    public BetHandler(IBetRepository betRepository, IClientRepository clientRepository, ITangerineRepository tangerineRepository, INotificationService notificationService)
    {
        _betRepository = betRepository;
        _clientRepository = clientRepository;
        _tangerineRepository = tangerineRepository;
        _notificationService = notificationService;
    }

    public async Task Bet(int id, double newBet, int tangerineId)
    {
        var client = await _clientRepository.GetClientById(id);
        if (client != null && client.Balance < newBet)
        {
            throw new Exception();
        }

        var betByTangerineId = await _betRepository.GetBetByTangerineId(tangerineId);
        if (betByTangerineId != null)
            await HandleOverridingBet(betByTangerineId,tangerineId,newBet,id);
        else
            await HandleNewBet(id, newBet, tangerineId);
        
        var updateTangerine = await _tangerineRepository.UpdateTangerinePrice(tangerineId, newBet);
        var updateClient = await _clientRepository.UpdateClient(client, newBet);
        
        if (!updateTangerine && updateClient)
            throw new Exception();
    }

    public async Task HandleNewBet(int id, double newBet, int tangerineId)
    {
        var bet = new Bet()
        {
            Id = new int(),
            CurrentPrice = newBet,
            TangerineId = tangerineId,
            ClientId = id
        };
        var result = await _betRepository.BetAdd(bet);
        if (!result)
            throw new Exception("Что то пошло не так! Попробуйте снова");
    }

    public async Task HandleOverridingBet(Bet bet, int tangerineId, double newBet, int id)
    {
        var oldBetClient = await _clientRepository.GetClientById(bet.ClientId);
        BackgroundJob.Enqueue(() =>
            _notificationService.SendNewBetNotification(tangerineId, oldBetClient!.Email, oldBetClient.Username));
        await _betRepository.UpdateBet(newBet, bet, id);
    }
}