namespace JITs.Models;

public class ClassParent : Auditable
{
    public ClassInfo Info { get; set; }

    public ClassParent() : base() { }

    public ClassParent(ClassInfo info, UserHeader userHeader, bool update) : base(userHeader, update) => Info = info;

    public static ClassParent Base(ClassInfo info, UserHeader userHeader, bool update) => new(info, userHeader, update);
}

public class ClassInfo
{
    public int Level { get; set; }
    public string Section { get; set; }
}

public class ClassVM : ClassInfo
{
    public string Id { get; set; }

    public ClassVM() { }
    public ClassVM(ClassParent classparent)
    {
        Level = classparent.Info.Level;
        Section = classparent.Info.Section;
    }
}