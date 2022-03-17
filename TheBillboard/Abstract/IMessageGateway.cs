using TheBillboard.Models;

namespace TheBillboard.Abstract;

public interface IMessageGateway
{
    IEnumerable<Message> GetAll();
    Message? GetById(int id);
    Message Create(Message message);
    void Update(Message message);
    void Delete(int id);
}