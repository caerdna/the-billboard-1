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

        public async Task<bool> WriteAsync<TEntity>(string query, TEntity entity , IEnumerable<(string name, object? value)> parameterList)
        {
            //query = "INSERT INTO \"Messages\"(\"Title\", \"Body\", \"CreatedAt\", \"UpdatedAt\", \"AuthorId\") VALUES (@Title, @Body, '2022-03-17 00:00:00.000000', '2022-03-17 00:00:00.000000', 4)";

            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand(query, connection);

            //command.Parameters.Add(new SqlParameter("@Title", "Title 1"));
            //command.Parameters.Add(new SqlParameter("@Body", "Body 1"));

            foreach (var parameter in parameterList)
            {
                command.Parameters.Add(new SqlParameter(parameter.name, parameter.value));
            }

            await connection.OpenAsync();
            //await command.PrepareAsync();
            await command.ExecuteNonQueryAsync();

            return true;
        }
    }
}
