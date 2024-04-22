namespace JITs.Models;

public class UserHeader
{
    public string Id { get; set; } // Corresponds to DocId

    [FirestoreProperty]
    public Name Name { get; set; }
}