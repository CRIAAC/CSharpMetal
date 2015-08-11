// Author : Vandewynckel Julien
// Creation date : 09/03/2015
// Last modified date : 05/05/2015

using System.Collections.Generic;
using CSharpMetal.Core;

namespace CSharpMetal.Operators.Mutation
{
    public abstract class BaseMutation : Operator
    {
        public BaseMutation(Dictionary<string, object> parameters) : base(parameters)
        {
        }
    }
}