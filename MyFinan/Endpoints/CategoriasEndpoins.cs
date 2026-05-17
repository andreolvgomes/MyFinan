using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using MyFinan.Repositories;

namespace MyFinan.Endpoints
{
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
}
