using System.CodeDom;

namespace CodeGenerator.Core.Dtos
{
    public class ClassPropertyModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string? Description { get; set; }

        public MemberAttributes? MemberAttributes { get; set; }
    }
}