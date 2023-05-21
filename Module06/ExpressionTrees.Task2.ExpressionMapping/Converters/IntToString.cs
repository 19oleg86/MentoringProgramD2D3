using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTrees.Task2.ExpressionMapping.Converters
{
    public class IntToString : ITypeConverter<int, string>
    {
        public string From(int source)
        {
            return source.ToString();
        }
    }
}
