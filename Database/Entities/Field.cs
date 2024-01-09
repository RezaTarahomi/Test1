namespace Database.Entities
{
    
    public class Field
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string? Description { get; set; }
        public bool IsParent { get; set; }
        public bool IsOneByOne { get; set; }
        public bool IsNullable { get; set; }
        public ICollection<Entity> Entities { get; set; }
        public Entity Parent { get; set; }
        public bool IsForeignKey { get; set; }
        public ICollection<Field> Fields { get; set; }
        public Field ForeignKey { get; set; }
        
        public Entity Entity { get; set; }
        public bool IsEnum { get; set; }
        public ICollection<EnumType> EnumTypes { get; set; }
        public EnumType EnumType { get; set; }
        public int? ParentId { get; set; }
        public int? ForeignKeyId { get; set; }
        public int EntityId { get; set; }
        public int? EnumTypeId { get; set; }
    }
}
