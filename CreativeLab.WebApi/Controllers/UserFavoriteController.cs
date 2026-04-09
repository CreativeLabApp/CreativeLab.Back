using AutoMapper;
using CreativeLab.Application.Features.UserFavorites.Commands.AddFavoriteMasterclass;
using CreativeLab.Application.Features.UserFavorites.Commands.AddFavoriteProduct;
using CreativeLab.Application.Features.UserFavorites.Commands.RemoveFavoriteMasterclass;
using CreativeLab.Application.Features.UserFavorites.Commands.RemoveFavoriteProduct;
using CreativeLab.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CreativeLab.WebApi.Controllers;

public class UserFavoriteController(IMapper mapper) : BaseController
{
    [HttpPost]
    public async Task<ActionResult> AddMasterclass([FromBody] AddFavoriteMasterclassDto dto)
    {
        var command = mapper.Map<AddFavoriteMasterclassCommand>(dto);
        var favorite = await Mediator.Send(command);
        return Ok(favorite);
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveMasterclass([FromBody] RemoveFavoriteMasterclassDto dto)
    {
        var command = mapper.Map<RemoveFavoriteMasterclassCommand>(dto);
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult> AddProduct([FromBody] AddFavoriteProductDto dto)
    {
        var command = mapper.Map<AddFavoriteProductCommand>(dto);
        var favorite = await Mediator.Send(command);
        return Ok(favorite);
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveProduct([FromBody] RemoveFavoriteProductDto dto)
    {
        var command = mapper.Map<RemoveFavoriteProductCommand>(dto);
        await Mediator.Send(command);
        return NoContent();
    }
}
