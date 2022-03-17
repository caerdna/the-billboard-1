using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Npgsql;
using TheBillboard.Abstract;
using TheBillboard.Models;
using TheBillboard.Options;

namespace TheBillboard.Readers;

public class PostgresReader : IReader
{
    private readonly string _connectionString;

    public PostgresReader(IOptions<ConnectionStringOptions> options)
    {
        _connectionString = options.Value.DefaultDatabase;
    }

    public async Task<IEnumerable<TEntity>> QueryAsync<TEntity>(string query, Func<NpgsqlDataReader, TEntity> selector)
    {
        var messages = new HashSet<TEntity>(); 
        
        await using var connection = new NpgsqlConnection(_connectionString);

        await using var command = new NpgsqlCommand(query, connection);

        await connection.OpenAsync();
        await using var dr = command.ExecuteReader();
        while (await dr.ReadAsync())
        {
            var message = selector(dr);
            messages.Add(message);
        }

        await connection.CloseAsync();
        await connection.DisposeAsync();
        
        return messages;
    }
}