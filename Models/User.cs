namespace JITs.Models;

public class User
{
    public string DocId { get; set; }
    public Name Name { get; set; }
    public string Sex { get; set; }
    public string Picture { get; set; }
    public string Email { get; set; }
    public string LRN { get; set; }
    public string Password { get; set; }
    public ClassVM Class { get; set; }
    public bool Administrator { get; set; }
    public bool SUP { get; set; }

    public User()
    {
        Name = new();
        Class = new();
    }
}