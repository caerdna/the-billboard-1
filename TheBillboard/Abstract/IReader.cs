using TheBillboard.Models;

namespace TheBillboard.Abstract;

public interface IReader
{
    public Task<IEnumerable<Message>> QueryAsync();
}