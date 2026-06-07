using MyFinan.Infrastructure;
using MyFinan.Models.Dtos;
using MyFinan.Models.Entities;
using MyFinan.Parsers;
using MyFinan.Repositories;

namespace MyFinan.Services
{
    public class FaturasService
    {
        private readonly S3StorageService _storageService;
        private readonly FaturasInterParser _faturaInterParser;
        private readonly ITransacoesRepository _transacoesRepository;
        private readonly ICategoriasRepository _categoriasRepository;

        public FaturasService(S3StorageService storageService,
            FaturasInterParser faturaInterParser,
            ITransacoesRepository transacoesRepository,
            ICategoriasRepository categoriasRepository)
        {
            _storageService = storageService;
            _faturaInterParser = faturaInterParser;
            _transacoesRepository = transacoesRepository;
            _categoriasRepository = categoriasRepository;
        }

        public async Task<List<TransacaoResponse>> Execute()
        {
            var stream = await _storageService.ReadFile("myfinan-faturas", "fatura.pdf");
            var transacoesDto = await _faturaInterParser.Extrair(stream);

            var categorias = await _categoriasRepository.GetAll();

            foreach (var item in transacoesDto)
            {
                await _transacoesRepository.Insert(new Transacoes
                {
                    Data = item.Data,
                    Beneficiario = item.Beneficiario,
                    Valor = item.Valor,
                    Categoria_Id = IdentificarCategoriaDinamicaAsync(item.Beneficiario, categorias)
                });
            }
            return transacoesDto;
        }

        public int IdentificarCategoriaDinamicaAsync(string beneficiario, IEnumerable<Categorias> categorias)
        {
            if (string.IsNullOrEmpty(beneficiario)) return 0;

            var nomeMinusculo = beneficiario.ToLower();

            foreach (var regra in categorias)
            {
                var termos = regra.Nome_Contains
                                  .Split(';', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(t => t.Trim().ToLower());

                if (termos.Any(termo => nomeMinusculo.Contains(termo)))
                    return regra.Id;
            }

            return 0;
        }
    }
}