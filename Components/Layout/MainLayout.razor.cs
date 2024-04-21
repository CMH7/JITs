
namespace JITs.Components.Layout;

public partial class MainLayout : IAsyncDisposable
{
    [Inject] private AppState appState { get; set; }
    [Inject] private NavigationManager navigationManager { get; set; }

    protected override void OnInitialized()
    {
        setup();
        base.OnInitialized();
    }

    private void setup()
    {
        appState ??= new();
        appState.CurrentUser ??= new();
        appState.stateHasChanged += StateHasChanged;
    }

    public ValueTask DisposeAsync()
    {
        appState.stateHasChanged -= StateHasChanged;
        return new ValueTask();
    }

    bool sidebarExpanded = false;

    private async Task Logout()
    {
        appState.IsBusy = true;
        await App.Db.DeleteAllAsync<SQliteUser>();
        appState.CurrentUser = new();
        appState.IsBusy = false;
        navigationManager.NavigateTo("/login");
    }
}