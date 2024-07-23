using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RateItBot.Configs;
using RateItBot.Domain;
using RateItBot.Domain.Repositories.Abstract;
using RateItBot.Domain.Repositories.EntityFrameWork;
using RateItBot.Managers;
using RateItBot.Services.TelegramBot.Abstract;
using RateItBot.Services.TelegramBot.Implementation;
using RateItBot.TelegramBot;
using Telegram.Bot;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RateItBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var telegramConfig = new TelegramConfig();
            var baseConfig = new BaseConfig();
            builder.Configuration.Bind("Telegram", telegramConfig);
            builder.Configuration.Bind("BaseConfig", baseConfig);

            builder.Services.AddSingleton<ITelegramBotClient>(provider =>
            {
                var botToken = TelegramConfig.Token;
                return new TelegramBotClient(botToken);
            });

            builder.Services.AddHostedService<TelegramBotBackgroundService>();

            builder.Services.AddTransient<ITelegramMessageHandler, RatingMessageHandler>();
            builder.Services.AddTransient<ITelegramMessageHandler, GetScoreMessageHandler>();

            // EntityFramework
            builder.Services.AddTransient<IUserRepository, EFUserRepository>();
            builder.Services.AddTransient<IRatingRepository, EFRatingRepository>();

            builder.Services.AddScoped<MessageHandlerManager>();
            builder.Services.AddScoped<UserManager>();
            builder.Services.AddScoped<RatingManager>();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(BaseConfig.ConnectionString, new MySqlServerVersion("8.0.0")));

            builder.Services.AddControllersWithViews();

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

            app.Run();
        }
    }
}