namespace Database.Entities
{
    
    public class VehicleUserType2
    {
        [System.ComponentModel.DataAnnotations.DisplayAttribute(Name="عنوان")]
        public string Caption { get; set; }
        public int Id { get; set; }
        public string? Description { get; set; }
        public AppType Application { get; set; }
    }
}
