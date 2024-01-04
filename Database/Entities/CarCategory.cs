using System.ComponentModel.DataAnnotations;


namespace Database.Entities
{
    
    public class CarCategory
    {
        public int Id { get; set; }
        [Display(Name="عنوان")]
        public string Caption { get; set; }
        [Display(Name="توضیحات")]
        public string? Description { get; set; }
    }
}
