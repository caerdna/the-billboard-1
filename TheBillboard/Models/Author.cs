using Microsoft.AspNetCore.Mvc;

namespace TheBillboard.Models
{
    public record Author(
        string Name = "",
        string Surname = "",
        DateTime? CreatedAt = default,
        DateTime? UpdatedAt = default,
        int? Id = default
        )
    {
        public string FormattedCreatedAt => CreatedAt.HasValue
                ? CreatedAt.Value.ToString("yyyy-MM-dd HH:mm")
                : string.Empty;

        public string FormattedUpdatedAt => UpdatedAt.HasValue
            ? UpdatedAt.Value.ToString("yyyy-MM-dd HH:mm")
            : string.Empty;

        public override string ToString()
        {
            return Name + " " + Surname;
        }
    }
}
