using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Fiit_passport.Models.Interfaces;

namespace Fiit_passport.Models;

[Table("authenticated_users")]
public class AuthenticatedUsers : IAuthenticatedUsers
{
    [Key]
    [Required]
    [Column("user_telegram_tag")]
    [StringLength(33, MinimumLength = 2,
        ErrorMessage = "Длина имени пользователя telegram должна быть от 2 до 33 символов")]
    [RegularExpression(@"@[A-Za-z0-9]+",
        ErrorMessage = "Имя пользователя telegram должно начинаться с @ и содержать только" +
                       "буквы и цифры латинского алфавита")]
    [NotNull]
    //[Remote(action: "CheckEmail", controller: "Home", ErrorMessage ="Email уже используется")]
    public string? TelegramTag { get; set; }

    public AuthenticatedUsers(string telegramTag)
    {
        TelegramTag = telegramTag;
    }
}