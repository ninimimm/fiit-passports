using System.Text.RegularExpressions;
using Fiit_passport.Models;
using Fiit_passport.TelegramBot;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Fiit_passport.Controllers;

[Route("api")]
[ApiController]
public class ApiController(TelegramDbContext repo, TelegramBot.TelegramBot botTools) : ControllerBase
{
    [HttpPost("update/passport")]
    public async Task UpdatePassport([FromBody] Dictionary<string, string> request)
    {
        var passport = new Passport().UpdateByDictionary(request);
        if (ApiTools.CheckValidity(passport))
            await repo.UpdatePassport(passport);
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] Dictionary<string, string> request)
    {
        var passport = new Passport().UpdateByDictionary(request);
        var response = new Dictionary<string, string>();
        var correctPassport = await repo.GetPassport(passport.SessionId);
        if (!await repo.CheckUser(passport.TelegramTag!))
        {
            response["state"] = "error";
            response["message"] = $"Наш бот ждет команды /start от пользователя {passport.TelegramTag}";
        }
        else if (correctPassport!.AuthenticatedTelegramTag != passport.TelegramTag)
        {
            await botTools.SendButton(await repo.GetUserId(passport.TelegramTag!), passport.SessionId!);
            response["state"] = "error";
            response["message"] = $"{passport.TelegramTag} нажмите \"Подтвердить\"";
        }
        else
        {
            response["state"] = "success";
            response["message"] = "Теперь всё готово к отправке проекта";
        }
        if (ApiTools.CheckValidity(passport))
            await repo.UpdatePassport(passport);
        return Ok(response);
    }
    
    [HttpPost("confirm/passport")]
    public async Task<IActionResult> ConfirmPassport([FromBody] Dictionary<string, string> request)
    {
        var passport = new Passport().UpdateByDictionary(request);
        var correctPassport = await repo.GetPassport(passport.SessionId);
        var response = new Dictionary<string, string>
        {
            { "state" , "success" },
            { "message", "Ok" }
        };
        if (passport.TelegramTag == "")
        {
            response["state"] = "error";
            response["message"] = "Имя пользователя telegram не может быть пустым";
            return Ok(response);
        }
        if (correctPassport!.AuthenticatedTelegramTag != passport.TelegramTag)
        {
            response["state"] = "error";
            response["message"] = $"Сначала подтвердите свою личность для пользователя {passport.TelegramTag}";
            return Ok(response);
        }
        if (ApiTools.CheckValidity(passport))
            await repo.UpdatePassport(passport);
        else
        {
            response["state"] = "error";
            response["message"] = "Не все поля паспорта валидны";
            return Ok(response);
        }
        passport.Status = Status.SendToReview;
        await repo.CreateSessionNumber(passport.SessionId!, passport.ProjectName!);
        return Ok(response);
    }

    [HttpPost("create/passport")]
    public async Task<IActionResult> CreatePassport()
    {
        var sessionId = Guid.NewGuid().ToString();
        await repo.CreatePassport(sessionId);
        var json = new Dictionary<string, string>
        {
            { "SessionId", sessionId }
        };
        return Ok(json);
    }

    [HttpPost("get/passport")]
    public async Task<IActionResult> GetPassport([FromBody] Dictionary<string, string> request)
    {
        var sessionId = request["sessionId"];
        var passport = await repo.GetPassport(sessionId);
        return Ok(passport);
    }
    
    [HttpPost("get/passports")]
    public async Task<IActionResult> GetPassports([FromBody] Dictionary<string, string> request)
    {
        // var sessionId = request["sessionId"];
        // var passport = await repo.GetPassport(sessionId);
        var passports = await repo.GetPassports(request["auth"]);
        return Ok(passports);
    }
    
    [HttpPost("update/numbers")]
    public async Task UpdateNumber([FromBody] Dictionary<string, Dictionary<string, string>> request)
    {
        foreach (var sessionNumber in request)
            await repo.UpdateSessionNumber(sessionNumber.Key, int.Parse(sessionNumber.Value["number"]),
                (Status)int.Parse(sessionNumber.Value["status"]), sessionNumber.Value["name"]);
    }

    [HttpPost("get/numbers")]
    public async Task<IActionResult> GetNumber()
    {
        var sessionNumbers = await repo.GetAllSessionNumbers();
        return Ok(sessionNumbers);
    }
    
    [HttpPost("create/comment")]
    public async Task<IActionResult> CreateComment([FromBody] Dictionary<string, string> request) =>
        Ok(await repo.CreateComment(request["sessionId"], request["fieldName"], 
            int.Parse(request["start"]), int.Parse(request["end"]), request["text"]));
    
    [HttpPost("get/comment")]
    public async Task<IActionResult> GetComments([FromBody] Dictionary<string, string> request) =>
        Ok(await repo.GetCommentsBySessionId(request["sessionId"]));

    [HttpPost("update/comment")]
    public async Task<IActionResult> UpdateComment([FromBody] Dictionary<string, string> request)
    {
        await repo.UpdateComment(int.Parse(request["id"]), request["text"]);
        return Ok();
    }

    [HttpPost("delete/comment")]
    public async Task<IActionResult> DeleteComment([FromBody] Dictionary<string, string> request)
    {
        await repo.DeleteComment(int.Parse(request["id"]));
        return Ok();
    }
}

public static partial class ApiTools
{
    public static bool CheckValidity(Passport passport)
    {
        if (passport.Name is not null && (passport.PhoneNumber == "" || !PhoneRegex().IsMatch(passport.PhoneNumber!) ||
            passport.Email == "" || !EmailRegex().IsMatch(passport.Email!) ||
            passport.TelegramTag == "" || !TelegramTagRegex().IsMatch(passport.TelegramTag!) ||
            passport.TelegramTag!.Length > 33 ||
            passport.Surname == "" || passport.Surname!.Length > 50 || !SurnameRegex().IsMatch(passport.Surname) ||
            passport.Name == "" || passport.Name!.Length > 50 || !NameRegex().IsMatch(passport.Name)))
            return false;
        if (passport.MeetingLocation is not null && (passport.MeetingLocation == "" || passport.MeetingLocation!.Length > 100 ||
            !MeetingLocationRegex().IsMatch(passport.MeetingLocation) ||
            passport.CopiesNumber < 1 || passport.CopiesNumber > 5))
            return false;
        if (passport.AcceptanceCriteria is not null && (passport.AcceptanceCriteria == "" || passport.AcceptanceCriteria!.Length > 100000 ||
            passport.Result == "" || passport.Result!.Length > 100000 ||
            passport.Goal == "" || passport.Goal!.Length > 100000))
            return false;
        if (passport.ProjectDescription is not null && (passport.ProjectDescription == "" || passport.ProjectDescription!.Length > 100000 ||
            passport.ProjectName == "" || passport.ProjectName!.Length > 100 ||
            passport.OrdererName == "" || passport.OrdererName!.Length > 100))
            return false;
        return passport.SessionId!.Length == 36;
    }
    
    [GeneratedRegex(@"^[A-Za-zА-Яа-я]+$")]
    public static partial Regex NameRegex();
    
    [GeneratedRegex(@"^[а-яa-zA-ZА-Я]+$")]
    public static partial Regex SurnameRegex();
    
    [GeneratedRegex(@"^\@[0-9a-zA-Z_]+$")]
    public static partial Regex TelegramTagRegex();
    
    [GeneratedRegex(@"^\+\d \(\d{3}\) \d{3}-\d{2}-\d{2}$")]
    public static partial Regex PhoneRegex();
    
    [GeneratedRegex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")]
    public static partial Regex EmailRegex();

    [GeneratedRegex(@"^[а-яa-zA-ZА-Я0-9, .:;'""/\\()]+$")]
    public static partial Regex MeetingLocationRegex();
}