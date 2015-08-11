// Author : Vandewynckel Julien
// Creation date : 16/03/2015
// Last modified date : 05/05/2015

using System;
using CSharpMetal.Core;
using CSharpMetal.Encodings.SolutionsType;

namespace CSharpMetal.Problems.DTLZ
{
    internal class DTLZ1 : Problem
    {
        public DTLZ1(String solutionType) : this(solutionType, 7, 3)
        {
        }

        public DTLZ1(String solutionType,
                     int numberOfVariables,
                     int numberOfObjectives)
        {
            NumberOfVariables = numberOfVariables;
            NumberOfObjectives = numberOfObjectives;
            NumberOfConstraints = 0;
            ProblemName = "DTLZ1";

            LowerLimit = new double[NumberOfVariables];
            UpperLimit = new double[NumberOfVariables];
            for (int var = 0; var < numberOfVariables; var++)
            {
                LowerLimit[var] = 0.0;
                UpperLimit[var] = 1.0;
            }

            if (string.Equals(solutionType, "BinaryReal", StringComparison.InvariantCultureIgnoreCase))
            {
                TypeOfSolution = new BinaryRealSolutionType(this);
            }
            else if (string.Equals(solutionType, "Real", StringComparison.InvariantCultureIgnoreCase))
            {
                TypeOfSolution = new RealSolutionType(this);
            }
            else
            {
                throw new Exception("Error: solution type " + solutionType + " invalid");
            }
        }

        public override void Evaluate(Solution solution)
        {
            BaseVariable[] gen = solution.DecisionVariables;

            double[] x = new double[NumberOfVariables];
            double[] f = new double[NumberOfObjectives];
            int k = NumberOfVariables - NumberOfObjectives + 1;

            for (int i = 0; i < NumberOfVariables; i++)
            {
                x[i] = gen[i].Value;
            }

            double g = 0.0;
            for (int i = NumberOfVariables - k; i < NumberOfVariables; i++)
            {
                g += (x[i] - 0.5)*(x[i] - 0.5) - Math.Cos(20.0*Math.PI*(x[i] - 0.5));
            }

            g = 100*(k + g);
            for (int i = 0; i < NumberOfObjectives; i++)
            {
                f[i] = (1.0 + g)*0.5;
            }

            for (int i = 0; i < NumberOfObjectives; i++)
            {
                for (int j = 0; j < NumberOfObjectives - (i + 1); j++)
                {
                    f[i] *= x[j];
                }
                if (i != 0)
                {
                    int aux = NumberOfObjectives - (i + 1);
                    f[i] *= 1 - x[aux];
                } //if
            } //for

            for (int i = 0; i < NumberOfObjectives; i++)
            {
                solution.Objective[i] = f[i];
            }
        }
    }
}