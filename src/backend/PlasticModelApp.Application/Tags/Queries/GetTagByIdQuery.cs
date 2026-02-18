using MediatR;

namespace PlasticModelApp.Application.Tags.Queries;

/// <summary>
/// Query to get a tag by its unique identifier. 
/// This query takes a string ID as input and returns the corresponding tag information if found.
/// </summary>
public record GetTagByIdQuery(string Id) : IRequest<GetTagByIdResult>;
