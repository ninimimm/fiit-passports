using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Fiit_passport.Models.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiit_passport.Models;

public enum Status
{
    Prepare,
    SendToReview,
    Reviewing,
    Reviewed,
    SendToCommands,
    Accept,
    Reject
}

[Table("passports")]
public class Passport : IPassport
{
    [Key]
    [Required]
    [StringLength(36, ErrorMessage = "Длина сессии должна быть 10 символов")]
    [Column("session_id")]
    public string? SessionId { get; set; }
    
    // [Required(ErrorMessage = "Назавнаие организации не может быть пустым")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Название организации и заказчика должны быть от 1 до 100 символов")]
    [Column("orderer_name")]
    public string? OrdererName { get; set; }
    
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Название проекта должно быть от 1 до 100 символов")]
    [Column("project_name")]
    public string? ProjectName { get; set; }
    
    [MinLength(1, ErrorMessage = "Описание не может быть пустым")]
    [MaxLength(100000, ErrorMessage = "Слишком большое сообщение")]
    [Column("project_description")]
    public string? ProjectDescription { get; set; }
    
    [MinLength(1, ErrorMessage = "Продукт не может быть без цели 🤨")]
    [MaxLength(100000, ErrorMessage = "Слишком большое сообщение")]
    [Column("goal")]
    public string? Goal { get; set; }
    
    [MinLength(1, ErrorMessage = "Описание результата продукта не может быть пустым")]
    [MaxLength(100000, ErrorMessage = "Слишком большое сообщение")]
    [Column("result")]
    public string? Result { get; set; }
    
    [MinLength(1, ErrorMessage = "Критерии приемки продукта не могут быть пустыми")]
    [MaxLength(100000, ErrorMessage = "Слишком большое сообщение")]
    [Column("error_message")]
    public string? AcceptanceCriteria { get; set; }

    [Range(1, 5, ErrorMessage = "Недопустимое количество команд")]
    [Column("copies_number")]
    [DefaultValue(1)]
    public int CopiesNumber { get; set; } = 1;
    
    [MinLength(1, ErrorMessage = "Место встречи не может быть пустым")]
    [Column("meeting_location")]
    [MaxLength(100)]
    public string? MeetingLocation { get; set; }
    
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина строки должна быть от 1 до 50 символов")]
    [Column("name")]
    public string? Name { get; set; }
    
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина строки должна быть от 1 до 50 символов")]
    [Column("surname")]
    public string? Surname { get; set; }
    
    // [Required(ErrorMessage = "Имя пользователя telegram не может быть пустым")]
    [Column("telegram_tag")]
    [StringLength(33, MinimumLength = 2,
        ErrorMessage = "Длина имени пользователя telegram должна быть от 2 до 33 символов")]
    [RegularExpression(@"@[A-Za-z0-9]+",
        ErrorMessage = "Имя пользователя telegram должно начинаться с @ и содержать только" +
                       "буквы и цифры латинского алфавита")] 
    //[Remote(action: "CheckEmail", controller: "Home", ErrorMessage ="Email уже используется")]
    public string? TelegramTag { get; set; }

    [Column("authenticated_telegram_tag")]
    public string? AuthenticatedTelegramTag { get; set; }
    
    [EmailAddress (ErrorMessage = "Некорректный адрес")]
    [DefaultValue("Не указана")]
    [Column("email")]
    [MaxLength(1000)]
    public string? Email { get; set; }
    
    [Phone(ErrorMessage = "Некорректный номер")]
    [DefaultValue("Не указан")]
    [Column("phone_number")]
    [MaxLength(50)]
    public string? PhoneNumber { get; set; }
    
    [Column("status")]
    [DefaultValue(Status.Prepare)]
    public Status Status { get; set; }
    
    public Passport() { }
    
    public Passport(string? sessionId)
    {
        SessionId = sessionId;
    }

    public void Update(Passport passport)
    {
        OrdererName = passport.OrdererName ?? OrdererName;
        ProjectName = passport.ProjectName ?? ProjectName;
        ProjectDescription = passport.ProjectDescription ?? ProjectDescription;
        Goal = passport.Goal ?? Goal;
        Result = passport.Result ?? Result;
        AcceptanceCriteria = passport.AcceptanceCriteria ?? AcceptanceCriteria;
        if (passport.CopiesNumber > 1)
            CopiesNumber = passport.CopiesNumber;
        MeetingLocation = passport.MeetingLocation ?? MeetingLocation;
        Name = passport.Name ?? Name;
        Surname = passport.Surname ?? Surname;
        TelegramTag = passport.TelegramTag ?? TelegramTag;
        AuthenticatedTelegramTag = passport.AuthenticatedTelegramTag ?? AuthenticatedTelegramTag;
        Email = passport.Email ?? Email;
        PhoneNumber = passport.PhoneNumber ?? PhoneNumber;
        Status = passport.Status;
    }

    public Passport UpdateByDictionary(Dictionary<string, string> properties)
    {
        var passportProperties = GetType().GetProperties();
        foreach (var passportProperty in passportProperties)
        {
            if (!properties.TryGetValue(passportProperty.Name, out var value))
                continue;
            passportProperty.SetValue(this, passportProperty.Name != "CopiesNumber" ? value : int.Parse(value));
        }
        return this;
    }
}