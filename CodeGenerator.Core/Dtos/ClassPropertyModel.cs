using Database.Data.Entities;
using System.CodeDom;

namespace CodeGenerator.Core.Dtos
{
    public class ClassPropertyModel
    {
        public ClassPropertyModel()
        {
            EnumType = new EnumType();
        }
        public string Name { get; set; }
        public string Type { get; set; }
        public string? Description { get; set; }
        public bool IsEnum { get; set; }

        public EnumType EnumType { get; set; }
        public MemberAttributes? MemberAttributes { get; set; }

        
    }
}