namespace JITs.Components.Pages;

public partial class Login : IAsyncDisposable
{
    [Inject] private AppState appState { get; set; }
    [Inject] private NotificationService notificationService { get; set; }

    private class Credential
    {
        public string EmailOrLRN { get; set; }
        public string Password { get; set; }
    }

    private Credential Creds { get; set; }

    protected override void OnInitialized()
    {
        setup();
        Creds ??= new();
        base.OnInitialized();
    }

    private void setup()
    {
        appState ??= new();
        appState.stateHasChanged += StateHasChanged;
    }

    public ValueTask DisposeAsync()
    {
        appState.stateHasChanged -= StateHasChanged;
        return new ValueTask();
    }

    private async Task Submit()
    {
        appState.IsBusy = true;

        notificationService.Notify(
            new NotificationMessage()
            {
                Severity = NotificationSeverity.Info,
                Summary = "Logging in",
                Detail = "Please wait...",
                Duration = 5000
            }    
        );


        await Task.Delay(4000);

        notificationService.Notify(
            new NotificationMessage()
            {
                Severity = NotificationSeverity.Success,
                Summary = "Success",
                Detail = "Welcome back!",
                Duration = 5000
            }
        );
        appState.IsBusy = false;
    }
}