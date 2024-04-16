using System.Diagnostics;
using Fiit_passport.Database;
using Microsoft.AspNetCore.Mvc;
using Fiit_passport.Models;
using Fiit_passport.TelegramBot;

namespace Fiit_passport.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
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