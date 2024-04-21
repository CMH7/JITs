using SQLite;

namespace JITs.Models;

public class SQliteUser
{
    [PrimaryKey]
    public int Id { get; set; }
    public string UserJSON { get; set; }
}
