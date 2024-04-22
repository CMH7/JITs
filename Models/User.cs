namespace JITs.Models;

[FirestoreData]
public class User
{
    public string DocId { get; set; }
    
    [FirestoreProperty]
    public Name Name { get; set; }
    
    [FirestoreProperty]
    public string Sex { get; set; }
    
    [FirestoreProperty]
    public string Picture { get; set; } = string.Empty;

    [FirestoreProperty]
    public string Email { get; set; } = string.Empty;
    
    [FirestoreProperty]
    public string LRN { get; set; }
    
    [FirestoreProperty]
    public string Password { get; set; }
    
    [FirestoreProperty]
    public ClassVM Class { get; set; }
    
    [FirestoreProperty]
    public bool Administrator { get; set; }
    
    [FirestoreProperty]
    public bool SUP { get; set; }

    public User()
    {
        Name = new();
        Class = new();
    }
}