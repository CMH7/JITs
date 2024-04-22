using JITs.Services.DB;
using JITs.Services.Helpers;
using Newtonsoft.Json;
using Query = Google.Cloud.Firestore.Query;

namespace JITs.Components.Pages;

public partial class CreateAccount
{
    [Inject] private AppState appState { get; set; }
    [Inject] private NavigationManager navigationManager { get; set; }
    [Inject] private IFDb db { get; set; }
    [Inject] private NotificationService notificationService { get; set; }

    private int currentStep { get; set; }

    private class Sex
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public Sex(string sex)
        {
            Name = sex;
            Value = sex;
        }
    }

    private User NewUser { get; set; }
    private string ConfirmPassword { get; set; }

    private List<Sex> sexes { get; set; }
    private List<ClassVM> classLevels { get; set; }
    private List<ClassVM> classSections { get; set; }

    protected override void OnInitialized()
    {
        setup();
        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        CollectionReference ClassesColRef = db.GetCollectionReference("Classes");
        QuerySnapshot QrySnapshot = await ClassesColRef.GetSnapshotAsync();
        bool NoData = QrySnapshot is null || QrySnapshot.Count <= 0;

        if (NoData) Notif(NotificationSeverity.Error, "Fatal error", "No data for classes");
        else
        {
            List<SQliteClassParent> classesJSON = [];
            foreach (DocumentSnapshot doc in QrySnapshot.Documents)
            {
                ClassParent classparent = Converter.GetObject<ClassParent>(doc.ToDictionary());
                ClassVM cvm = new(classparent);
                classLevels.Add(cvm);
                classSections.Add(cvm);
                //string classJSON = JsonConvert.SerializeObject(classparent);
                //classesJSON.Add(new(classJSON));
            }

            //await App.Db.InsertAllAsync(classesJSON);
            Notif(NotificationSeverity.Success, "Success", "Classes data fetched");
        }
        await base.OnInitializedAsync();
    }

    private void setup()
    {
        appState ??= new();
        NewUser ??= new();
        sexes ??= [new("Male"), new("Female"), new("Rather not say")];
        classLevels ??= [];
        classSections ??= [];
    }

    private async Task Submit()
    {
        try
        {
            appState.IsBusy = true;
            Notif(NotificationSeverity.Info, "Processing", "Please wait...");

            NewUser.Name.Full = Name.SetFull(NewUser.Name);

            CollectionReference UsersColRef = db.GetCollectionReference("Users");
            DocumentReference docRef = UsersColRef.Document();
            string usernamejson = JsonConvert.SerializeObject(NewUser.Name);
            string userclassjson = JsonConvert.SerializeObject(NewUser.Class);
            string json = JsonConvert.SerializeObject(NewUser);
            Dictionary<string, object> kv = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            kv["Name"] = JsonConvert.DeserializeObject<Dictionary<string, object>>(usernamejson);
            kv["Class"] = JsonConvert.DeserializeObject<Dictionary<string, object>>(userclassjson);
            WriteResult res = await docRef.SetAsync(kv);
            //DocumentReference UserDocRef = await UsersColRef.AddAsync(NewUser);

            //if (UserDocRef is null) Notif(NotificationSeverity.Error, "Creationg failed", "There's something wrong. Please contact your developer.");
            if (res is null) Notif(NotificationSeverity.Error, "Creationg failed", "There's something wrong. Please contact your developer.");
            else
            {
                Notif(NotificationSeverity.Success, "Success", "Account created, login now!");
                Notif(NotificationSeverity.Info, "Processing", "Auto-logging in...");

                Query Qry = UsersColRef.WhereEqualTo("LRN", NewUser.LRN).WhereEqualTo("Password", NewUser.Password);
                QuerySnapshot QrySnapshot = await Qry.GetSnapshotAsync();
                bool NoData = QrySnapshot is null || QrySnapshot.Count <= 0;

                if (NoData) Notif(NotificationSeverity.Error, "Failed", "No user found");
                else
                {
                    foreach (DocumentSnapshot doc in QrySnapshot.Documents)
                    {
                        NewUser.DocId = doc.Id;
                        string UserJson = JsonConvert.SerializeObject(NewUser);
                        await App.Db.InsertAsync(new SQliteUser() { UserJSON = UserJson });
                    }
                    Notif(NotificationSeverity.Success, "Success", "Welcome back!");
                    navigationManager.NavigateTo("/home", true, true);
                }
                appState.IsBusy = false;
            }
            await Task.Delay(4000);

            appState.IsBusy = false;
        }
        catch (Exception ex)
        {
            Notif(NotificationSeverity.Error, $"{ex.Message}", $"{ex.InnerException?.Message}");
            appState.IsBusy = false;
        }
    }

    private void Login()
    {
        navigationManager.NavigateTo("/login", true, true);
    }

    private void OnChange(int index)
    {

    }

    private void SectionChanged()
    {
        NewUser.Class.Id = classLevels.First(x => x.Level == NewUser.Class.Level && x.Section == NewUser.Class.Section).Id;
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