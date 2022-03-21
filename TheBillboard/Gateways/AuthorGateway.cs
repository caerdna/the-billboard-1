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

        public IAsyncEnumerable<Author> GetAllAsync()
        {
            const string query =
                   "SELECT * "
                + " FROM \"Authors\"";
             return  _reader.QueryAsync<Author>(query, Map);
        }

        public async Task<Author?> GetByIdAsync(int id)
        {
            string query =
                "SELECT * "
                + " FROM \"Authors\" WHERE \"Id\" = @id";
            return await _reader.QueryByIdAsync<Author>(query, Map, id);
        }

        public async Task<bool> CreateAsync(Author author)
        {
            const string query =
                "INSERT INTO \"Authors\"(\"Name\", \"Surname\", \"CreatedAt\", \"UpdatedAt\") "
                + " VALUES (@Name, @Surname, @CreatedAt, @UpdatedAt)";

            var parameterList = new List<(string, object?)>
        {
            ("@Name", author.Name),
            ("@Surname", author.Surname),
            ("@CreatedAt", DateTime.Now),
            ("@UpdatedAt", DateTime.Now)
        };
            return await _writer.WriteAsync<Author>(query, author, parameterList);
        }

        public async Task<bool> UpdateAsync(Author author)
        {
            const string query =
                "UPDATE \"Authors\" "
                + " SET \"Name\" = @Name, \"Surname\" = @Surname, \"UpdatedAt\" = @UpdatedAt "
                + " WHERE \"Id\" = @id";

            var parameterList = new List<(string, object?)>
        {
            ("@Name", author.Name),
            ("@Surname", author.Surname),
            ("@UpdatedAt", DateTime.Now),
            ("@Id", author.Id)
        };
            return await _writer.WriteAsync<Author>(query, author, parameterList);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string query =
                "DELETE "
                + " FROM \"Authors\" "
                + " WHERE \"Id\" = @id";
            var parameterList = new List<(string, object?)>
        {
            ("@Id", id)
        };
            return await _writer.WriteAsync<Author>(query, new Author(), parameterList);
        }

        private Author Map(IDataReader dr)
        {
            return new Author
            {
                Id = dr["id"] as int?,
                Name = dr["name"].ToString()!,
                Surname = dr["surname"].ToString()!,
                CreatedAt = dr["createdAt"] as DateTime?,
                UpdatedAt = dr["updatedAt"] as DateTime?,
            };
        }

    }
}
