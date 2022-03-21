using System.Data;
using TheBillboard.Abstract;
using TheBillboard.Models;

namespace TheBillboard.Gateways;

public class MessageGateway : IMessageGateway
{
    private readonly IReader _reader;
    private readonly IWriter _writer;

    //OLD
    private ICollection<Message> _messages = new List<Message>()
    {
        new("Hello  World!", "What A Wonderful World!", 1, default, DateTime.Now.AddHours(-2), DateTime.Now.AddHours(-1), 1),
        new("Hello  World!", "What A Wonderful World!", 1, default, DateTime.Now, DateTime.Now, 2),
    };
    private int _nextId = 3;

    public MessageGateway(IReader reader, IWriter writer)
    {
        _reader = reader;
        _writer = writer;
    }

    public async Task<IEnumerable<Message>> GetAllAsync()
    {
        const string query = @"select * from ""Messages"" join ""Authors"" A on A.""Id"" = ""Messages"".""AuthorId""";
        return await _reader.QueryAsync<Message>(query, Map);
    }

    public async Task<Message?> GetByIdAsync(int id)
    {
        const string query = $"SELECT * FROM \"Messages\" JOIN \"Authors\" A ON A.\"Id\" = \"Messages\".\"AuthorId\" WHERE \"Messages\".\"Id\" = @id";
        return await _reader.QueryByIdAsync<Message>(query, Map, id);
    }

    public Task<bool> Create(Message message)
    {
        const string query = "INSERT INTO \"Messages\"(\"Title\", \"Body\", \"CreatedAt\", \"UpdatedAt\", \"AuthorId\") VALUES (@Title, @Body, @CreatedAt, @UpdatedAt, @AuthorId)";

        var parameterList = new List<(string, object?)>
        {
            ("@Title", message.Title),
            ("@Body", message.Body),
            ("@CreatedAt", DateTime.Now),
            ("@UpdatedAt", DateTime.Now),
            ("@AuthorId", message.AuthorId)
        };
        return _writer.WriteAsync<Message>(query, message, parameterList);
    }

    public void Delete(int id) =>
        _messages = _messages
            .Where(message => message.Id != id)
            .ToList();

    public void Update(Message message)
    {
        _messages = _messages
            .Where(m => m.Id != message.Id)
            .ToList();

        message = message with { UpdatedAt = DateTime.Now };

        _messages.Add(message);
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