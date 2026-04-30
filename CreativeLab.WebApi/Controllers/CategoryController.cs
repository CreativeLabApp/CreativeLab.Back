using AutoMapper;
using CreativeLab.Application.Features.Categories.Commands.CreateCategory;
using CreativeLab.Application.Features.Categories.Commands.DeleteCategory;
using CreativeLab.Application.Features.Categories.Commands.UpdateCategory;
using CreativeLab.Application.Features.Categories.Queries.GetCategoryDetails;
using CreativeLab.Application.Features.Categories.Queries.GetCategoryList;
using CreativeLab.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CreativeLab.WebApi.Controllers;

public class CategoryController(IMapper mapper) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<List<CategoryLookupDto>>> GetAll()
    {
        var vm = await Mediator.Send(new GetCategoryListQuery());
        return Ok(vm);
    }

    [HttpGet]
    public async Task<ActionResult<CategoryDetailsVm>> Get([FromQuery] Guid id)
    {
        var vm = await Mediator.Send(new GetCategoryDetailsQuery { Id = id });
        return Ok(vm);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateCategoryDto dto)
    {
        var command = mapper.Map<CreateCategoryCommand>(dto);
        var category = await Mediator.Send(command);
        return Ok(category);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateCategoryDto dto)
    {
        var command = mapper.Map<UpdateCategoryCommand>(dto);
        var category = await Mediator.Send(command);
        return Ok(category);
    }

    [HttpDelete]
    public async Task<ActionResult> Delete([FromQuery] Guid id)
    {
        await Mediator.Send(new DeleteCategoryCommand { Id = id });
        return NoContent();
    }
}
