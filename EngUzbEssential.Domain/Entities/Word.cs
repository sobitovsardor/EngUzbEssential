using EngUzbEssential.Domain.Common;

namespace EngUzbEssential.Domain.Entities
{
    public class Word : BaseEntity
    {
        public string EngWord { get; set; } = string.Empty;
        public string UzbWord { get; set; } = string.Empty;
        public int CorrectPoints { get; set; }
        public int LevelId { get; set; }
    }
}
