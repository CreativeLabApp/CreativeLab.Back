namespace CreativeLab.WebApi.Dto;

public class AgeCategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public string? Description { get; set; }
}