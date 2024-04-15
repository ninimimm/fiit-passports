using Fiit_passport.Database;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Fiit_passport.TelegramBot;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<TelegramDbContext>();

builder.Services.AddScoped<TelegramBot>();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

builder.Services.AddSingleton<ITelegramBotClient>(provider =>
    new TelegramBotClient(provider.GetRequiredService<IConfiguration>().GetSection("TelegramSecrets")["Token"]!
));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


var telegramBot =  app.Services.CreateScope().ServiceProvider.GetRequiredService<TelegramBot>();
var bot = app.Services.GetRequiredService<ITelegramBotClient>();
bot.StartReceiving(new DefaultUpdateHandler(telegramBot.HandleUpdateAsync, TelegramBot.HandleErrorAsync));

app.Run();