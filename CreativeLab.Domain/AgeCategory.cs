namespace CreativeLab.Domain;

public class AgeCategory
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public string? Description { get; set; }

    public ICollection<Masterclass> Masterclasses { get; set; } = [];
}