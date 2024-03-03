using System.Net.Mail;
using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Infrastructure.Repository.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Infrastructure.Services;

public class NotificationService: INotificationService
{
    private readonly IBetRepository _betRepository;
    private readonly MailSettings _mailSettings;

    public NotificationService(IOptions<MailSettings> mailSettings, IBetRepository betRepository)
    {
        _betRepository = betRepository;
        _mailSettings = mailSettings.Value;
    }

    public async Task SendWinEmailNotification()
    {
        var winClientsInfo =  await GetWinClientsInfo();
        foreach (var info in winClientsInfo)
        {
            var mailModel = new MailModel()
            {
                Mail = info.Email,
                Username = info.Username
            };
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailModel.Mail));
            email.Subject = mailModel.Username;
            email.Body = new TextPart(TextFormat.Html) { Text = $"Вы выиграли мандарин {info.TangerineId}" };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port,SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSettings.Mail,_mailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
            
        }
    }

    public async Task SendNewBetNotification(int tangId,string address, string username)
    {
        var mailModel = new MailModel()
        {
            Mail = address,
            Username = username
        };
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
        email.To.Add(MailboxAddress.Parse(mailModel.Mail));
        email.Subject = mailModel.Username;
        email.Body = new TextPart(TextFormat.Html) { Text = $"Вашу ставку на мандарин Id{tangId} перебили!" };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port,SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_mailSettings.Mail,_mailSettings.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }

    private async Task<List<WinMailModel>?> GetWinClientsInfo()
    {
        var allBets = await _betRepository.GetAllBets();
        List<WinMailModel>? winMailModels = new ();
        foreach (var bet in allBets)
        {
            var winMailModel = new WinMailModel()
            {
                Email = bet.Client.Email,
                TangerineId = bet.TangerineId,
                Username = bet.Client.Username
            };
            winMailModels.Add(winMailModel);
        }
        if (winMailModels.Count > 0)
            return winMailModels;
        return null;
    }
}