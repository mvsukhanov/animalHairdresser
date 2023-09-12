using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Npgsql;
using Npgsql.NameTranslation;
using System.Numerics;
using System.Xml.Linq;
using animalHairdresser.Controllers;
using Microsoft.AspNetCore.HttpOverrides;
using System.Net;

namespace animalHairdresser
{
    internal class Program
    {
        public const string connString = "Host=localhost;Username=Administrator;Port=5432;Password=123;Database=AnimalShop";
        static void Main(string[] args)
        {
            NpgsqlConnection.GlobalTypeMapper.MapComposite<Animals>("animals", new NpgsqlNullNameTranslator());

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddTransient<IUserBaseService, UsersBaseService>();
            builder.Services.AddTransient<IClientBaseService, ClientBaseService>();
            builder.Services.AddTransient<IOrderBaseService, OrderBaseService>();
            builder.Services.AddTransient<IAnimalsBreedsAndPriceService, AnimalsBreedsAndPriceCervice>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.LoginPath = "/AcessDenied");
            builder.Services.AddAuthorization();
            
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Parse("127.0.0.1"));
            });

            var app = builder.Build();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication(); 
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=HomeGet}");

            app.Run();
        }
    }
}