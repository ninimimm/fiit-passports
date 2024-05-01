using System.Text.RegularExpressions;
using Fiit_passport.Models;
using Fiit_passport.TelegramBot;
using Microsoft.AspNetCore.Mvc;
using static System.Text.RegularExpressions.Regex;

namespace Fiit_passport.Controllers;

[Route("api/passport")]
[ApiController]
public class ApiController(TelegramDbContext repo, TelegramBot.TelegramBot botTools) : ControllerBase
{
    [HttpPost("update")]
    public async Task Update([FromBody] Dictionary<string, string> request)
    {
        var passport = new Passport().UpdateByDictionary(request);
        if (ApiTools.CheckValidity(passport))
        {
            await repo.UpdatePassport(passport);
        }
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] Dictionary<string, string> request)
    {
        var passport = new Passport().UpdateByDictionary(request);
        var response = new Dictionary<string, string>();
        var correctPassport = await repo.GetPassport(passport.SessionId);
        if (!await repo.CheckUser(passport.TelegramTag!))
            response["state"] = $"Наш бот ждет команды /start от пользователя {passport.TelegramTag}";
        else if (correctPassport!.AuthenticatedTelegramTag != passport.TelegramTag)
        {
            await botTools.SendButton(await repo.GetUserId(passport.TelegramTag!), passport.SessionId!);
            response["state"] = $"{passport.TelegramTag} нажмите подтвердить \"Подтвердить\"";
        }
        else
            response["state"] = "Теперь всё готово к отправке проекта";
        if (ApiTools.CheckValidity(passport))
        {
            await repo.UpdatePassport(passport);
        }
        return Ok(response);
    }
    
    [HttpPost("confirm")]
    public async Task<IActionResult> Confirm([FromBody] Dictionary<string, string> request)
    {
        var passport = new Passport().UpdateByDictionary(request);
        var correctPassport = await repo.GetPassport(passport.SessionId);
        var response = new Dictionary<string, string>
        {
            { "state" , "Ok" }
        };
        if (passport.TelegramTag == "")
        {
            response["state"] = "Имя пользователя telegram не может быть пустым";
        }
        else if (correctPassport!.AuthenticatedTelegramTag != passport.TelegramTag)
        {
            response["state"] = $"Сначала подтвердите свою личность для пользователя {passport.TelegramTag}";
        }
        passport.Status = Status.SendToReview;
        if (ApiTools.CheckValidity(passport))
        {
            await repo.UpdatePassport(passport);
        }
        else
        {
            response["state"] = "Не все поля паспорта валидны";
        }
        return Ok(response);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create()
    {
        var sessionId = Guid.NewGuid().ToString();
        await repo.CreatePassport(sessionId);
        var json = new Dictionary<string, string>
        {
            { "SessionId", sessionId }
        };
        return Ok(json);
    }

    [HttpPost("get")]
    public async Task<IActionResult> Get([FromBody] Dictionary<string, string> request)
    {
        var sessionId = request["sessionId"];
        var passport = await repo.GetPassport(sessionId);
        return Ok(passport);
    }
}

public static partial class ApiTools
{
    public static bool CheckValidity(Passport passport)
    {
        if (passport.Name is not null)
        {
            if (passport.PhoneNumber == "" || !PhoneRegex().IsMatch(passport.PhoneNumber) ||
                passport.Email == "" || !EmailRegex().IsMatch(passport.Email) ||
                passport.TelegramTag == "" || !TelegramTagRegex().IsMatch(passport.TelegramTag) ||
                passport.TelegramTag!.Length > 33 ||
                passport.Surname == "" || passport.Surname!.Length > 50 || !SurnameRegex().IsMatch(passport.Surname) ||
                passport.Name == "" || passport.Name!.Length > 50 || !NameRegex().IsMatch(passport.Name))
            {
                return false;
            }
        }
        if (passport.MeetingLocation is not null)
        {
            if (passport.MeetingLocation == "" || passport.MeetingLocation!.Length > 100 ||
                !MeetingLocationRegex().IsMatch(passport.MeetingLocation) ||
                passport.CopiesNumber < 1 || passport.CopiesNumber > 5)
            {
                return false;
            }
        }
        if (passport.AcceptanceCriteria is not null)
        {
            if (passport.AcceptanceCriteria == "" || passport.AcceptanceCriteria!.Length > 100000 ||
                passport.Result == "" || passport.Result!.Length > 100000 ||
                passport.Goal == "" || passport.Goal!.Length > 100000)
            {
                return false;
            }
        }
        if (passport.ProjectDescription is not null)
        {
            if (passport.ProjectDescription == "" || passport.ProjectDescription!.Length > 100000 ||
                passport.ProjectName == "" || passport.ProjectName!.Length > 100 ||
                passport.OrdererName == "" || passport.OrdererName!.Length > 100)
            {
                return false;
            }
        }
        if (passport.SessionId.Length != 36)
        {
            return false;
        }
        return true;
    }
    
    [GeneratedRegex(@"^[A-Za-zА-Яа-я]+$")]
    private static partial Regex NameRegex();
    
    [GeneratedRegex(@"^[а-яa-zA-ZА-Я]+$")]
    private static partial Regex SurnameRegex();
    
    [GeneratedRegex(@"^\@[0-9a-zA-Z_]+$")]
    private static partial Regex TelegramTagRegex();
    
    [GeneratedRegex(@"^\+\d \(\d{3}\) \d{3}-\d{2}-\d{2}$")]
    private static partial Regex PhoneRegex();
    
    [GeneratedRegex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
    private static partial Regex EmailRegex();

    [GeneratedRegex(@"^[а-яa-zA-ZА-Я0-9, .:;'""/\\()]+$")]
    private static partial Regex MeetingLocationRegex();
}
