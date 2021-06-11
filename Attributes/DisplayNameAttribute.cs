using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALARM
{
    /// <summary>
    /// Usage: <br/>
    /// public static string DisplayName() => "Display Name";
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class DisplayNameAttribute : Attribute
    {
        /// <summary>
        /// The default function for creating names for class types
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string Default(Type type)
        {
            string tName = type.Name;
            List<char> nameBuilder = new List<char>();
            for (int i = 0; i < tName.Length; i++)
            {
                char c = tName[i];
                if (i == 0)
                {
                    nameBuilder.Add(c);
                    continue;
                }

                if (c == '_')
                {
                    nameBuilder.Add(' ');
                    continue;
                }

                char prev = tName[i - 1];

                if (char.IsLetter(c))
                {
                    if (!char.IsLetter(prev))
                    {
                        nameBuilder.Add(' ');
                        nameBuilder.Add(c);
                        continue;
                    }
                }

                if (char.IsUpper(c))
                {
                    bool prevUpper = char.IsUpper(prev);
                    bool nextLower = i + 1 < tName.Length ? char.IsLower(tName[i + 1]) : false;

                    if (nextLower)
                    {
                        nameBuilder.Add(' ');
                        nameBuilder.Add(c);
                        continue;
                    }
                    else if (!nextLower && !prevUpper)
                    {
                        nameBuilder.Add(' ');
                        nameBuilder.Add(c);
                        continue;
                    }
                    else
                    {
                        nameBuilder.Add(c);
                        continue;
                    }
                }

                if (char.IsDigit(c))
                {
                    if (!char.IsDigit(prev)) // adds a space before digits if the previous isn't a digit
                    {
                        nameBuilder.Add(' ');
                        nameBuilder.Add(c);
                        continue;
                    }
                    else
                    {
                        nameBuilder.Add(c);
                        continue;
                    }
                }

                nameBuilder.Add(c);
            }

            return new string(nameBuilder.ToArray());
        }
    }
}
