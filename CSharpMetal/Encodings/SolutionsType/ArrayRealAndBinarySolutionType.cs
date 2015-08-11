// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using CSharpMetal.Core;
using CSharpMetal.Encodings.Variables;

namespace CSharpMetal.Encodings.SolutionsType
{
    internal class ArrayRealAndBinarySolutionType : BaseSolutionType
    {
        private readonly int _binaryStringLength;
        private readonly int _numberOfRealVariables;

        public ArrayRealAndBinarySolutionType(Problem problem, int realVariables, int binaryStringLength)
            : base(problem)
        {
            _binaryStringLength = binaryStringLength;
            _numberOfRealVariables = realVariables;
        }

        public override BaseVariable[] CreateVariables()
        {
            BaseVariable[] variables = new BaseVariable[2];

            variables[0] = new ArrayReal(_numberOfRealVariables, Problema);
            variables[1] = new Binary(_binaryStringLength);
            return variables;
        }
    }
}