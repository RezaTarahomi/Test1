namespace Database.Data.Entities
{
    public class ResponseParameter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PrimitiveType { get; set; }
        public int? ObjectTypeId { get; set; }
        public ObjectType ObjectType { get; set; }
        public bool IsList { get; set; }
        public int ApiId { get; set; }
        public Api Api { get; set; }

    }
}
