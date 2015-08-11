// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System.Collections.Generic;
using CSharpMetal.Core;

namespace CSharpMetal.Operators.Selection
{
    public abstract class BaseSelection : Operator
    {
        protected BaseSelection(Dictionary<string, object> parameters) : base(parameters)
        {
        }
    }
}