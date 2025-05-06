using PolyglotEssential.Domain.Common;

namespace PolyglotEssential.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
    }
}
