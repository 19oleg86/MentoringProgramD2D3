using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTrees.Task2.ExpressionMapping.Converters
{
    public class StringToInt : ITypeConverter<string, int>
    {
        public int From(string source)
        {
            return int.Parse(source);
        }
    }
}
