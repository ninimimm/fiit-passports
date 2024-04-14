using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Fiit_passport.Models;
using Fiit_passport.TelegramBot;

namespace Fiit_passport.Controllers;

public class PassportController : Controller
{
    private readonly TelegramDbContext _repo;
    private readonly TelegramBot.TelegramBot _telegramBot;
    
    public PassportController(TelegramDbContext repo, TelegramBot.TelegramBot botTools)
    {
        _telegramBot = botTools;
        _repo = repo;
    }
    

    public string CreateIdSession() => Guid.NewGuid().ToString();

    [HttpPost]
    public async Task AuthenticationUser(Passport passport)
    {
        await _telegramBot.AuthenticationUser(passport.TelegramTag);
    }
        
    
    public async Task<IActionResult> UpdatePassport()
    {
        var idSession = CreateIdSession();
        await _repo.CreatePassport(idSession);
        if (!await _repo.CheckPassport(idSession))
            return NotFound();
        Console.WriteLine("я тут");
        return View(await _repo.GetPassport(idSession));
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePassport1 (Passport passport)
    {
        Console.WriteLine("update");
        await _repo.UpdatePassport(passport);
        return RedirectToAction("");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}