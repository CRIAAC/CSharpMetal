// Author : Vandewynckel Julien
// Creation date : 09/03/2015
// Last modified date : 05/05/2015

using System;
using CSharpMetal.Core;
using CSharpMetal.Encodings.SolutionsType;
using CSharpMetal.Util.Wrapper;

namespace CSharpMetal.Problems.ZDT
{
    internal class ZDT1 : Problem
    {
        public ZDT1(String solutionType)
            : this(solutionType, 30)
        {
        }

        /**
  * Creates a new instance of problem ZDT1.
  * @param numberOfVariables Number of variables.
  * @param solutionType The solution type must "Real", "BinaryReal, and "ArrayReal". 
  */

        public ZDT1(String solutionType, int numberOfVariables)
        {
            NumberOfVariables = numberOfVariables;
            NumberOfObjectives = 2;
            NumberOfConstraints = 0;
            ProblemName = "ZDT1";

            UpperLimit = new double[NumberOfVariables];
            LowerLimit = new double[NumberOfVariables];

            // Establishes upper and lower limits for the variables
            for (int var = 0; var < NumberOfVariables; var++)
            {
                UpperLimit[var] = 0.0;
                UpperLimit[var] = 1.0;
            } // for

            if (string.Equals(solutionType, "BinaryReal", StringComparison.InvariantCultureIgnoreCase))
            {
                TypeOfSolution = new BinaryRealSolutionType(this);
            }
            else if (string.Equals(solutionType, "Real", StringComparison.InvariantCultureIgnoreCase))
            {
                TypeOfSolution = new RealSolutionType(this);
            }
            else if (string.Equals(solutionType, "ArrayReal", StringComparison.InvariantCultureIgnoreCase))
            {
                TypeOfSolution = new ArrayRealSolutionType(this);
            }
            else
            {
                throw new Exception("Error: solution type " + solutionType + " invalid");
            }
        }

        public override void Evaluate(Solution solution)
        {
            XReal x = new XReal(solution);

            double[] f = new double[NumberOfObjectives];
            f[0] = x.GetValue(0);
            double g = (EvalG(x));
            double h = EvalH(f[0], g);
            f[1] = h*g;

            solution.Objective[0] = f[0];
            solution.Objective[1] = f[1];
        }

        private double EvalG(XReal x)
        {
            double g = 0.0;
            for (int i = 1; i < x.GetNumberOfDecisionVariables(); i++)
            {
                g += x.GetValue(i);
            }
            double constante = (9.0/(NumberOfVariables - 1));
            g = constante*g;
            g = g + 1.0;
            return g;
        }

        public double EvalH(double f, double g)
        {
            double h = 0.0;
            h = 1.0 - Math.Sqrt(f/g);
            return h;
        }
    }
}