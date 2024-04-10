using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fiit_passport.Models;

[Table("connects")]
[PrimaryKey(nameof(TelegramTag), nameof(SessionId))]
public class Connect : IConnect
{
    [Required]
    [Column("telegram_tag")]
    [StringLength(33, MinimumLength = 2,
        ErrorMessage = "Длина имени пользователя telegram должна быть от 2 до 33 символов")]
    [RegularExpression(@"@[A-Za-z0-9]+",
        ErrorMessage = "Имя пользователя telegram должно начинаться с @ и содержать только" +
                       "буквы и цифры латинского алфавита")]
    //[Remote(action: "CheckEmail", controller: "Home", ErrorMessage ="Email уже используется")]
    public string TelegramTag { get; set; }
    
    [Required]
    [StringLength(10, ErrorMessage = "Длина сессии должна быть 10 символов")]
    [Column("session_id")]
    [ForeignKey(nameof(Passport))]
    public string SessionId { get; set; }

    public virtual Passport Passport { get; set; }

    public Connect(string telegramTag, string sessionId)
    {
        TelegramTag = telegramTag;
        SessionId = sessionId;
    }
}