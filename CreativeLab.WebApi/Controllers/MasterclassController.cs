using AutoMapper;
using CreativeLab.Application.Features.Masterclasses.Commands.CreateMasterclass;
using CreativeLab.Application.Features.Masterclasses.Commands.DeleteMasterclass;
using CreativeLab.Application.Features.Masterclasses.Commands.RateMasterclass;
using CreativeLab.Application.Features.Masterclasses.Commands.UpdateMasterclass;
using CreativeLab.Application.Features.Masterclasses.Queries.GetMasterclassDetails;
using CreativeLab.Application.Features.Masterclasses.Queries.GetMasterclassList;
using CreativeLab.Application.Features.Masterclasses.Queries.GetMasterclassRatings;
using CreativeLab.Application.Features.Masterclasses.Queries.GetUserRating;
using CreativeLab.Application.Interfaces;
using CreativeLab.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CreativeLab.WebApi.Controllers;

public class MasterclassController(IMapper mapper, ICreativeLabDbContext dbContext) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<MasterclassListVm>> GetAll([FromQuery] bool onlyPublished = true)
    {
        var vm = await Mediator.Send(new GetMasterclassListQuery { OnlyPublished = onlyPublished });
        return Ok(vm);
    }

    [HttpGet]
    public async Task<ActionResult<MasterclassListVm>> GetByAuthor([FromQuery] Guid authorId, [FromQuery] bool onlyPublished = true)
    {
        var vm = await Mediator.Send(new GetMasterclassListQuery { AuthorId = authorId, OnlyPublished = onlyPublished });
        return Ok(vm);
    }

    [HttpGet]
    public async Task<ActionResult<MasterclassDetailsVm>> Get([FromQuery] Guid id)
    {
        var vm = await Mediator.Send(new GetMasterclassDetailsQuery { Id = id });
        return Ok(vm);
    }

    [HttpGet("materials")]
    public async Task<ActionResult<List<string>>> GetMaterials()
    {
        var materials = await dbContext.MasterclassMaterials
            .Select(m => m.Name)
            .Distinct()
            .OrderBy(m => m)
            .ToListAsync();
        return Ok(materials);
    }

    [HttpGet("agecategories")]
    public async Task<ActionResult<List<AgeCategoryDto>>> GetAgeCategories()
    {
        var ageCategories = await dbContext.AgeCategories
            .OrderBy(a => a.MinAge)
            .ToListAsync();
        return Ok(ageCategories.Select(a => new AgeCategoryDto
        {
            Id = a.Id,
            Name = a.Name,
            MinAge = a.MinAge,
            MaxAge = a.MaxAge,
            Description = a.Description
        }));
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateMasterclassDto dto)
    {
        var command = mapper.Map<CreateMasterclassCommand>(dto);
        var masterclass = await Mediator.Send(command);
        return Ok(new { masterclass.Id });
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateMasterclassDto dto)
    {
        var command = mapper.Map<UpdateMasterclassCommand>(dto);
        var masterclass = await Mediator.Send(command);
        return Ok(masterclass);
    }

    [HttpDelete]
    public async Task<ActionResult> Delete([FromQuery] Guid id)
    {
        await Mediator.Send(new DeleteMasterclassCommand { Id = id });
        return NoContent();
    }

    [HttpPatch]
    public async Task<ActionResult> Rate([FromQuery] Guid id, [FromQuery] Guid userId, [FromQuery] int score, [FromQuery] string? comment = null)
    {
        var result = await Mediator.Send(new RateMasterclassCommand { MasterclassId = id, UserId = userId, Score = score, Comment = comment });
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult> GetUserRating([FromQuery] Guid id, [FromQuery] Guid userId)
    {
        var result = await Mediator.Send(new GetUserMasterclassRatingQuery { MasterclassId = id, UserId = userId });
        return Ok(new { score = result?.Score, comment = result?.Comment });
    }

    [HttpGet]
    public async Task<ActionResult<List<MasterclassRatingDto>>> GetRatings([FromQuery] Guid id)
    {
        var ratings = await Mediator.Send(new GetMasterclassRatingsQuery { MasterclassId = id });
        return Ok(ratings);
    }
}
