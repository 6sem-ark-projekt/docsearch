using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using WebAppServer.Data;
using Core;  // Ensure your namespace for ISearchLogic and SearchLogic is correctly referenced

namespace WebAppServer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddSingleton<WeatherForecastService>();

        // Add SearchLogic as a scoped service
        builder.Services.AddScoped<ISearchLogic, SearchProxy>(); // Registering SearchLogic as the implementation of ISearchLogic

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }
}
