using Npgsql;
using TheBillboard.Models;

namespace TheBillboard.Abstract;

public interface IReader
{
    public Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string query, Func<NpgsqlDataReader, TEntity> selector);
}