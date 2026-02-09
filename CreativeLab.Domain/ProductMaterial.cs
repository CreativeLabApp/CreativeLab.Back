namespace CreativeLab.Domain;

public class ProductMaterial
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty; // Изменил Title на Name для консистентности
    public string? Description { get; set; }

    public ICollection<Product> Products { get; set; } = [];
}
