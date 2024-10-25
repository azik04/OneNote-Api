using BLL.Services.Implementations;
using BLL.Services.Interfaces;
using DAL.Repositories.Folder;
using DAL.Repositories.Note;
using DAL.Repositories.User;
using DAL.Repositories;
using Domain.Models;
using DAL.Repositories.Section;
using DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Domain.Enum;

namespace OneNote_Api;

public static class Initializer
{
    //Initialize Services
    public static void InitializeRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBaseRepository<Users>, UserRepository>();
        services.AddScoped<IBaseRepository<Folders>, FolderRepository>();
        services.AddScoped<IBaseRepository<Notes>, NoteRepository>();
        services.AddScoped<IBaseRepository<Sections>, SectionRepository>();
    }


    //Initialize Repositories
    public static void InitializeServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IFolderService, FolderService>();
        services.AddScoped<INoteService, NoteService>();
        services.AddScoped<ISectionService, SectionService>();
    }


    public static void Initialize(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
        .WriteTo.File("logs/myapp-.log", rollingInterval: RollingInterval.Day)
        .CreateLogger();

        Log.Information("Starting up!");


        //SQL Connection
        var connection = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("connection"));


        //Authentication Settings
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>{
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("0BD0E95C-6387-4135-A80E-489FF6E5C1DF")),
                ValidateIssuer = false,
                ValidateAudience = false,
            };
        });


        //Authorization Settings
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy => policy.RequireRole(Role.Admin.ToString()));
            options.AddPolicy("User", policy => policy.RequireRole(Role.User.ToString(), Role.Admin.ToString()));
        });


        //Cors Settings
        services.AddCors(options =>
        {
            options.AddPolicy("MyCors", policy =>
            {
                policy.WithOrigins("http://localhost:3000", "https://ittaskmanager.adra.gov.az")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });
    }
}
