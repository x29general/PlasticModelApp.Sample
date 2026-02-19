using MediatR;

namespace PlasticModelApp.Application.Paints.Queries;

/// <summary>
/// Get paint by id query
/// </summary>
public record GetPaintByIdQuery(string Id) : IRequest<GetPaintByIdResult>;
