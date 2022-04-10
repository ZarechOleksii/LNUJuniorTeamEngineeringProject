using Data;
using Microsoft.EntityFrameworkCore;
using Models;
using Npgsql;
using Serilog;
using Serilog.Events;
using Services.Implementations;
using Services.Interfaces;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();

Log.Information("Starting up");
Log.Information("Glory to Ukraine!");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Seq("http://localhost:5341")
        .ReadFrom.Configuration(ctx.Configuration));

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddScoped<IBaseEntityService, BaseEntityService>();
    builder.Services.AddScoped<IRepository<BaseEntity>, BaseRepository<BaseEntity>>();

    // Database context
    var connectionString = GetHerokuConString();

    if (connectionString is null)
    {
        builder.Configuration.AddJsonFile("appsettings.Local.json");
        connectionString = builder.Configuration.GetConnectionString("DATABASE_URL");
    }

    builder.Services.AddDbContext<ApplicationContext>(x => x.UseNpgsql(connectionString));

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsProduction())
    {
        app.UseExceptionHandler("/Home/Error");

        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}

finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}

static string? GetHerokuConString()
{
    var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

    if (databaseUrl is null)
    {
        return null;
    }

    var databaseUri = new Uri(databaseUrl);
    var userInfo = databaseUri.UserInfo.Split(':');

    var builder = new NpgsqlConnectionStringBuilder
    {
        Host = databaseUri.Host,
        Port = databaseUri.Port,
        Username = userInfo[0],
        Password = userInfo[1],
        Database = databaseUri.LocalPath.TrimStart('/')
    };

    return builder.ToString();
}
