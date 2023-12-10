namespace Database.Data.Entities
{
    public class ObjectType
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public int Description { get; set; }
        public int? ParentId { get; set; }
        public ObjectType Parent { get; set; }
        public ICollection<ObjectType> Child { get; set; }

        public int ApiId { get; set; }
        public Api Api { get; set; }
    }
}
