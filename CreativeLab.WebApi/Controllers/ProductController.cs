using AutoMapper;
using CreativeLab.Application.Features.Products.Commands.CreateProduct;
using CreativeLab.Application.Features.Products.Commands.DeleteProduct;
using CreativeLab.Application.Features.Products.Commands.RateProduct;
using CreativeLab.Application.Features.Products.Commands.UpdateProduct;
using CreativeLab.Application.Features.Products.Queries.GetProductDetails;
using CreativeLab.Application.Features.Products.Queries.GetProductList;
using CreativeLab.Application.Features.Products.Queries.GetUserRating;
using CreativeLab.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CreativeLab.WebApi.Controllers;

public class ProductController(IMapper mapper) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<ProductListVm>> GetAll([FromQuery] bool onlyAvailable = true)
    {
        var vm = await Mediator.Send(new GetProductListQuery { OnlyAvailable = onlyAvailable });
        return Ok(vm);
    }

    [HttpGet]
    public async Task<ActionResult<ProductListVm>> GetBySeller([FromQuery] Guid sellerId, [FromQuery] bool onlyAvailable = true)
    {
        var vm = await Mediator.Send(new GetProductListQuery { SellerId = sellerId, OnlyAvailable = onlyAvailable });
        return Ok(vm);
    }

    [HttpGet]
    public async Task<ActionResult<ProductDetailsVm>> Get([FromQuery] Guid id)
    {
        var vm = await Mediator.Send(new GetProductDetailsQuery { Id = id });
        return Ok(vm);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateProductDto dto)
    {
        var command = mapper.Map<CreateProductCommand>(dto);
        var product = await Mediator.Send(command);
        return Ok(new { product.Id });
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateProductDto dto)
    {
        var command = mapper.Map<UpdateProductCommand>(dto);
        var product = await Mediator.Send(command);
        return Ok(product);
    }

    [HttpDelete]
    public async Task<ActionResult> Delete([FromQuery] Guid id)
    {
        await Mediator.Send(new DeleteProductCommand { Id = id });
        return NoContent();
    }

    [HttpPatch]
    public async Task<ActionResult<decimal>> Rate([FromQuery] Guid id, [FromQuery] int score)
    {
        var newRating = await Mediator.Send(new RateProductCommand { ProductId = id, UserId = UserId, Score = score });
        return Ok(new { rating = newRating });
    }

    [HttpGet]
    public async Task<ActionResult<UserProductRatingDto?>> GetUserRating([FromQuery] Guid productId)
    {
        var result = await Mediator.Send(new GetUserRatingQuery { ProductId = productId, UserId = UserId });
        return Ok(result);
    }
}
