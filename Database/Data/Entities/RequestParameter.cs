namespace Database.Data.Entities
{
    public class RequestParameter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
        public bool Filterable { get; set; }
        public bool Sortable { get; set; }

        public AttributeType? AttributeType { get; set; }

        public int ApiId { get; set; }
        public Api Api { get; set; }

    }
}
