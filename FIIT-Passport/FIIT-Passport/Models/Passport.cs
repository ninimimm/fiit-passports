using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fiit_passport.Models;

public class Passport : IPassport
{
    [Key]
    [Required]
    [StringLength(10, ErrorMessage = "Длина сессии должна быть 10 символов")]
    public string SessionId { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Название организации и заказчика должны быть от 1 до 100 символов")]
    public string OrdererName { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Название проекта должно быть от 1 до 100 символов")]
    public string ProjectName { get; set; }
    
    [Required]
    [MinLength(1, ErrorMessage = "Описание не может быть пустым")]
    public string ProjectDescription { get; set; }
    
    [Required]
    [MinLength(1, ErrorMessage = "Продукт не может быть без цели 🤨")]
    public string Goal { get; set; }
    
    [Required]
    [MinLength(1, ErrorMessage = "Описание результата продукта не может быть пустым")]
    public string Result { get; set; }
    
    [Required]
    [MinLength(1, ErrorMessage = "Критерии приемки продукта не могут быть пустыми")]
    public string AcceptanceCriteria { get; set; }
    
    [Required]
    [Range(1, 5, ErrorMessage = "Недопустимое количество команд")]
    public int CopiesNumber { get; set; }
    
    [Required]
    [MinLength(1, ErrorMessage = "Место встречи не может быть пустым")]
    public string MeetingLocation { get; set; }
    
    [Required]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина строки должна быть от 1 до 50 символов")]
    public string Name { get; set; }
    
    [Required]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина строки должна быть от 1 до 50 символов")]
    public string Surname { get; set; }
    
    [EmailAddress (ErrorMessage = "Некорректный адрес")]
    [DefaultValue("Не указана")]
    public string Email { get; set; }
    
    [Phone(ErrorMessage = "Некорректный номер")]
    [DefaultValue("Не указан")]
    public string PhoneNumber { get; set; }
    
    
}