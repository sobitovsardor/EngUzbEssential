using PolyglotEssential.Domain.Common;

namespace PolyglotEssential.Domain.Entities
{
    public class Level : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }
}
