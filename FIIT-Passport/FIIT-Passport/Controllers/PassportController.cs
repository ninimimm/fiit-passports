using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Fiit_passport.Models;
using Fiit_passport.TelegramBot;

namespace Fiit_passport.Controllers;

public class PassportController(TelegramDbContext repo, TelegramBot.TelegramBot botTools) : Controller
{
    public string CreateIdSession() => Guid.NewGuid().ToString();

    [HttpPost]
    public async Task<IActionResult> AuthenticationUser(Passport passport)
    {
        await botTools.AuthenticationUser(passport.TelegramTag!, TempData);
        return RedirectToAction("UpdatePassport");
    }
        
    
    public async Task<IActionResult> UpdatePassport()
    {
        var idSession = CreateIdSession();
        await repo.CreatePassport(idSession);
        if (!await repo.CheckPassport(idSession))
            return NotFound();
        return View(await repo.GetPassport(idSession));
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePassport1 (Passport passport)
    {
        await repo.UpdatePassport(passport);
        return RedirectToAction("");
    }
    
    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}