using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using TheBillboard.Options;


namespace TheBillboard.Writers
{
    public class SQLWriter : IWriter
    {
        private readonly string _connectionString;

        public SQLWriter(IOptions<ConnectionStringOptions> options)
        {
            _connectionString = options.Value.DefaultDatabase;
        }

        public async Task<bool> WriteAsync<TEntity>(string query, TEntity entity, IEnumerable<(string name, object? value)> parameterList)
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand(query, connection);

            foreach (var (name, value) in parameterList)
            {
                command.Parameters.Add(new SqlParameter(name, value));
            }

            await connection.OpenAsync();           
            await command.ExecuteNonQueryAsync();

            return true;
        }
    }
}
