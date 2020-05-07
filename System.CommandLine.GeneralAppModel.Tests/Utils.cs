using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.CommandLine.GeneralAppModel.Tests
{
  public  static class  Utils
    {
        public static string CompareLists<T>(this IEnumerable<T> list1, IEnumerable<T> list2, string name)
        {
            var a1 = list1.ToArray();
            var a2 = list2.ToArray();
            if (a1.Length != a2.Length)
            {
                return $"The length of {name} {list1.Count()} does not equal {list2.Count()}";
            }
            for (int i = 0; i < list1.Count(); i++)
            {
                if (!(a1[i].Equals(a2[i])))
                {
                    return $"Position {i} of {name} is {a1}, while {a2} was expected";
                }

            }
            return null;
        }

    }
}
