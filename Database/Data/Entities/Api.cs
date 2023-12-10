namespace Database.Data.Entities
{
    public class Api
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public ICollection<RequestParameter> RequestParameters { get; set; }
        public ICollection<ResponseParameter> ResponseParameters { get; set; }
        public ICollection<ObjectType> ObjectTypes { get; set; }


    }
}
