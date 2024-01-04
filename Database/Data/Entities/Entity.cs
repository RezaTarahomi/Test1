using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Data.Entities
{
    public class Entity
    {
        public Entity()
        {
            Fields = new HashSet<Field>();
          //  EntityParents = new HashSet<EntityParent>();
            EntityChilds = new HashSet<Field>();
        }
        public int Id { get; set; }
        [Display(Name="نام")]
        public string Name { get; set; }
        [Display(Name = "مسیر")]
        public string? Path { get; set; }
        public Domains? Domain { get; set; }
        public string? Description { get; set; }

        public ICollection<Field> Fields { get; set; }
         
        public ICollection<Field> EntityChilds { get; set; }
    }
}
