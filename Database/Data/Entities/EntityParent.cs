using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Data.Entities
{
    public class EntityParent
    {
        public int Id { get; set; }
        public bool OneToOne { get; set; }

        public string? ForeginKey { get; set; }

        public Entity Entity { get; set; }
        public int EntityId { get; set; }

        public Entity Parent { get; set; }
        public int ParentId { get; set; }
    }
}
