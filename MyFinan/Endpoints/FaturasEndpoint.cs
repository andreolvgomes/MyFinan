using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using MyFinan.Repositories;
using MyFinan.Services;

namespace MyFinan.Endpoints
{
    [AllowAnonymous]
    [HttpGet("faturas/upload")]
    public class FaturasUploadEndpoint : EndpointWithoutRequest
    {
        private readonly FaturasService _faturaService;

        public FaturasUploadEndpoint(FaturasService faturaService)
        {
            _faturaService = faturaService;
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            await Send.OkAsync(await _faturaService.Execute());
        }
    }

    [AllowAnonymous]
    [HttpGet("faturas")]
    public class FaturasGetAllEndpoint : EndpointWithoutRequest
    {
        private readonly ITransacoesRepository _transacoesRepository;

        public FaturasGetAllEndpoint(ITransacoesRepository transacoesRepository)
        {
            _transacoesRepository = transacoesRepository;
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            await Send.OkAsync(await _transacoesRepository.ObterTodos());
        }
    }

    [AllowAnonymous]
    [HttpGet("faturas/agrupar")]
    public class FaturasPorBeneficiarioEndpoint : EndpointWithoutRequest
    {
        private readonly ITransacoesRepository _transacoesRepository;

        public FaturasPorBeneficiarioEndpoint(ITransacoesRepository transacoesRepository)
        {
            _transacoesRepository = transacoesRepository;
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            await Send.OkAsync(await _transacoesRepository.AgruparPorBeneficiario());
        }
    }
}