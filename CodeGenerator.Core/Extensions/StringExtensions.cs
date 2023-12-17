using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToLowerFirst(this string input)
        {
            if (input == null)
                return string.Empty;

          return  char.ToLower(input[0]) + input.Substring(1);
        }
        public static string Underscore(this string input)
        {
            if (input == null)
                return string.Empty;

            return "_" + input.ToLowerFirst();
        }
    }
}
