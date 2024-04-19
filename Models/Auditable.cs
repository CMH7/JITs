namespace JITs.Entities;

public class Auditable
{
    public UserHeader CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public UserHeader LastModifiedBy { get; set; }
    public DateTime LastModifiedDate { get; set; }

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