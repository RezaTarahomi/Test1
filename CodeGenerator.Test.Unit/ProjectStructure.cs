using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Test.Unit
{
    public class ProjectStructure
    {
        public static string Application => "Application";
        public static string Domain => "Database";
        public static string Persistance => "Persistance";

        public static string EntitiesFolder => "Entities";

        public static string ApiPath => "CodeGenerator\\Api";
    }
}
