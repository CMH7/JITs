using Microsoft.JSInterop;

namespace JITs.Components.Pages;

public partial class TestScanner
{
    [Inject] private IJSRuntime jsRuntime { get; set; }
    [Inject] private NotificationService notificationService { get; set; }

    private IJSObjectReference jsObjectReference { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await LoadJavaScriptFile("qrCodeScanner.js");
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task LoadJavaScriptFile(string fileName)
    {
        try
        {
            // Get a reference to the JavaScript file
            jsObjectReference = await jsRuntime.InvokeAsync<IJSObjectReference>("import", $"./js/{fileName}");

            // You can now use 'module' to invoke functions defined in the JavaScript file if needed.
            // For example, module.InvokeVoidAsync("functionName", args);
        }
        catch (Exception ex)
        {
            // Handle any errors
            Notif(NotificationSeverity.Error, $"Error", $"Failed to load {fileName}: {ex.Message}");
            await jsRuntime.InvokeVoidAsync("console.log", $"Failed to load {fileName}: {ex.Message}");
        }
    }

    private void Notif(NotificationSeverity Severity, string Summary, string Detail, double Duration = 5000)
    {
        notificationService.Notify(
            new NotificationMessage()
            {
                Severity = Severity,
                Summary = Summary,
                Detail = Detail,
                Duration = Duration
            }
        );
    }
}