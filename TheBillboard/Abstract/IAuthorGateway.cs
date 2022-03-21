using TheBillboard.Models;

namespace TheBillboard.Abstract
{
    public interface IAuthorGateway
    {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<Author?> GetByIdAsync(int id);

        Author Create(Author author);
        void Delete(int id);
    }
}
