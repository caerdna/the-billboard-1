using System.Data;
using TheBillboard.Abstract;
using TheBillboard.Models;

namespace TheBillboard.Gateways;

public class MessageGateway : IMessageGateway
{
    private readonly IReader _reader;
    private readonly IWriter _writer;

    public MessageGateway(IReader reader, IWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public IAsyncEnumerable<Message> GetAllAsync()
    {
        const string query =
            "SELECT * "
            + "FROM \"Messages\" "
            + "JOIN \"Authors\" A ON A.\"Id\" = \"Messages\".\"AuthorId\"";
        return _reader.QueryAsync<Message>(query, Map);
    }

    public async Task<Message?> GetByIdAsync(int id)
    {
        const string query =
            "SELECT * "
            + "FROM \"Messages\" "
            + "JOIN \"Authors\" A ON A.\"Id\" = \"Messages\".\"AuthorId\" "
            + "WHERE \"Messages\".\"Id\" = @id";
        return await _reader.QueryByIdAsync<Message>(query, Map, id);
    }

    public async Task<bool> CreateAsync(Message message)
    {
        const string query =
            "INSERT INTO \"Messages\"(\"Title\", \"Body\", \"CreatedAt\", \"UpdatedAt\", \"AuthorId\") "
            + "VALUES (@Title, @Body, @CreatedAt, @UpdatedAt, @AuthorId)";

        var parameterList = new List<(string, object?)>
        {
            ("@Title", message.Title),
            ("@Body", message.Body),
            ("@CreatedAt", DateTime.Now),
            ("@UpdatedAt", DateTime.Now),
            ("@AuthorId", message.AuthorId)
        };
        return await _writer.WriteAsync<Message>(query, message, parameterList);
    }

    public async Task<bool> UpdateAsync(Message message)
    {
        const string query =
            "UPDATE \"Messages\" "
            + "SET \"Title\" = @Title, \"Body\" = @Body, \"UpdatedAt\" = @UpdatedAt, \"AuthorId\" = @AuthorId "
            + "WHERE \"Id\" = @id";

        var parameterList = new List<(string, object?)>
        {
            ("@Title", message.Title),
            ("@Body", message.Body),
            ("@UpdatedAt", DateTime.Now),
            ("@AuthorId", message.AuthorId),
            ("@Id", message.Id)
        };
        return await _writer.WriteAsync<Message>(query, message, parameterList);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string query = "DELETE FROM \"Messages\" WHERE \"Id\" = @id";

        var parameterList = new List<(string, object?)>
        {
            ("@Id", id)
        };
        return await _writer.WriteAsync<Message>(query, new Message(), parameterList);
    }



    private Message Map(IDataReader dr)
    {
        return new Message
        {
            Id = dr["id"] as int?,
            Body = dr["body"].ToString()!,
            Title = dr["title"].ToString()!,
            CreatedAt = dr["createdAt"] as DateTime?,
            UpdatedAt = dr["updatedAt"] as DateTime?,
            AuthorId = (int)dr["authorId"],
            Author = new Author
            {
                Id = dr["authorId"] as int?,
                Name = dr["name"].ToString()!,
                Surname = dr["surname"].ToString()!,
            }
        };
    }

}