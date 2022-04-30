using Data;
using Data.MovieRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Entities;
using Npgsql;
using Serilog;
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
    builder.Services.AddScoped<IMailService, MailService>();
    builder.Services.AddScoped<IMovieService, MovieService>();
    builder.Services.AddScoped<IFavouriteService, FavouriteService>();
    builder.Services.AddScoped<ICommentService, CommentService>();
    builder.Services.AddScoped<IRepository<BaseEntity>, BaseRepository<BaseEntity>>();
    builder.Services.AddScoped<IMovieRepository, MovieRepository>();
    builder.Services.AddScoped<IFavouriteRepository, FavouriteRepository>();
    builder.Services.AddScoped<IRepository<Comment>, BaseRepository<Comment>>();

    // Database context
    var connectionString = GetHerokuConString();

    if (connectionString is null)
    {
        builder.Configuration.AddJsonFile("appsettings.Local.json");
        connectionString = builder.Configuration.GetConnectionString("DATABASE_URL");
    }

    builder.Services.AddDbContext<ApplicationContext>(x => x.UseNpgsql(connectionString));
    builder.Services.AddIdentity<User, IdentityRole>(option =>
        {
            option.Password.RequiredLength = 8;
            option.Password.RequireNonAlphanumeric = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationContext>()
        .AddDefaultTokenProviders();

    var app = builder.Build();

    // Roles initializing
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await RoleInitializer.InitializeAsync(userManager, rolesManager);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsProduction())
    {
        app.UseExceptionHandler("/Home/Error");

        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseAuthentication();
    app.UseAuthorization();
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
