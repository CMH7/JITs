namespace JITs.Models;

public class Auditable
{
    [FirestoreProperty]
    public UserHeader CreatedBy { get; set; }

    [FirestoreProperty]
    public DateTime CreatedDate { get; set; }

    [FirestoreProperty]
    public UserHeader LastModifiedBy { get; set; }

    [FirestoreProperty]
    public DateTime LastModifiedDate { get; set; }

    public Auditable() { }

    public Auditable(UserHeader userHeader, bool update)
    {
        DateTime now = DateTimeProvider.Now;
        if (update)
        {
            LastModifiedBy = userHeader;
            LastModifiedDate = now;
        } 
        else
        {
            CreatedBy = userHeader;
            CreatedDate = now;
            LastModifiedBy = userHeader;
            LastModifiedDate = now;
        }
    }
}