using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using MyFinan.Repositories;

namespace MyFinan.Endpoints;

[AllowAnonymous]
[HttpGet("dash")]
public class DashEndpoint : EndpointWithoutRequest
{
    private readonly ITransacoesRepository _transacoesRepository;

    public DashEndpoint(ITransacoesRepository transacoesRepository)
    {
        _transacoesRepository = transacoesRepository;
    }

    public async override Task HandleAsync(CancellationToken ct)
    {
        await Send.OkAsync(await _transacoesRepository.Dash());
    }
}