using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Data.Entities
{
    public class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public Domains? Domain { get; set; }

        public ICollection<Field> Fields { get; set; }
        public ICollection<EntityParent> EntityParents { get; set; }
        public ICollection<EntityParent> EntityChilds { get; set; }
    }
}
