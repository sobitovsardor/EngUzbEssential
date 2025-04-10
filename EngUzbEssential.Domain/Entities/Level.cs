using EngUzbEssential.Domain.Common;

namespace EngUzbEssential.Domain.Entities
{
    public class Level : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }
}
