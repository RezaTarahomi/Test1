using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Core.Services.Dtos
{
    public class AddFieldModel
    {
       
        public int EntityId { get; set; }
        public int? EnumTypeId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string? Description { get; set; }
        public bool IsEnum { get; set; }
    }

    public class EditFieldModel:AddFieldModel
    {
        public int Id { get; set; }
        
    }
}
