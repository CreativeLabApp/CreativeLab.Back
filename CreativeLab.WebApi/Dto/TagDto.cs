namespace CreativeLab.WebApi.Dto;

public class CreateTagDto
{
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; }
}

public class UpdateTagDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; }
}
