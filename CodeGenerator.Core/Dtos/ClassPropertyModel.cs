using System.CodeDom;

namespace CodeGenerator.Core.Dtos
{
    public class ClassPropertyModel
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public MemberAttributes? MemberAttributes { get; set; }
    }
}