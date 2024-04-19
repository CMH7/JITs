
namespace JITs.Components.Layout;

public partial class MainLayout : IAsyncDisposable
{
    [Inject] private AppState appState { get; set; }

    protected override void OnInitialized()
    {
        setup();
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

    bool sidebarExpanded = false;
}