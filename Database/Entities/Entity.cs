using System.ComponentModel.DataAnnotations;

namespace Database.Entities
{
    public class Entity
    {
        public int Id { get; set; }

        [Display(Name = "نام")]
        public string Name { get; set; }

        [Display(Name = "مسیر")]
        public string? Path { get; set; }
        public Domains? Domain { get; set; }
        public string? Description { get; set; }
        public ICollection<Field> Fields { get; set; }
        public ICollection<Field> EntityChilds { get; set; }
        public ICollection<Entity> Entities { get; set; }
    }
}