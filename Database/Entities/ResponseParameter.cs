namespace Database.Entities
{
    
    public class ResponseParameter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PrimitiveType { get; set; }
        public ICollection<ObjectType> ObjectTypes { get; set; }
        public ObjectType ObjectType { get; set; }
        public bool IsList { get; set; }
        public ICollection<Api> Apis { get; set; }
        public Api Api { get; set; }
        public int? ObjectTypeId { get; set; }
        public int ApiId { get; set; }
        public int? EntityId { get; set; }
        public Entity Entity { get; set; }
    }
}
