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
        var result = await _transacoesRepository.Dash();
        var totalGastos = result.Sum(s => s.Valor);

        var dto = new
        {
            dados = result,
            total = totalGastos,
            percentuais = result.Select(s => new
            {
                s.Categoria,
                percentual = (result.Where(c => c.Categoria == s.Categoria)
                                    .Sum(sum => sum.Valor) / totalGastos) * 100
            })
        };

        await Send.OkAsync(dto);
    }
}