using SQLite;

namespace JITs.Models;

public class SQliteClassParent
{
    [PrimaryKey]
    public int Id { get; set; }
    public string ClassParentJSON { get; set; }
    

    public SQliteClassParent() { }

    public SQliteClassParent(string data) => ClassParentJSON = data;
}
