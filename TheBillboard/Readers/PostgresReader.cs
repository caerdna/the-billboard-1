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

    public async Task<IEnumerable<Message>> QueryAsync()
    {
        var messages = new HashSet<Message>(); 
        
        await using var connection = new NpgsqlConnection(_connectionString);

        const string query = @"select * from ""Message""";

        await using var command = new NpgsqlCommand(query, connection);

        await connection.OpenAsync();
        await using var dr = command.ExecuteReader();
        while (dr.Read())
        {
            var message = new Message
            {
                Id = dr["id"] as int?,
                Body = dr["body"].ToString()!,
                Title = dr["title"].ToString()!,
                CreatedAt = dr["createdAt"] as DateTime?,
                UpdatedAt = dr["updatedAt"] as DateTime?,
                AuthorId = (int) dr["authorId"],
            };

            messages.Add(message);
        }

        await connection.CloseAsync();
        await connection.DisposeAsync();
        
        return messages;
    }
}