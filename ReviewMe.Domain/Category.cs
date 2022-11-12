namespace ReviewMe.Domain;
public record Category
{
    public string Name { get; }
    public string Description { get; }
    public Category(string name, string description)
    {
        Name = name;
        Description = description;
    }
}