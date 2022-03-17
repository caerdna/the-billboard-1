public interface IWriter
{
    Task<bool> WriteAsync<TEntity>(string query, TEntity entity);
}