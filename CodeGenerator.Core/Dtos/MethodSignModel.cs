using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Core.Dtos
{
    public class MethodSignModel
    {
        public string Name { get; set; }
        public string ResponseType { get; set; }

        public List<ClassPropertyModel> Parameters { get; set; }
    }
}
