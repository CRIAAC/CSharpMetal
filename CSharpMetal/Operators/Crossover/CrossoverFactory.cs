// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpMetal.Operators.Crossover
{
    public static class CrossoverFactory
    {
        public static Crossover GetCrossoverOperator(String operatorName, Dictionary<string, object> parameters)
        {
            if (operatorName == null)
            {
                throw new ArgumentNullException("name");
            }
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            Type interfaceType = typeof (Crossover);
            IEnumerable<Crossover> _operator = AppDomain.CurrentDomain.GetAssemblies()
                                                        .SelectMany(x => x.GetTypes())
                                                        .Where(
                                                            x =>
                                                            interfaceType.IsAssignableFrom(x) && !x.IsInterface &&
                                                            !x.IsAbstract &&
                                                            string.Equals(x.Name, operatorName,
                                                                          StringComparison.OrdinalIgnoreCase))
                                                        .Select(
                                                            a => Activator.CreateInstance(a, parameters) as Crossover);

            if (_operator == null)
            {
                throw new PlatformNotSupportedException("This crossover operator: " + operatorName + " does not exist");
            }

            return _operator.First();
        }
    }
}