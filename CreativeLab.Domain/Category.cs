namespace CreativeLab.Domain;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }

    public ICollection<Masterclass> Masterclasses { get; set; } = [];
    public ICollection<Product> Products { get; set; } = [];
}
