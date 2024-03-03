using Core.Models;

namespace Core.Interfaces;

public interface INotificationService
{
    Task SendWinEmailNotification();

    Task SendNewBetNotification(int tangId,string address, string username);
}