namespace CreativeLab.Domain;

public class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; }

    public ICollection<Masterclass> Masterclasses { get; set; } = [];
    public ICollection<Product> Products { get; set; } = [];
}
