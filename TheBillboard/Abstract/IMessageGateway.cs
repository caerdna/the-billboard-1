using TheBillboard.Models;

namespace TheBillboard.Abstract;

public interface IMessageGateway
{
    Task<IEnumerable<Message>> GetAllAsync();
    Task<Message?> GetByIdAsync(int id);
    Task<bool> CreateAsync(Message message);
    Task<bool> UpdateAsync(Message message);
    Task<bool> DeleteAsync(int id);
}