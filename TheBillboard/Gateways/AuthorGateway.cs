using TheBillboard.Abstract;
using TheBillboard.Models;

namespace TheBillboard.Gateways
{
    public class AuthorGateway : IAuthorGateway
    {
        private List<Author> _authors = new List<Author>()
        {
            new Author("Alberto", "", 1),
            new Author("Marco", "Pacchialat", 2),
        };

        private int nextId = 3;

        public Author Create(Author author)
        {
            author = author with { Id = nextId };
            _authors.Add(author);
            nextId++;
            return (author);
        }

        public void Delete(int id) =>
            _authors = _authors
                .Where(x => x.Id != id)
                .ToList();

        public IEnumerable<Author> GetAll() => _authors;

        public Author? GetById(int id) => _authors.SingleOrDefault(x => x.Id == id);

        //public void Update(Author author)
        //{
        //    _authors = _authors
        //        .Where(x => x.Id != message.Id)
        //        .ToList();

        //    message = message with { UpdatedAt = DateTime.Now };

        //    _messages.Add(message);
        //}
    }
}
