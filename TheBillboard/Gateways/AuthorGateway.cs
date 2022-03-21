using System.Data;
using TheBillboard.Abstract;
using TheBillboard.Models;

namespace TheBillboard.Gateways
{
    public class AuthorGateway : IAuthorGateway
    {
        private readonly IReader _reader;
        private readonly IWriter _writer;

        public AuthorGateway(IReader reader, IWriter writer)
        {
            _reader = reader;
            _writer = writer;
        }

        //OLD
        private List<Author> _authors = new List<Author>()
        {
            new Author("WARNING_1", "", 1),
            new Author("WARNING_2", "", 2),
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

        //public IEnumerable<Author> GetAll() => _authors;

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            const string query = "SELECT * FROM \"Authors\"";
            return await _reader.QueryAsync<Author>(query, Map);
        }

        public Author? GetById(int id) => _authors.SingleOrDefault(x => x.Id == id);

        public async Task<Author?> GetByIdAsync(int id)
        {
            string query = $"SELECT * FROM \"Authors\" WHERE \"Id\" = @id";
            return await _reader.QueryByIdAsync<Author>(query, Map, id);
        }

        //public void Update(Author author)
        //{
        //    _authors = _authors
        //        .Where(x => x.Id != message.Id)
        //        .ToList();

        //    message = message with { UpdatedAt = DateTime.Now };

        //    _messages.Add(message);
        //}

        private Author Map(IDataReader dr)
        {
            return new Author
            {
                Id = dr["id"] as int?,
                Name = dr["name"].ToString()!,
                Surname = dr["surname"].ToString()!,
                //CreatedAt = dr["createdAt"] as DateTime?,
                //UpdatedAt = dr["updatedAt"] as DateTime?,
            };
        }

    }
}
