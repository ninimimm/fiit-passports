using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Fiit_passport.Models.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiit_passport.Models;

[Table("passports")]
public class Passport : IPassport
{
    [Key]
    [Required]
    [StringLength(36, ErrorMessage = "Длина сессии должна быть 10 символов")]
    [Column("session_id")]
    public string? SessionId { get; set; }
    
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Название организации и заказчика должны быть от 1 до 100 символов")]
    [Column("orderer_name")]
    public string? OrdererName { get; set; }
    
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Название проекта должно быть от 1 до 100 символов")]
    [Column("project_name")]
    public string? ProjectName { get; set; }
    
    [MinLength(1, ErrorMessage = "Описание не может быть пустым")]
    [Column("project_description")]
    public string? ProjectDescription { get; set; }
    
    [MinLength(1, ErrorMessage = "Продукт не может быть без цели 🤨")]
    [Column("goal")]
    public string? Goal { get; set; }
    
    [MinLength(1, ErrorMessage = "Описание результата продукта не может быть пустым")]
    [Column("result")]
    public string? Result { get; set; }
    
    [MinLength(1, ErrorMessage = "Критерии приемки продукта не могут быть пустыми")]
    [Column("error_message")]
    public string? AcceptanceCriteria { get; set; }
    
    [Range(1, 5, ErrorMessage = "Недопустимое количество команд")]
    [Column("copies_number")]
    public int CopiesNumber { get; set; }
    
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
    
    public virtual ConnectSession? ConnectSession { get; set; }

    #region ConstructorForTable
    public Passport (string sessionId, string ordererName, string projectName, string projectDescription, string goal, string result, string acceptanceCriteria, int copiesNumber, string meetingLocation, string name, string surname, string email, string phoneNumber)
    {
        SessionId = sessionId;
        OrdererName = ordererName;
        ProjectName = projectName;
        ProjectDescription = projectDescription;
        Goal = goal;
        Result = result;
        AcceptanceCriteria = acceptanceCriteria;
        CopiesNumber = copiesNumber;
        MeetingLocation = meetingLocation;
        Name = name;
        Surname = surname;
        Email = email;
        PhoneNumber = phoneNumber;
    }
    #endregion

    public Passport() { }
    
    public Passport(string sessionId)
    {
        SessionId = sessionId;
    }
    
    public Passport(Dictionary<string, object> data) => Update(data);
    
    public Passport Update(Dictionary<string, object> data)
    {
        var type = GetType();
        foreach (var kvp in data)
        {
            var prop = type.GetProperty(kvp.Key);
            if (prop != null && prop.CanWrite)
                prop.SetValue(this, kvp.Value);
        }
        return this;
    }
}