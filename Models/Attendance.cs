namespace JITs.Models;

public class Attendance
{
    public ClassVM Class {  get; set; }
    public DateTime ScanDate { get; set; }
    public UserHeader ScannedBy { get; set; }
    public string Status { get; set; }
    public StudentHeader Student { get; set; }
}

public class StudentHeader
{
    public string LRN { get; set; }
    public Name Name { get; set; }
    public string UserId { get; set; }
}