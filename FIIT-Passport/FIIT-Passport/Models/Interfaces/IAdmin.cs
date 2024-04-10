using System.ComponentModel.DataAnnotations;

namespace Fiit_passport.Models;

public interface IAdmin
{
    public string TelegramTag { get; set; }
    public string AdminLink { get; set; }
}