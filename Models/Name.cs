namespace JITs.Models;

public class Name
{
    [FirestoreProperty]
    public string First { get; set; }
    
    [FirestoreProperty]
    public string Middle { get; set; }
    
    [FirestoreProperty]
    public string Last { get; set; }
    
    [FirestoreProperty]
    public string Extensions { get; set; }
    
    [FirestoreProperty]
    public string Full { get; set; }

    public static string SetFull(Name name)
    {
        return $"{name.First}{(!string.IsNullOrEmpty(name.Middle) ? $" {name.Middle.Substring(0,1)}." : "")} {name.Last}{(!string.IsNullOrEmpty(name.Extensions) ? name.Extensions : "")}";
    }
}