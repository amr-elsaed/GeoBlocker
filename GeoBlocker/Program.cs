using GeoBlocker.BLL.Services.Abstraction;
using GeoBlocker.BLL.Services.Implmentation;
using GeoBlocker.DAL.Repo.Abstraction;
using GeoBlocker.DAL.Repo.Implmentation;
using GeoBlocker.PL.Helper;

namespace GeoBlocker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Injection Confg
            builder.Services.AddSingleton<ICountryRepo, CountryRepo>();
            builder.Services.AddSingleton<ICountryService, CountryService>();
            builder.Services.AddSingleton<IGeoIpService, GeoIpService>();
            builder.Services.AddSingleton<ILogRepo, LogRepo>();
            builder.Services.AddSingleton<ILogService, LogService>();


            var baseUrl = builder.Configuration["IpApiSettings:BaseUrl"] ?? "https://ipapi.co/";
            builder.Services.AddHttpClient("IpApi", client =>
            {
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(10);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", "CountryBlockingAPI/1.0");
            });


            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
