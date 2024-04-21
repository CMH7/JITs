using JITs.Entities;
using JITs.Services.DB;
using JITs.Services.Helpers;
using Newtonsoft.Json;
using Query = Google.Cloud.Firestore.Query;

namespace JITs.Components.Pages;

public partial class Login : IAsyncDisposable
{
    [Inject] private AppState appState { get; set; }
    [Inject] private NotificationService notificationService { get; set; }
    [Inject] private IFDb db { get; set; }
    [Inject] private NavigationManager navigationManager { get; set; }

    private class Credential
    {
        public string EmailOrLRN { get; set; }
        public string Password { get; set; }
    }

    private Credential Creds { get; set; }

    protected override void OnInitialized()
    {
        setup();
        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        bool hasUser = await HasUser();
        if (hasUser) navigationManager.NavigateTo("/home", true, true);
        await base.OnInitializedAsync();
    }
    private void setup()
    {
        appState ??= new();
        appState.stateHasChanged += StateHasChanged;
        Creds ??= new();
    }

    public ValueTask DisposeAsync()
    {
        appState.stateHasChanged -= StateHasChanged;
        return new ValueTask();
    }

    private async Task<bool> HasUser()
    {
        List<SQliteUser> users = await App.Db.GetAllAsync<SQliteUser>();
        //SQliteUser user = users.First();
        //appState.CurrentUser = JsonConvert.DeserializeObject<User>(user.UserJSON);
        
        return users is not null && users.Count > 0;
    }

    private async Task Submit()
    {
        appState.IsBusy = true;

        Notif(NotificationSeverity.Info, "Loggin in", "Please wait...");

        CollectionReference UsersColRef = db.GetCollectionReference("Users");
        Query Qry = UsersColRef.WhereEqualTo("LRN", Creds.EmailOrLRN);
        QuerySnapshot QrySnapshot = await Qry.GetSnapshotAsync();
        bool NoData = QrySnapshot is null || QrySnapshot.Count <= 0;

        if (NoData) Notif(NotificationSeverity.Error, "Failed", "No user found");
        else
        {
            foreach (DocumentSnapshot doc in QrySnapshot.Documents)
            {
                if (doc.GetValue<string>("Password") != Creds.Password) Notif(NotificationSeverity.Error, "Invalid", "Wrong password");
                else
                {
                    appState.CurrentUser = Converter.GetObject<User>(doc.ToDictionary());
                    appState.CurrentUser.DocId = doc.Id;
                    string UserJson = JsonConvert.SerializeObject(appState.CurrentUser);
                    await App.Db.InsertAsync(new SQliteUser() { UserJSON = UserJson });
                }
            }
            Notif(NotificationSeverity.Success, "Success", "Welcome back!");
            navigationManager.NavigateTo("/home", true, true);
        }
        appState.IsBusy = false;
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