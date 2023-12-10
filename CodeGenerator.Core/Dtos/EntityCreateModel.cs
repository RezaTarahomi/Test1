using System.Collections.Generic;

namespace CodeGenerator.Core.Dtos
{
    public class EntityCreateModel
    {
        public EntityCreateModel()
        {
            Properties = new List<ClassPropertyModel>();
        }
        public string Name { get; set; }
        public List<ClassPropertyModel> Properties { get; set; }


    }
}