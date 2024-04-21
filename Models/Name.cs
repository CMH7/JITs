namespace JITs.Models;

public class Name
{
    public string First { get; set; }
    public string Middle { get; set; }
    public string Last { get; set; }
    public string Extensions { get; set; }
    public string Full { get; set; }

    public static string SetFull(Name name)
    {
        return $"{name.First}{(!string.IsNullOrEmpty(name.Middle) ? $" {name.Middle.Substring(0,1)}." : "")} {name.Last}{(!string.IsNullOrEmpty(name.Extensions) ? name.Extensions : "")}";
    }
}