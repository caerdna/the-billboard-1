using Microsoft.Extensions.Options;
using Npgsql;
using TheBillboard.Options;

namespace TheBillboard.Writers;

public class PostgresWriter
{
    private readonly string _connectionString;

    public PostgresWriter(IOptions<ConnectionStringOptions> options)
    {
        _connectionString = options.Value.DefaultDatabase;
    }

    public async Task<bool> WriteAsync<TEntity>(string query, TEntity entity)
    {
        query = @"INSERT INTO public.""Message""(""Title"", ""Body"", ""CreatedAt"", ""UpdatedAt"", ""AuthorId"") VALUES (@Title, @Body, '2022-03-17 00:00:00.000000', '2022-03-17 00:00:00.000000', 4)";
        
        await using var connection = new NpgsqlConnection(_connectionString);
        await using var command = new NpgsqlCommand(query, connection);

        command.Parameters.Add(new NpgsqlParameter("Title", "Title 1"));
        command.Parameters.Add(new NpgsqlParameter("Body", "Body 1"));
        
        await connection.OpenAsync();
                                                  
        await command.PrepareAsync();             
        await command.ExecuteNonQueryAsync();

        return true;
    }
}