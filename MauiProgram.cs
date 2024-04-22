using JITs.Services.DB;
using Microsoft.Extensions.Logging;

namespace JITs;

public static class MauiProgram
{
    // CMH7
    //private static string CredentialPath = @"D:\Personal\Projects\JITs\wwwroot\jits-f801f-firebase-adminsdk-p7fj2-cee723cdf0.json";

    // TBSI-CM
    //private static string CredentialPath = @"D:\OTHERS\comms\JITs\wwwroot\jits-f801f-firebase-adminsdk-p7fj2-cee723cdf0.json";

    // Mobile
    private static string CredentialPath = @"/wwwroot/js/jits-f801f-firebase-adminsdk-p7fj2-cee723cdf0.json";

    public static MauiApp CreateMauiApp()
    {
        CredentialPath = "/jits-f801f-firebase-adminsdk-p7fj2-cee723cdf0.json";
        //Stream? stream = FileSystem.OpenAppPackageFileAsync("jits-f801f-firebase-adminsdk-p7fj2-cee723cdf0.json").GetAwaiter().GetResult();
        //using var reader = new StreamReader(stream);
        //string dataResourceText = reader.ReadToEndAsync().GetAwaiter().GetResult();
        //File.Copy(CredentialPath, FileSystem.CacheDirectory);
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
