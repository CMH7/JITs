using BCrypt.Net;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace JITs.Components.Pages;

public partial class Home : IAsyncDisposable
{
    [Inject] private AppState appState { get; set; }
    [Inject] private NotificationService notificationService { get; set; }
    [Inject] private IJSRuntime jSRuntime { get; set; }

    protected override void OnInitialized()
    {
        setup();
        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        bool hasUser = await HasUser();
        if (hasUser) Notif(NotificationSeverity.Success, "Welcome back", $"{appState.CurrentUser.Name.Full}");
        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender) await InitializeQRCode(); 
        await base.OnAfterRenderAsync(firstRender);
    }

    private void setup()
    {
        appState ??= new();
        appState.CurrentPage = "Home (QR Code)";
        appState.CurrentUser = new();
        appState.stateHasChanged += StateHasChanged;
    }

    private async Task InitializeQRCode()
    {
        string hashedLRN = BCrypt.Net.BCrypt.HashString(appState.CurrentUser.LRN, SaltRevision.Revision2Y);
        await jSRuntime.InvokeVoidAsync("GenerateQRCode", "qrcode", hashedLRN);
    }

    public ValueTask DisposeAsync()
    {
        appState.stateHasChanged -= StateHasChanged;
        return new ValueTask();
    }

    private async Task<bool> HasUser()
    {
        List<SQliteUser> users = await App.Db.GetAllAsync<SQliteUser>();
        SQliteUser user = users.First();
        appState.CurrentUser = JsonConvert.DeserializeObject<User>(user.UserJSON);
        appState.IsBusy = false;

        return users is not null && users.Count > 0;
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