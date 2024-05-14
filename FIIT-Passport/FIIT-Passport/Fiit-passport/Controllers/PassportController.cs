using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Fiit_passport.Models;
using Fiit_passport.TelegramBot;

namespace Fiit_passport.Controllers;

public class PassportController(TelegramDbContext repo, TelegramBot.TelegramBot botTools) : Controller
{
    public string CreateIdSession() => Guid.NewGuid().ToString();
    
    public async Task<IActionResult> AuthenticationUser(Passport passport)
    {
        var correctPassport = await repo.GetPassport(passport.SessionId);
        if (!await repo.CheckUser(passport.TelegramTag!))
            TempData["error"] = $"Наш бот ждет команды /start от пользователя {passport.TelegramTag}";
        else if (correctPassport!.AuthenticatedTelegramTag != passport.TelegramTag)
        {
            TempData["error"] = $"{passport.TelegramTag} нажмите подтвердить \"Подтвердить\"";
            await botTools.SendButton(await repo.GetUserId(passport.TelegramTag!), passport.SessionId!);
        }
        else
        {
            TempData["success"] = "Теперь всё готово к отправке проекта";
        }
        return await SaveAndRedirect("CheckRequest", passport, GlobalCheck);
    }

    private void AddCookie(string key, string value)
    {
        var options = new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddYears(10)
        };
        Response.Cookies.Append(key, value, options);
    }
    public string? GetCookie(string key) => Request.Cookies[key];
    
    public void DeleteCookie(string key) => Response.Cookies.Delete(key);

    public async Task<IActionResult> SaveAndRedirect(string pageName, Passport? passport, Func<Passport, bool> check)
    {
        if (passport is null)
            return RedirectToAction(pageName);
        if(check(passport))
        {
            await repo.UpdatePassport(passport);
        }
        return RedirectToAction(pageName, await repo.GetPassport(passport.SessionId));
    }
    
    public IActionResult HomePage() => View(GetCookie("idSession") is not null);
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AboutProject(bool isNewProject)
    {
        var idSession = GetCookie("idSession");
        if (!isNewProject && idSession is not null && await repo.CheckPassport(idSession))
            return View("AboutProject", await repo.GetPassport(idSession));
        idSession = CreateIdSession();
        DeleteCookie("idSession");
        AddCookie("idSession", idSession);
        await repo.CreatePassport(idSession);
        return View("AboutProject", await repo.GetPassport(idSession));
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AboutProjectToDetails(Passport passport) =>
        await SaveAndRedirect( "DetailsProject", passport, CheckAboutProject);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AboutProjectToHome(Passport passport) =>
        await SaveAndRedirect("HomePage", passport, CheckAboutProject);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DetailsProjectToAbout(Passport passport)
    {
        if (CheckDetailsProject(passport))
        {
            await repo.UpdatePassport(passport);
        }
        return await AboutProject(false);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DetailsProjectToOthers(Passport passport) =>
        await SaveAndRedirect("OthersProject", passport, CheckDetailsProject);
    
    public IActionResult DetailsProject(Passport passport) => View(passport);
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OthersProjectToDetails(Passport passport) =>
        await SaveAndRedirect("DetailsProject", passport, CheckOtherProject);
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OthersProjectToContacts(Passport passport) =>
        await SaveAndRedirect("Contacts", passport, CheckOtherProject);
    
    public IActionResult OthersProject(Passport passport) => View(passport);
    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ContactsToOthers(Passport passport) =>
        await SaveAndRedirect("OthersProject", passport, CheckContacts);
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ContactsToCheck(Passport passport) => 
        await SaveAndRedirect("CheckRequest", passport, CheckContacts);
    
    public IActionResult Contacts(Passport passport) => View(passport);
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckToContacts(Passport passport) =>
        await SaveAndRedirect("Contacts", passport, GlobalCheck);
    
    public IActionResult CheckRequest(Passport passport) => View(passport);
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckToRequest(Passport passport)
    {
        var correctPassport = await repo.GetPassport(passport.SessionId);
        if (passport.TelegramTag is null)
        {
            TempData["error"] = "Имя пользователя telegram не может быть пустым";
            return await SaveAndRedirect("CheckRequest", passport, GlobalCheck);
        }
        if (correctPassport!.AuthenticatedTelegramTag != passport.TelegramTag)
        {
            TempData["error"] = $"Сначала подтвердите свою личность для пользователя {passport.TelegramTag}";
            return await SaveAndRedirect("CheckRequest", passport, GlobalCheck);
        }
        DeleteCookie("idSession");
        await repo.CreateSessionNumber(passport.SessionId!);
        passport.Status = Status.SendToReview;
        return await SaveAndRedirect("RequestSend", passport, GlobalCheck);
    }

    public async Task<IActionResult> RequestSend(Passport passport)
    {
        var newPassport = await repo.GetPassport(passport.SessionId);
        if (newPassport!.Status == Status.Reviewed)
            return RedirectToAction("RequestChecked", newPassport);
        return View(newPassport);
    }

    public async Task<IActionResult> RequestChecked(Passport passport) => View(await repo.GetPassport(passport.SessionId));
    
    public IActionResult RequestToHome() => RedirectToAction("HomePage");
    
    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    public bool CheckAboutProject(Passport passport) => 
            !(passport.ProjectDescription is null || passport.ProjectDescription!.Length > 100000 || passport.ProjectDescription!.Length == 0 ||
            passport.ProjectName is null || passport.ProjectName!.Length > 100 || passport.ProjectName!.Length == 0 ||
            passport.OrdererName is null || passport.OrdererName!.Length > 100 || passport.OrdererName!.Length == 0);

    public bool CheckDetailsProject(Passport passport) =>
            !(passport.AcceptanceCriteria is null  || passport.AcceptanceCriteria!.Length > 100000 || passport.AcceptanceCriteria!.Length == 0 ||
            passport.Result is null || passport.Result!.Length > 100000 || passport.Result!.Length == 0 ||
            passport.Goal is null || passport.Goal!.Length > 100000 || passport.Goal!.Length == 0);

    public bool CheckOtherProject(Passport passport) =>
        !(passport.MeetingLocation is null || passport.MeetingLocation!.Length > 100 || passport.MeetingLocation!.Length == 0 ||
            !ApiTools.MeetingLocationRegex().IsMatch(passport.MeetingLocation) ||
            passport.CopiesNumber < 1 || passport.CopiesNumber > 5);

    public bool CheckContacts(Passport passport) =>
        !(passport.PhoneNumber is null || !ApiTools.PhoneRegex().IsMatch(passport.PhoneNumber!) ||
            passport.Email is null || !ApiTools.EmailRegex().IsMatch(passport.Email!) ||
            passport.TelegramTag is null || !ApiTools.TelegramTagRegex().IsMatch(passport.TelegramTag!) ||
            passport.TelegramTag!.Length > 33 ||
            passport.Surname is null || passport.Surname!.Length > 50 || !ApiTools.SurnameRegex().IsMatch(passport.Surname) ||
            passport.Name is null || passport.Name!.Length > 50 || !ApiTools.NameRegex().IsMatch(passport.Name));

    public bool GlobalCheck(Passport passport) =>
        CheckAboutProject(passport) && CheckDetailsProject(passport) && CheckOtherProject(passport) && CheckContacts(passport);
}