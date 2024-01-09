namespace Database.Entities
{
    
    public class ObjectType
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public int Description { get; set; }
        public ICollection<ObjectType> ObjectTypes { get; set; }
        public ObjectType Parent { get; set; }
        public ICollection<ObjectType> Child { get; set; }
        public ICollection<Api> Apis { get; set; }
        public Api Api { get; set; }
        public int? ParentId { get; set; }
        public int ApiId { get; set; }
    }
}
