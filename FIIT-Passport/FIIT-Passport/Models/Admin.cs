using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiit_passport.Models;

[Table("admins")]
public class Admins : IAdmins
{
    [Key]
    [Required]
    [Column("telegramtag")]
    [MaxLength(33)]
    [MinLength(2)]
    public string TelegramTag { get; set; }
    [Required]
    [Column("adminlink")]
    [MaxLength(255)]
    public string AdminLink { get; set; }

    public Admins(string telegramTag, string adminLink)
    {
        TelegramTag = telegramTag;
        AdminLink = adminLink;
    }
}