﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Data.Entities
{
    public class Field
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string? Description { get; set; }

        public int EntityId { get; set; }
        public Entity Entity { get; set; }

        public int? EnumTypeId { get; set; }
        public EnumType EnumType { get; set; }

    }
}