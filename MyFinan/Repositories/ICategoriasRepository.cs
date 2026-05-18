using Dapper;
using DapperExtensions;
using MyFinan.Models.Entities;
using System.Data;

namespace MyFinan.Repositories
{
    public interface ICategoriasRepository
    {
        Task<Categorias> Insert(Categorias categorias);
        Task<IEnumerable<Categorias>> GetAll();
        Task Update(Categorias categorias);
        Task Delete(long id);
    }

    public class CategoriasRepository : ICategoriasRepository
    {
        private readonly IDbConnection _connection;

        public CategoriasRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<Categorias>> GetAll()
        {
            var items = await _connection.GetAllAsync<Categorias>();
            return items.OrderBy(item => item.Id);
        }

        public async Task Update(Categorias categorias)
        {
            await _connection.UpdateAsync(categorias);
        }

        public async Task Delete(long id)
        {
            await _connection.ExecuteAsync("delete from myfinan.categorias where id = @id", new { id = id });
        }

        public async Task<Categorias> Insert(Categorias categorias)
        {
            categorias.Id = await _connection.InsertAsync(categorias);
            return categorias;
        }
    }
}
