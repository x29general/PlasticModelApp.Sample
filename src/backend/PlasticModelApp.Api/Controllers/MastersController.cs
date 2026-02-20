using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlasticModelApp.Api.Contracts.Responses;
using PlasticModelApp.Application.Masters.Queries;

namespace PlasticModelApp.Api.Controllers;

[ApiController]
[Route("api/masters")]
public sealed class MastersController : ControllerBase
{
    private readonly ISender _sender;

    public MastersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [ProducesResponseType(typeof(MastersResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<MastersResponse>> GetMasters(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetMastersQuery(), cancellationToken);

        var response = new MastersResponse
        {
            Brands = result.Brands.Select(x => new MasterItemResponse { Id = x.Id, Name = x.Name }).ToList(),
            PaintTypes = result.PaintTypes.Select(x => new MasterItemResponse { Id = x.Id, Name = x.Name }).ToList(),
            Glosses = result.Glosses.Select(x => new MasterItemResponse { Id = x.Id, Name = x.Name }).ToList(),
            TagCategories = result.TagCategories.Select(x => new MasterItemResponse { Id = x.Id, Name = x.Name }).ToList(),
            Tags = result.Tags.Select(x => new TagResponse
            {
                Id = x.Id,
                Name = x.Name,
                Category = x.CategoryId,
                Hex = x.Hex,
                Effect = x.Effect,
                Description = x.Description,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToList()
        };

        return Ok(response);
    }
}
