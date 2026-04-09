namespace CreativeLab.WebApi.Dto;

public class AddFavoriteMasterclassDto
{
    public Guid UserId { get; set; }
    public Guid MasterclassId { get; set; }
}

public class RemoveFavoriteMasterclassDto
{
    public Guid UserId { get; set; }
    public Guid MasterclassId { get; set; }
}

public class AddFavoriteProductDto
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
}

public class RemoveFavoriteProductDto
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
}
