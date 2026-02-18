using MediatR;

namespace PlasticModelApp.Application.Masters.Queries;

/// <summary>
/// Query to get master data.
/// </summary>
public sealed record GetMastersQuery : IRequest<GetMastersResult>;

