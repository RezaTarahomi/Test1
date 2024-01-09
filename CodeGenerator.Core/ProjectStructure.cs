using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Core
{
    public class ProjectStructure
    {
        public static string Application => "Application";
        public static string Domain => "Database";
        public static string Persistance => "Persistance";

        public static string EntitiesFolder => "Entities";
        public static string ConfigFolder => "Config";
        public static string Id_CaptionPath => "Common";

        public static string DbContextClassName => "CodeGeneratorDbContext";
    }
}
