// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpMetal.Operators.Selection
{
    public static class SelectionFactory
    {
        public static BaseSelection GetSelectionOperator(String name, Dictionary<string, object> parameters)
        {
            Type t = typeof (BaseSelection);
            BaseSelection selection = AppDomain.CurrentDomain.GetAssemblies()
                                               .SelectMany(x => x.GetTypes())
                                               .Where(
                                                   x =>
                                                   t.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract &&
                                                   x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                                               .Select(
                                                   a => Activator.CreateInstance(a, parameters) as BaseSelection)
                                               .First();

            if (selection == null)
            {
                throw new Exception("unknown selection operator");
            }
            return selection;
        }
    }
}