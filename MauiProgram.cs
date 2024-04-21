using JITs.Services.DB;
using Microsoft.Extensions.Logging;

namespace JITs;

public static class MauiProgram
{
    private static string CredentialPath = @"D:\Personal\Projects\JITs\wwwroot\jits-f801f-firebase-adminsdk-p7fj2-cee723cdf0.json";

    public static MauiApp CreateMauiApp()
    {
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", CredentialPath);

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

        builder.Services.AddRadzenComponents();

        builder.Services.AddScoped<IFDb, FDb>();
        builder.Services.AddScoped<AppState>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
