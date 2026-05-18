using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using MyFinan.Models.Entities;
using MyFinan.Repositories;

namespace MyFinan.Endpoints;

[AllowAnonymous]
[HttpPost("categorias")]
public class CategoriasNovoEndpoint : Endpoint<Categorias>
{
    private readonly ICategoriasRepository _categoriasRepository;

    public CategoriasNovoEndpoint(ICategoriasRepository categoriasRepository)
    {
        _categoriasRepository = categoriasRepository;
    }

    public override async Task HandleAsync(Categorias req, CancellationToken ct)
    {
        await Send.OkAsync(await _categoriasRepository.Insert(req));
    }
}

[AllowAnonymous]
[HttpGet("categorias")]
public class CategoriasGetAllEndpoint : EndpointWithoutRequest
{
    private readonly ICategoriasRepository _categoriasRepository;

    public CategoriasGetAllEndpoint(ICategoriasRepository categoriasRepository)
    {
        _categoriasRepository = categoriasRepository;
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await Send.OkAsync(await _categoriasRepository.GetAll());
    }
}

[AllowAnonymous]
[HttpPut("categorias/{id}")]
public class CategoriasUpdateEndpoint : Endpoint<Categorias>
{
    private readonly ICategoriasRepository _categoriasRepository;

    public CategoriasUpdateEndpoint(ICategoriasRepository categoriasRepository)
    {
        _categoriasRepository = categoriasRepository;
    }

    public override async Task HandleAsync(Categorias req, CancellationToken ct)
    {
        await _categoriasRepository.Update(req);
        await Send.OkAsync();
    }
}

[AllowAnonymous]
[HttpDelete("categorias/{id}")]
public class CategoriasDeleteEndpoint : EndpointWithoutRequest
{
    private readonly ICategoriasRepository _categoriasRepository;

    public CategoriasDeleteEndpoint(ICategoriasRepository categoriasRepository)
    {
        _categoriasRepository = categoriasRepository;
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<long>("id");
        await _categoriasRepository.Delete(id);
        await Send.OkAsync();
    }
}
