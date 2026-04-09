using AutoMapper;
using CreativeLab.Application.Features.ProductMaterials.Commands.CreateProductMaterial;
using CreativeLab.Application.Features.ProductMaterials.Commands.DeleteProductMaterial;
using CreativeLab.Application.Features.ProductMaterials.Commands.UpdateProductMaterial;
using CreativeLab.Application.Features.ProductMaterials.Queries.GetProductMaterialDetails;
using CreativeLab.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CreativeLab.WebApi.Controllers;

public class ProductMaterialController(IMapper mapper) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<ProductMaterialDetailsVm>> Get([FromQuery] Guid id)
    {
        var vm = await Mediator.Send(new GetProductMaterialDetailsQuery { Id = id });
        return Ok(vm);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateProductMaterialDto dto)
    {
        var command = mapper.Map<CreateProductMaterialCommand>(dto);
        var material = await Mediator.Send(command);
        return Ok(material);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateProductMaterialDto dto)
    {
        var command = mapper.Map<UpdateProductMaterialCommand>(dto);
        var material = await Mediator.Send(command);
        return Ok(material);
    }

    [HttpDelete]
    public async Task<ActionResult> Delete([FromQuery] Guid id)
    {
        await Mediator.Send(new DeleteProductMaterialCommand { Id = id });
        return NoContent();
    }
}
