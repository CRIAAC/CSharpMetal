// Author : Vandewynckel Julien
// Creation date : 05/03/2015
// Last modified date : 05/05/2015

using System;
using CSharpMetal.Core;
using CSharpMetal.Encodings.SolutionsType;
using CSharpMetal.Encodings.Variables;

namespace CSharpMetal.Util.Wrapper
{
    public class XReal
    {
        public Solution RSolution { get; set; }
        public BaseSolutionType RType { get; set; }

        public XReal()
        {
        }

        public XReal(Solution solution)
        {
            RType = solution.SolutionType;
            RSolution = solution;
        }

        public double GetValue(int index)
        {
            Type locaType = RType.GetType();
            if (locaType == typeof (RealSolutionType) || locaType == typeof (BinaryRealSolutionType))
            {
                return RSolution.DecisionVariables[index].Value;
            }
            if (locaType == typeof (ArrayRealSolutionType))
            {
                return ((ArrayReal) (RSolution.DecisionVariables[0])).DoubleValues[index];
            }
            if (locaType == typeof (ArrayRealAndBinarySolutionType))
            {
                return ((ArrayReal) (RSolution.DecisionVariables[0])).DoubleValues[index];
            }
            throw new Exception("solution type " + locaType + " invalid");
        }

        public void SetValue(int index, double value)
        {
            Type locaType = RType.GetType();
            if (locaType == typeof (RealSolutionType) || locaType == typeof (BinaryRealSolutionType))
            {
                RSolution.DecisionVariables[index].Value = value;
            }
            else if (locaType == typeof (ArrayRealSolutionType))
            {
                ((ArrayReal) (RSolution.DecisionVariables[0])).DoubleValues[index] = value;
            }
            else if (locaType == typeof (ArrayRealAndBinarySolutionType))
            {
                ((ArrayReal) (RSolution.DecisionVariables[0])).DoubleValues[index] = value;
            }
            else
            {
                throw new Exception("solution type " + locaType + " invalid");
            }
        }

        public double GetLowerBound(int index)
        {
            Type locaType = RType.GetType();
            if (locaType == typeof (RealSolutionType) /*|| locaType == typeof(BinaryRealSolutionType)*/)
            {
                return RSolution.DecisionVariables[index].LowerBound;
            }
            if (locaType == typeof (ArrayRealSolutionType))
            {
                return ((ArrayReal) (RSolution.DecisionVariables[0])).GetLowerBound(index);
            }
            if (locaType == typeof (ArrayRealAndBinarySolutionType))
            {
                return ((ArrayReal) (RSolution.DecisionVariables[0])).GetLowerBound(index);
            }
            throw new Exception("solution type " + locaType + " invalid");
        }

        public double GetUpperBound(int index)
        {
            Type locaType = RType.GetType();
            if (locaType == typeof (RealSolutionType) /*|| locaType == typeof(BinaryRealSolutionType)*/)
            {
                return RSolution.DecisionVariables[index].UpperBound;
            }
            if (locaType == typeof (ArrayRealSolutionType))
            {
                return ((ArrayReal) (RSolution.DecisionVariables[0])).GetUpperBound(index);
            }
            if (locaType == typeof (ArrayRealAndBinarySolutionType))
            {
                return ((ArrayReal) (RSolution.DecisionVariables[0])).GetUpperBound(index);
            }
            throw new Exception("solution type " + locaType + " invalid");
        }

        public int GetNumberOfDecisionVariables()
        {
            Type locaType = RType.GetType();
            if (locaType == typeof (RealSolutionType) /*|| locaType == typeof(BinaryRealSolutionType)*/)
            {
                return RSolution.DecisionVariables.Length;
            }
            if (locaType == typeof (ArrayRealSolutionType))
            {
                return ((ArrayReal) (RSolution.DecisionVariables[0])).Size;
            }
            throw new Exception("solution type " + locaType + " invalid");
        }

        public int GetSize()
        {
            return GetNumberOfDecisionVariables();
        }
    }
}