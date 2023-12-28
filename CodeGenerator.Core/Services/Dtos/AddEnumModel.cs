using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Core.Services.Dtos
{
    public class AddEnumFieldModel
    {     
      
        public int EnumTypeId { get; set; }
        public string Name { get; set; }
        public int? Value { get; set; }
        public int? Order { get; set; }
        public string? Description { get; set; }       
    }

    public class EditEnumFieldModel: AddEnumFieldModel
    {
        public int Id { get; set; }
        
    }
}
