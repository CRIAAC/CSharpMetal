// Author : Vandewynckel Julien
// Creation date : 06/03/2015
// Last modified date : 05/05/2015

using CSharpMetal.Core;
using CSharpMetal.Encodings.Variables;

namespace CSharpMetal.Encodings.SolutionsType
{
    public class ArrayRealSolutionType : BaseSolutionType
    {
        public ArrayRealSolutionType(Problem problem) : base(problem)
        {
        }

        public override BaseVariable[] CreateVariables()
        {
            BaseVariable[] variables = new BaseVariable[1];
            variables[0] = new ArrayReal(Problema.NumberOfVariables, Problema);
            return variables;
        }

        public BaseVariable[] CopyeVariables(BaseVariable[] variables)
        {
            var copy = new BaseVariable[1];
            copy[0] = variables[0].Clone();
            return variables;
        }
    }
}