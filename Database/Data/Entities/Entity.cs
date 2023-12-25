using System;
using System.Collections.Generic;
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
            EntityParents = new HashSet<EntityParent>();
            EntityChilds = new HashSet<EntityParent>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Path { get; set; }
        public Domains? Domain { get; set; }
        public string? Description { get; set; }

        public ICollection<Field> Fields { get; set; }
        public ICollection<EntityParent> EntityParents { get; set; }
        public ICollection<EntityParent> EntityChilds { get; set; }
    }
}
