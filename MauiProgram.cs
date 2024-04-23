using JITs.Services.DB;
using JITs.Services.Helpers;
using Microsoft.Extensions.Logging;

namespace JITs;

public static class MauiProgram
{
    // CMH7
    //private static string CredentialPath = @"D:\Personal\Projects\JITs\wwwroot\jits-f801f-firebase-adminsdk-p7fj2-cee723cdf0.json";

    // TBSI-CM
    private static string CredentialPath = @"D:\OTHERS\comms\JITs\wwwroot\jits-f801f-firebase-adminsdk-p7fj2-cee723cdf0.json";

    public static MauiApp CreateMauiApp()
    {
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
        builder.Services.AddScoped<IFileSystemAccess, FileSystemAccess>();
        builder.Services.AddScoped<AppState>();

#if ANDROID
        string filename = "jits-f801f-firebase-adminsdk-p7fj2-cee723cdf0.json";
        Stream? stream = FileSystem.OpenAppPackageFileAsync(filename).GetAwaiter().GetResult();
        FileSystemAccess fileSystem = new();
        fileSystem.UploadFile(filename, stream);
        string path = $"{fileSystem.GetStorageDirectory()}/{filename}";
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
#else
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", CredentialPath);
#endif

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
