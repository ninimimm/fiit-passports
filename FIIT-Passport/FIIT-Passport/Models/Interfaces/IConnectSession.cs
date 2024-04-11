namespace Fiit_passport.Models;

public interface IConnectSession
{
    public string TelegramTag { get; set; }
    public string SessionId { get; set; }
}