using TheBillboard.Models;

namespace TheBillboard.Abstract
{
    public interface IAuthorGateway
    {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<Author?> GetByIdAsync(int id);
        Task<bool> CreateAsync(Author author);
        Task<bool> UpdateAsync(Author author);
        Task<bool> DeleteAsync(int id);
    }
}
