namespace JITs;

public class AppState
{
    public Action stateHasChanged;

    private bool _isBusy;

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            stateHasChanged?.Invoke();
        }
    }

    public string CurrentPage { get; set; } = "Home (QR Code)";
}
