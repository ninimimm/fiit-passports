using System.Security.Cryptography.X509Certificates;
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
    new TelegramBotClient(provider.GetRequiredService<IConfiguration>().GetSection("TelegramSecrets")["Token"]!));


var app = builder.Build();
var host = new WebHostBuilder()
    .UseContentRoot(Directory.GetCurrentDirectory())
    .UseKestrel(options =>
    {
        var certPath = Path.Combine(Directory.GetCurrentDirectory(), "certs", "fullchain.pem");
        var keyPath = Path.Combine(Directory.GetCurrentDirectory(), "certs", "privkey.pem");
        var certificate = X509Certificate2.CreateFromPemFile(certPath, keyPath);
        options.ListenAnyIP(8888, listenOptions =>
        {
            listenOptions.UseHttps(certificate);
        });
    })
    .ConfigureServices(services =>
    {
        services.AddCors();
        services.AddScoped<TelegramDbContext>();
        services.AddScoped<TelegramBot>();
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection")
        ));
        services.AddSingleton<ITelegramBotClient>(_ =>
            new TelegramBotClient("6599160966:AAEPk4mP04rI5jzHQJr65a4xlyHRQIUCygk"));
        services.AddControllers();
    })
    .Configure(applicationBuilder =>
    {
        if (applicationBuilder.ApplicationServices.GetService<IWebHostEnvironment>()!.IsDevelopment())
        {
            applicationBuilder.UseDeveloperExceptionPage();
        }
        applicationBuilder.UseCors(builder => 
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        applicationBuilder.UseRouting();
        applicationBuilder.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    })
    .Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Passport/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Passport}/{action=HomePage}/{id?}");


var telegramBot =  app.Services.CreateScope().ServiceProvider.GetRequiredService<TelegramBot>();
var bot = app.Services.GetRequiredService<ITelegramBotClient>();
bot.StartReceiving(new DefaultUpdateHandler(telegramBot.HandleUpdateAsync, TelegramBot.HandleErrorAsync));

var thread = new Thread(host.Run);
thread.Start();

// app.Run();
