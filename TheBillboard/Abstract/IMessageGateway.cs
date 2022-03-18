using TheBillboard.Models;

namespace TheBillboard.Abstract;

public interface IMessageGateway
{
    Task<IEnumerable<Message>> GetAll();
    Task<Message?> GetByIdAsync(int id);
    Task<bool> Create(Message message);
    void Update(Message message);
    void Delete(int id);
}