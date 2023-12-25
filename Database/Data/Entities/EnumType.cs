using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Data.Entities
{

    public class EnumType
    {
        public EnumType()
        {
            Fields= new List<Field>();
            EnumFields = new List<EnumField>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }

        public ICollection<Field> Fields { get; set; }
        public ICollection<EnumField> EnumFields { get; set; }
    }

    public class EnumField
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Value { get; set; }
        public int? Order { get; set; }
        public string? Description { get; set; }
        public int? EnumTypeId { get; set; }
        public EnumType EnumType { get; set; }
    }
}
