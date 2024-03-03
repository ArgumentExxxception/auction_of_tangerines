using AuctionOfTangerines;
using Core.Interfaces;
using Core.Models;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure;
using Infrastructure.Repository;
using Infrastructure.Repository.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddScoped<IAuthenticationService,AuthenticationService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddEntityFrameworkNpgsql().
    AddDbContext<Context>(opt =>
    {
        opt.UseNpgsql(builder.Configuration.GetConnectionString("Db"));
        opt.UseNpgsql(n => n.MigrationsAssembly("App"));
    });
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt=>
        opt.LoginPath = "/Index");
builder.Services.AddAuthorization();
builder.Services.AddHostedService<AuctionWorker>();
builder.Services.AddScoped<IBetRepository, BetRepository>();
builder.Services.AddScoped<IBetHandler, BetHandler>();
builder.Services.AddScoped<ITangerineRepository,TangerineRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(
        builder.Configuration.GetConnectionString("HangfireConnection")));
builder.Services.AddHangfireServer();
builder.Services.Configure<MailSettings>(
    builder.Configuration.GetSection("MailSettings"));


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// app.UseHangfireDashboard("/dashboard");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseAuthorization();

app.MapRazorPages();

app.UseHangfireServer();

app.UseHangfireDashboard();

app.MapHangfireDashboard("/dashboard");

app.Run();