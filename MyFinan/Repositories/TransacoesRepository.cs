using Dapper;
using DapperExtensions;
using MyFinan.Models.Entities;
using System.Collections;
using System.Data;

namespace MyFinan.Repositories
{
    //public class NullableDateTimeTypeHandler : SqlMapper.TypeHandler<DateTime?>
    //{
    //    public override void SetValue(IDbDataParameter parameter, DateTime? value)
    //    {
    //        if (value.HasValue)
    //            parameter.Value = DateOnly.FromDateTime(value.Value);
    //        else
    //            parameter.Value = DBNull.Value;
    //    }

    //    public override DateTime? Parse(object value)
    //    {
    //        if (value == null || value == DBNull.Value)
    //            return null;

    //        if (value is DateOnly dateOnly)
    //            return dateOnly.ToDateTime(TimeOnly.MinValue);

    //        return Convert.ToDateTime(value);
    //    }
    //}

    public interface ITransacoesRepository
    {
        Task Insert(Transacoes transacoes);
        Task Update(Transacoes transacoes);
        Task<Transacoes> GetById(long id);
        Task Delete(Transacoes transacoes);
        Task<IEnumerable<Transacoes>> GetAll();
        Task<IEnumerable<dynamic>> AgruparPorBeneficiario();
        Task<IEnumerable<object>> ObterTodos();
        Task<IEnumerable<object>> Dash();
    }

    public class TransacoesRepository : ITransacoesRepository
    {
        private readonly IDbConnection _connection;

        public TransacoesRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task Insert(Transacoes transacoes)
        {
            transacoes.Id = await _connection.InsertAsync(transacoes);
        }

        public async Task Update(Transacoes transacoes)
        {
            await _connection.UpdateAsync(transacoes);
        }

        public async Task<Transacoes> GetById(long id)
        {
            return await _connection.FindAsync<Transacoes>(id);
        }

        public async Task Delete(Transacoes transacoes)
        {
            await _connection.DeleteAsync(transacoes);
        }

        public async Task<IEnumerable<object>> ObterTodos()
        {
            var sql = @"
select 
    coalesce(categorias.nome, '') as Categoria,
    coalesce(categorias.cor, '') as Cor,
    transacoes.*
from myfinan.transacoes transacoes
left join myfinan.categorias categorias on transacoes.categoria_id = categorias.id
order by transacoes.id";

            return await _connection.QueryAsync(sql);
        }

        public async Task<IEnumerable<Transacoes>> GetAll()
        {
            return await _connection.GetAllAsync<Transacoes>();
        }

        public async Task<IEnumerable<dynamic>> AgruparPorBeneficiario()
        {
            var result = await _connection.QueryAsync("select beneficiario, sum(valor) from myfinan.transacoes\r\ngroup by beneficiario");
            return result;
        }

        public async Task<IEnumerable<object>> Dash()
        {
            var sql = @"
select 
    coalesce(categorias.nome, 'Sem categoria') as Categoria,
    coalesce(categorias.cor, '#94A3B8') as Cor,
    sum(transacoes.valor) as Valor
from myfinan.transacoes transacoes
left join myfinan.categorias categorias on transacoes.categoria_id = categorias.id
group by categorias.nome, categorias.cor";

            return await _connection.QueryAsync(sql);
        }
    }
}