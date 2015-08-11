// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpMetal.Operators.Mutation
{
    public static class MutationFactory
    {
        public static BaseMutation GetMutationOperator(String name, Dictionary<string, object> parameters)
        {
            Type t = typeof (BaseMutation);
            BaseMutation baseMutation = AppDomain.CurrentDomain.GetAssemblies()
                                                 .SelectMany(x => x.GetTypes())
                                                 .Where(
                                                     x =>
                                                     t.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract &&
                                                     x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                                                 .Select(
                                                     a => Activator.CreateInstance(a, parameters) as BaseMutation)
                                                 .First();

            if (baseMutation == null)
            {
                throw new Exception("unknown BaseMutation method");
            }
            return baseMutation;
        }
    }
}