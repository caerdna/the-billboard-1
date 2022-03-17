namespace TheBillboard.Models
{
    public record MessageCreationViewModel(Message Message, IEnumerable<Author> Authors);
}
