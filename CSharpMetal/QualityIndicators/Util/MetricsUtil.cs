// Author : Vandewynckel Julien
// Creation date : 08/03/2015
// Last modified date : 05/05/2015

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using CSharpMetal.Core;
using CSharpMetal.Util;

namespace CSharpMetal.QualityIndicators.Util
{
    public static class MetricsUtil
    {
        public static double[][] ReadFront(string filename)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var lines = File.ReadAllLines(filename).
                             Select(x => x.Split(new[] {';', '\t', ' '}, StringSplitOptions.RemoveEmptyEntries));
            return lines.Select(s => Array.ConvertAll(s, Double.Parse)).ToArray();
        }

        public static double[] GetMaximumValues(double[][] front, int noObjectives)
        {
            double[] maximumValue = new double[noObjectives];
            for (int i = 0; i < noObjectives; i++)
            {
                maximumValue[i] = double.NegativeInfinity;
            }

            foreach (double[] aFront in front)
            {
                for (int j = 0; j < aFront.Length; j++)
                {
                    if (aFront[j] > maximumValue[j])
                    {
                        maximumValue[j] = aFront[j];
                    }
                }
            }

            return maximumValue;
        }

        public static double[] GetMinimumValues(double[][] front, int noObjectives)
        {
            double[] maximumValue = new double[noObjectives];
            for (int i = 0; i < noObjectives; i++)
            {
                maximumValue[i] = double.MaxValue;
            }

            foreach (double[] aFront in front)
            {
                for (int j = 0; j < aFront.Length; j++)
                {
                    if (aFront[j] < maximumValue[j])
                    {
                        maximumValue[j] = aFront[j];
                    }
                }
            }

            return maximumValue;
        }

        public static double EuclideanDistance(double[] a, double[] b)
        {
            return Math.Sqrt(a.Select((t, i) => Math.Pow(t - b[i], 2.0)).Sum());
        }

        public static double DistanceToClosedPoint(double[] point, double[][] front)
        {
            double minDistance = EuclideanDistance(point, front[0]);

            for (int i = 1; i < front.Length; i++)
            {
                double aux = EuclideanDistance(point, front[i]);
                if (aux < minDistance)
                {
                    minDistance = aux;
                }
            }

            return minDistance;
        }

        public static double DistanceToNearestPoint(double[] point, double[][] front)
        {
            double minDistance = double.MaxValue;

            foreach (double[] aFront in front)
            {
                double aux = EuclideanDistance(point, aFront);
                if ((aux < minDistance) && (aux > 0.0))
                {
                    minDistance = aux;
                }
            }

            return minDistance;
        }

        public static double[][] GetNormalizedFront(double[][] front,
                                                    double[] maximumValue,
                                                    double[] minimumValue)
        {
            double[][] normalizedFront = new double[front.Length][];

            for (int i = 0; i < front.Length; i++)
            {
                normalizedFront[i] = new double[front[i].Length];
                for (int j = 0; j < front[i].Length; j++)
                {
                    normalizedFront[i][j] = (front[i][j] - minimumValue[j])/
                                            (maximumValue[j] - minimumValue[j]);
                }
            }
            return normalizedFront;
        }

        public static double[][] InvertedFront(double[][] front)
        {
            double[][] invertedFront = new double[front.Length][];

            for (int i = 0; i < front.Length; i++)
            {
                invertedFront[i] = new double[front[i].Length];
                for (int j = 0; j < front[i].Length; j++)
                {
                    if (front[i][j] <= 1.0 && front[i][j] >= 0.0)
                    {
                        invertedFront[i][j] = 1.0 - front[i][j];
                    }
                    else if (front[i][j] > 1.0)
                    {
                        invertedFront[i][j] = 0.0;
                    }
                    else if (front[i][j] < 0.0)
                    {
                        invertedFront[i][j] = 1.0;
                    }
                }
            }
            return invertedFront;
        }

        public static SolutionSet ReadSolutionSet(string filename)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var lines = File.ReadAllLines(filename).
                             Select(x => x.Split(new[] {';', '\t', ' '}, StringSplitOptions.RemoveEmptyEntries));
            SolutionSet solutionSet = new SolutionSet();
            solutionSet.Capacity = solutionSet.Capacity + 1;
            solutionSet.SolutionList.AddRange(from doubles in lines.Select(s => Array.ConvertAll(s, Double.Parse))
                                              select new Solution(doubles.Length)
                                              {
                                                  Objective = doubles
                                              });
            return solutionSet;
        }

        public static SolutionSet ReadNonDominatedSolutionSet(String filename)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var lines = File.ReadAllLines(filename).
                             Select(x => x.Split(new[] {';', '\t', ' '}, StringSplitOptions.RemoveEmptyEntries));
            SolutionSet solutionSet = new NonDominatedSolutionList();
            solutionSet.SolutionList.AddRange(from doubles in lines.Select(s => Array.ConvertAll(s, Double.Parse))
                                              select new Solution(doubles.Length)
                                              {
                                                  Objective = doubles
                                              });
            return solutionSet;
        }

        public static void ReadNonDominatedSolutionSet(String filename, NonDominatedSolutionList solutionSet)
        {
            var lines = File.ReadAllLines(filename).
                             Select(x => x.Split(new[] {';', '\t', ' '}, StringSplitOptions.RemoveEmptyEntries));
            solutionSet.SolutionList.AddRange(from doubles in lines.Select(s => Array.ConvertAll(s, Double.Parse))
                                              select new Solution(doubles.Length)
                                              {
                                                  Objective = doubles
                                              });
        }

        public static double[] HvContributions(int _numberOfobjectives, double[][] front)
        {
            Hypervolume hypervolume = new Hypervolume();
            int numberOfObjectives = _numberOfobjectives;
            double[] contributions = new double[front.Length];
            double[][] frontSubset = new double[front.Length - 1][];
            for (int index = 0; index < front.Length - 1; index++)
            {
                frontSubset[index] = new double[front[0].Length];
            }


            List<double[]> frontCopy = new List<double[]>();
            frontCopy.Add(front.SelectMany(x => x).ToArray());

            double[][] totalFront = frontSubset.Select(a => a.ToArray()).ToArray();


            double totalVolume = hypervolume.CalculateHypervolume(totalFront, totalFront.Length, numberOfObjectives);
            for (int i = 0; i < front.Length; i++)
            {
                double[] evaluatedPoint = frontCopy[i];
                frontCopy.RemoveAt(i);
                frontSubset = frontCopy.ToArray();
                // STEP4. The hypervolume (control is passed to java version of Zitzler code)
                double hv = hypervolume.CalculateHypervolume(frontSubset, frontSubset.Length, numberOfObjectives);
                double contribution = totalVolume - hv;
                contributions[i] = contribution;
                // put point back
                frontCopy.Insert(i, evaluatedPoint);
            }
            return contributions;
        }

        public static double[] HvContributions(SolutionSet[] populations)
        {
            bool empty = true;
            foreach (SolutionSet population2 in populations)
            {
                if (population2.Size() > 0)
                {
                    empty = false;
                }
            }

            if (empty)
            {
                double[] contributions = new double[populations.Length];
                for (int i = 0; i < populations.Length; i++)
                {
                    contributions[i] = 0;
                }
                for (int i = 0; i < populations.Length; i++)
                {
                    Console.WriteLine(contributions[i]);
                }
                return contributions;
            }


            const double offset = 0.0;

            //determining the global Size of the population
            int size = populations.Sum(population1 => population1.Size());

            //allocating space for the union 
            var union = new SolutionSet(size);

            // filling union
            foreach (SolutionSet population in populations)
            {
                for (int j = 0; j < population.Size(); j++)
                {
                    union.Add(population[j]);
                }
            }


            //determining the number of objectives		  
            int numberOfObjectives = union[0].NumberOfObjectives;

            //writing everything in matrices
            double[][][] frontValues = new double[populations.Length + 1][][];

            frontValues[0] = union.WriteObjectivesToMatrix();
            for (int i = 0; i < populations.Length; i++)
            {
                if (populations[i].Size() > 0)
                {
                    frontValues[i + 1] = populations[i].WriteObjectivesToMatrix();
                }
                else
                {
                    frontValues[i + 1] = new double[0][];
                }
            }


            // obtain the maximum and minimum values of the Pareto front
            double[] maximumValues = GetMaximumValues(union.WriteObjectivesToMatrix(), numberOfObjectives);
            double[] minimumValues = GetMinimumValues(union.WriteObjectivesToMatrix(), numberOfObjectives);


            // normalized all the fronts
            double[][][] normalizedFront = new double[populations.Length + 1][][];
            for (int i = 0; i < normalizedFront.Length; i++)
            {
                if (frontValues[i].Length > 0)
                {
                    normalizedFront[i] = GetNormalizedFront(frontValues[i], maximumValues, minimumValues);
                }
                else
                {
                    normalizedFront[i] = new double[0][];
                }
            }

            // compute offsets for reference point in normalized space
            double[] offsets = new double[maximumValues.Length];
            for (int i = 0; i < maximumValues.Length; i++)
            {
                offsets[i] = offset/(maximumValues[i] - minimumValues[i]);
            }

            //Inverse all the fronts front. This is needed because the original
            //metric by Zitzler is for maximization problems

            double[][][] invertedFront = new double[populations.Length + 1][][];
            for (int i = 0; i < invertedFront.Length; i++)
            {
                if (normalizedFront[i].Length > 0)
                {
                    invertedFront[i] = InvertedFront(normalizedFront[i]);
                }
                else
                {
                    invertedFront[i] = new double[0][];
                }
            }

            // shift away from origin, so that boundary points also get a contribution > 0
            foreach (double[][] anInvertedFront in invertedFront)
            {
                foreach (double[] point in anInvertedFront)
                {
                    for (int i = 0; i < point.Length; i++)
                    {
                        point[i] += offsets[i];
                    }
                }
            }

            // calculate contributions 
            double[] contribution = new double[populations.Length];
            Hypervolume hypervolume = new Hypervolume();

            for (int i = 0; i < populations.Length; i++)
            {
                if (invertedFront[i + 1].Length == 0)
                {
                    contribution[i] = 0;
                }
                else
                {
                    if (invertedFront[i + 1].Length != invertedFront[0].Length)
                    {
                        double[][] aux = new double[invertedFront[0].Length - invertedFront[i + 1].Length][];
                        int startPoint = 0;
                        for (int j = 0; j < i; j++)
                        {
                            startPoint += invertedFront[j + 1].Length;
                        }
                        int endPoint = startPoint + invertedFront[i + 1].Length;
                        int index = 0;
                        for (int j = 0; j < invertedFront[0].Length; j++)
                        {
                            if (j < startPoint || j >= (endPoint))
                            {
                                aux[index++] = invertedFront[0][j];
                            }
                        }
                        //System.out.println(hypervolume.calculateHypervolume(invertedFront[0], invertedFront[0].Length, getNumberOfObjectives));
                        //System.out.println(hypervolume.calculateHypervolume(aux, aux.Length, getNumberOfObjectives));

                        contribution[i] =
                            hypervolume.CalculateHypervolume(invertedFront[0], invertedFront[0].Length,
                                                             numberOfObjectives) -
                            hypervolume.CalculateHypervolume(aux, aux.Length, numberOfObjectives);
                    }
                    else
                    {
                        contribution[i] = hypervolume.CalculateHypervolume(invertedFront[0], invertedFront[0].Length,
                                                                           numberOfObjectives);
                    }
                }
            }

            //for (int i = 0; i < contribution.Length; i++) 
            //System.out.println(invertedFront[0].Length +" "+ invertedFront[i+1].Length +" "+ contribution[i]);

            return contribution;
        }

        public static double[] HvContributions(SolutionSet archive, SolutionSet[] populations)
        {
            const double offset_ = 0.0;

            //determining the global Size of the population
            int size = populations.Sum(population => population.Size());

            //allocating space for the union 
            SolutionSet union = archive;

            //determining the number of objectives		  
            int numberOfObjectives = union[0].NumberOfObjectives;

            //writing everything in matrices
            double[][][] frontValues = new double[populations.Length + 1][][];

            frontValues[0] = union.WriteObjectivesToMatrix();
            for (int i = 0; i < populations.Length; i++)
            {
                if (populations[i].Size() > 0)
                {
                    frontValues[i + 1] = populations[i].WriteObjectivesToMatrix();
                }
                else
                {
                    frontValues[i + 1] = new double[0][];
                }
            }


            // obtain the maximum and minimum values of the Pareto front
            double[] maximumValues = GetMaximumValues(union.WriteObjectivesToMatrix(), numberOfObjectives);
            double[] minimumValues = GetMinimumValues(union.WriteObjectivesToMatrix(), numberOfObjectives);


            // normalized all the fronts
            double[][][] normalizedFront = new double[populations.Length + 1][][];
            for (int i = 0; i < normalizedFront.Length; i++)
            {
                if (frontValues[i].Length > 0)
                {
                    normalizedFront[i] = GetNormalizedFront(frontValues[i], maximumValues, minimumValues);
                }
                else
                {
                    normalizedFront[i] = new double[0][];
                }
            }

            // compute offsets for reference point in normalized space
            double[] offsets = new double[maximumValues.Length];
            for (int i = 0; i < maximumValues.Length; i++)
            {
                offsets[i] = offset_/(maximumValues[i] - minimumValues[i]);
            }

            //Inverse all the fronts front. This is needed because the original
            //metric by Zitzler is for maximization problems

            double[][][] invertedFront = new double[populations.Length + 1][][];
            for (int i = 0; i < invertedFront.Length; i++)
            {
                if (normalizedFront[i].Length > 0)
                {
                    invertedFront[i] = InvertedFront(normalizedFront[i]);
                }
                else
                {
                    invertedFront[i] = new double[0][];
                }
            }

            // shift away from origin, so that boundary points also get a contribution > 0
            foreach (double[][] anInvertedFront in invertedFront)
            {
                foreach (double[] point in anInvertedFront)
                {
                    for (int i = 0; i < point.Length; i++)
                    {
                        point[i] += offsets[i];
                    }
                }
            }

            // calculate contributions 
            double[] contribution = new double[populations.Length];
            Hypervolume hypervolume = new Hypervolume();

            for (int i = 0; i < populations.Length; i++)
            {
                if (invertedFront[i + 1].Length == 0)
                {
                    contribution[i] = 0;
                }
                else
                {
                    int auxSize = 0;
                    for (int j = 0; j < populations.Length; j++)
                    {
                        if (j != i)
                        {
                            auxSize += invertedFront[j + 1].Length;
                        }
                    }

                    if (size == archive.Size())
                    {
                        // the contribution is the maximum hv
                        contribution[i] = hypervolume.CalculateHypervolume(invertedFront[0], invertedFront[0].Length,
                                                                           numberOfObjectives);
                    }
                    else
                    {
                        //make a front with all the populations but the target one
                        int index = 0;
                        double[][] aux = new double[auxSize][];
                        for (int j = 0; j < populations.Length; j++)
                        {
                            if (j != i)
                            {
                                for (int k = 0; k < populations[j].Size(); k++)
                                {
                                    aux[index++] = invertedFront[j + 1][k];
                                }
                            }
                        }
                        contribution[i] =
                            hypervolume.CalculateHypervolume(invertedFront[0], invertedFront[0].Length,
                                                             numberOfObjectives) -
                            hypervolume.CalculateHypervolume(aux, aux.Length, numberOfObjectives);
                    }


                    /*
	    			  int Size2 = 0;
	    			  for (int j = 0; j < populations.Length; j++) 
	    				  Size2+=invertedFront[j+1].Length;
	    			  
	    			  
	    			  double [][] aux = new double[Size2 - invertedFront[i+1].Length][];
	    			  int index = 0;
    				  for (int j = 0; j < populations.Length; j++) {
    					  if (j!=i) {						  
    						  for (int k = 0; k < invertedFront[j+1].Length; k++)
    							  aux[index++] = invertedFront[j+1][k];
    					  }
    				  }
	    			  	    			  		  
    				  System.out.println(hypervolume.calculateHypervolume(invertedFront[0], invertedFront[0].Length, getNumberOfObjectives));
    				  System.out.println(index+" "+aux.Length);
    				  System.out.println(hypervolume.calculateHypervolume(aux, aux.Length, getNumberOfObjectives));
	    		  
    				  
    				  
    				  contribution[i] = hypervolume.calculateHypervolume(invertedFront[0], invertedFront[0].Length, getNumberOfObjectives) -
	    				  			hypervolume.calculateHypervolume(aux, aux.Length, getNumberOfObjectives);
	    			*/
                }
            }

            //for (int i = 0; i < contribution.Length; i++) 
            //System.out.println(invertedFront[0].Length +" "+ invertedFront[i+1].Length +" "+ contribution[i]);

            return contribution;
        }
    }
}