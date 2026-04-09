using AutoMapper;
using CreativeLab.Application.Features.Products.Commands.CreateProduct;
using CreativeLab.Application.Features.Products.Commands.DeleteProduct;
using CreativeLab.Application.Features.Products.Commands.UpdateProduct;
using CreativeLab.Application.Features.Products.Queries.GetProductDetails;
using CreativeLab.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CreativeLab.WebApi.Controllers;

public class ProductController(IMapper mapper) : BaseController
{
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
        return Ok(product);
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
}
