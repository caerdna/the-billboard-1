using TheBillboard.Models;

namespace TheBillboard.Abstract
{
    public interface IAuthorGateway
    {
        public IEnumerable<Author> GetAll();

        Author? GetById(int id);

        Author Create(Author author);

        void Delete(int id);
    }
}
