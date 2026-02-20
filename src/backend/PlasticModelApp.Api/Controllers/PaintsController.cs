using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlasticModelApp.Api.Contracts.Requests;
using PlasticModelApp.Api.Contracts.Responses;
using PlasticModelApp.Application.Paints.Queries;
using PlasticModelApp.Application.Tags.Queries;

namespace PlasticModelApp.Api.Controllers;

[ApiController]
[Route("api/paints")]
public sealed class PaintsController : ControllerBase
{
    private readonly ISender _sender;

    public PaintsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("search")]
    [ProducesResponseType(typeof(PaintListResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaintListResponse>> Search(
        [FromBody] PaintSearchCriteria request,
        CancellationToken cancellationToken)
    {
        var criteria = new Criteria
        {
            BrandIds = request.BrandIds ?? [],
            PaintTypeIds = request.PaintTypeIds ?? [],
            GlossIds = request.GlossIds ?? [],
            TagIds = request.TagIds ?? [],
            Sort = ParseSort(request.Sort),
            Page = request.Page,
            PageSize = request.PageSize,
        };

        criteria.Normalize();

        var result = await _sender.Send(new SearchPaintsQuery(criteria), cancellationToken);

        var response = new PaintListResponse
        {
            Items = result.Items.Select(item => new PaintListItem
            {
                Id = item.Id,
                Name = item.Name,
                ModelNumber = item.ModelNumber,
                Brand = item.BrandName,
                Hex = item.HexColor,
                PaintType = null,
                Gloss = null,
            }).ToList(),
            Total = result.Total,
            Page = criteria.Page,
            PageSize = criteria.PageSize,
        };

        return Ok(response);
    }

    [HttpPost("color-search")]
    [ProducesResponseType(typeof(SimilarPaintListResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<SimilarPaintListResponse>> SearchByColor(
        [FromBody] ColorSearchCriteria request,
        CancellationToken cancellationToken)
    {
        var query = new SearchSimilarPaintsQuery(
            request.R,
            request.G,
            request.B,
            request.Threshold,
            request.Page,
            request.PageSize);

        query.Normalize();

        var result = await _sender.Send(query, cancellationToken);

        var response = new SimilarPaintListResponse
        {
            Items = result.Items.Select(item => new SimilarPaintItem
            {
                Id = item.Id,
                Name = item.Name,
                ModelNumber = item.ModelNumber,
                Brand = item.BrandName,
                Hex = item.HexColor,
                PaintType = null,
                Gloss = null,
                Similarity = item.Similarity,
            }).ToList(),
            Total = result.Total,
            Page = query.Page,
            PageSize = query.PageSize,
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PaintResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaintResponse>> GetById(string id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetPaintByIdQuery(id), cancellationToken);

        var response = new PaintResponse
        {
            Id = result.Id,
            Name = result.Name,
            ModelNumber = result.ModelNumber,
            Brand = result.BrandName,
            PaintType = result.PaintTypeName,
            Gloss = result.GlossName,
            Price = result.Price,
            Description = result.Description,
            Hex = result.Hex,
            RgbR = result.Rgb_R,
            RgbG = result.Rgb_G,
            RgbB = result.Rgb_B,
            HslH = result.Hsl_H,
            HslS = result.Hsl_S,
            HslL = result.Hsl_L,
            ImageUrl = result.ImageUrl,
            Tags = result.Tags.Select(MapTag).ToList(),
            CreatedAt = result.CreatedAt,
            UpdatedAt = result.UpdatedAt,
        };

        return Ok(response);
    }

    private static SearchPaintsSortOption ParseSort(string? sort)
    {
        return Enum.TryParse<SearchPaintsSortOption>(sort, true, out var parsed)
            ? parsed
            : SearchPaintsSortOption.ModelNumberAsc;
    }

    private static TagResponse MapTag(GetTagByIdResult tag)
    {
        return new TagResponse
        {
            Id = tag.Id,
            Name = tag.Name,
            Category = tag.CategoryId,
            Hex = tag.Hex,
            Effect = tag.Effect,
            Description = tag.Description,
            CreatedAt = tag.CreatedAt,
            UpdatedAt = tag.UpdatedAt,
        };
    }
}
