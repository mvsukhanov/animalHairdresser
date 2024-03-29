using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Npgsql;
using System.Numerics;
using System.Xml.Linq;
using animalHairdresser.Controllers;
using System.Net;
using Npgsql.NameTranslation;
using Microsoft.AspNetCore.HttpOverrides;

namespace animalHairdresser
{
    internal class Program
    {
        public const string connString = "Host=localhost;Username=users;Port=5432;Password=12345;Database=AnimalShop";
        static void Main(string[] args)
        {
            NpgsqlConnection.GlobalTypeMapper.MapComposite<Animal>("animal", new NpgsqlNullNameTranslator());

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddTransient<IClientBaseService, ClientBaseService>();
            builder.Services.AddTransient<IOrderBaseService, OrderBaseService>();
            builder.Services.AddTransient<IAnimalsBreedsAndPriceService, AnimalsBreedsAndPriceCervice>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.LoginPath = "/Login");
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

            app.UseStaticFiles();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=HomeGet}");

            app.Run();
        }
    }
}