public interface IWriter
{
    Task<bool> WriteAsync<TEntity>(string query, TEntity entity, IEnumerable<(string name, object? value)> parametersS);
}