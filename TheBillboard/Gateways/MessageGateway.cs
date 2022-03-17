using TheBillboard.Abstract;
using TheBillboard.Models;

namespace TheBillboard.Gateways;

public class MessageGateway : IMessageGateway  
{
    private ICollection<Message> _messages = new List<Message>()
    {
        new("Hello  World!", "What A Wonderful World!", 1, DateTime.Now.AddHours(-2), DateTime.Now.AddHours(-1), 1),
        new("Hello  World!", "What A Wonderful World!", 1, DateTime.Now, DateTime.Now, 2),
    };
    private int nextId = 3;
    
    public IEnumerable<Message> GetAll() => _messages;

    public Message? GetById(int id) => _messages.SingleOrDefault(message => message.Id == id);

    public Message Create(Message message)
    {
        message = message with { Id = nextId, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        nextId++;
        _messages.Add(message);
        return message;
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
}