using Database.Data.Entities;

namespace Database.Entities
{
    
    public class EnumType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        public ICollection<Field> Fields { get; set; }
        public ICollection<EnumField> EnumFields { get; set; }
      
    }
}
