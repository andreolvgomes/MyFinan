using DapperExtensions;
using MyFinan.Models.Entities;
using System.Data;

namespace MyFinan.Repositories
{
    public interface ICategoriasRepository
    {
        Task<IEnumerable<Categorias>> GetAll();
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
            return await _connection.GetAllAsync<Categorias>();
        }
    }
}
