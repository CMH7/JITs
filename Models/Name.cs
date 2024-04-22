namespace JITs.Models;

public class Name
{
    [FirestoreProperty]
    public string First { get; set; } = string.Empty;
    
    [FirestoreProperty]
    public string Middle { get; set; } = string.Empty;

    [FirestoreProperty]
    public string Last { get; set; } = string.Empty;

    [FirestoreProperty]
    public string Extensions { get; set; } = string.Empty;

    [FirestoreProperty]
    public string Full { get; set; } = string.Empty;

    public static string SetFull(Name name)
    {
        return $"{name.First}{(!string.IsNullOrEmpty(name.Middle) ? $" {name.Middle.Substring(0,1)}." : "")} {name.Last}{(!string.IsNullOrEmpty(name.Extensions) ? name.Extensions : "")}";
    }

    public static Name FormatNames(Name name)
    {
        name.First = $"{name.First.Substring(0, 1).ToUpper()}{name.First.Substring(1,name.First.Length).ToLower()}";
        name.Middle = $"{name.Middle.Substring(0, 1).ToUpper()}{name.Middle.Substring(1,name.Middle.Length).ToLower()}";
        name.Last = $"{name.Last.Substring(0, 1).ToUpper()}{name.Last.Substring(1,name.Last.Length).ToLower()}";
        name.Extensions = $"{name.Extensions.Substring(0, 1).ToUpper()}{name.Extensions.Substring(1,name.Extensions.Length).ToLower()}";
        return name;
    }
}