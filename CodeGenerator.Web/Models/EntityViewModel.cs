using Database.Data.Entities;

namespace CodeGenerator.Web.Models
{
    public class EntityViewModel
    {
        public EntityViewModel()
        {
            Fields= new List<FieldViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Path { get; set; }
        public Domains? Domain { get; set; }
        public string? Description { get; set; }

        public int? ParentId { get; set; }
        public bool IsOneToOne { get; set; }

        public List<FieldViewModel> Fields { get; set; }
    }

   
}
