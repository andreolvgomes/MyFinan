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
        var totalGastos = result.Sum(s => s.Total);
        var categoriaMaisCara = result.MaxBy(order => Math.Abs(order.Total));
        var categoriaMaisTransacoes = result
            .GroupBy(x => x.Categoria)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .First();

        var itemCategoriaMaisTransacoes = result.FirstOrDefault(c => c.Categoria == categoriaMaisTransacoes);

        var dto = new
        {
            dados = result,
            total = totalGastos,
            categoriaMaisCara,
            numeroTransacoes = result.Sum(c => c.Count),
            categoriaMaisTransacoes = itemCategoriaMaisTransacoes,

            percentuais = result.Select(s => new
            {
                s.Categoria,
                percentual = (result.Where(c => c.Categoria == s.Categoria)
                                    .Sum(sum => sum.Total) / totalGastos) * 100
            })
        };

        await Send.OkAsync(dto);
    }
}