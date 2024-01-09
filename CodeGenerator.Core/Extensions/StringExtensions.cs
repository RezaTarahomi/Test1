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

        public static string GetPluralForm(this string noun)
        {
            if (string.IsNullOrEmpty(noun))
                return noun;

            string pluralForm = noun;

            // Add more rules here for different pluralization patterns

            if (noun.EndsWith("s") || noun.EndsWith("sh") || noun.EndsWith("ch") || noun.EndsWith("x") || noun.EndsWith("z"))
            {
                pluralForm += "es"; // For nouns ending with 's', 'sh', 'ch', 'x', or 'z'
            }
            else if (noun.EndsWith("y"))
            {
                pluralForm = noun.Remove(noun.Length - 1) + "ies"; // For nouns ending with 'y', replace 'y' with 'ies'
            }
            else
            {
                pluralForm += "s"; // For regular pluralization, just add 's'
            }

            return pluralForm;
        }
    }
}
