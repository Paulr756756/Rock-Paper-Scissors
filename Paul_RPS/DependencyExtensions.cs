using Microsoft.Extensions.Diagnostics.HealthChecks;
using Paul_RPS.Data;

namespace Paul_RPS;

public static class DependencyExtensions
{
    
    /// <summary>
    /// Standard Dependencies
    /// </summary>
    /// <param name="builder">The application builder</param>
    public static void RegisterStandardDependencies(WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
    }


    /// <summary>
    /// Business Dependencies
    /// </summary>
    /// <param name="builder">The Application Builder</param>
    public static void RegisterBusinessDependencies(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IDataAccess, DataAccess>();
        builder.Services.AddScoped<IGameService, GameService>();
    }


    /// <summary>
    /// Default Application Methods
    /// </summary>
    /// <param name="app">The Application</param>
    public static void UseDefaultBuilderMethods(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();
        app.MapControllers();
        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        }
}
