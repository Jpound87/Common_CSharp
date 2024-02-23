using Common;
using Common.Struct;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Datam.WinForms
{
    public class VariableRelation : IIdentifiable
    {
        #region Identity
        public const String ClassName = nameof(VariableRelation);
        public String Identity
        {
            get
            {
                return ClassName;
            }
        }
        #endregion /Identity

        #region Readonly
        private readonly Double weight = .7;//must be <= 1, weights points relation relitive to max value correlation
        private readonly Double[][] relAtPair;
        public readonly Double[] relations;
        public readonly Dictionary<Double, IPair<uint>> relationToPair = new Dictionary<Double, IPair<uint>>();
        #endregion /Readonly

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public VariableRelation(ParsedCapture p, double plotAxisWeightFactor)
        {
            weight = plotAxisWeightFactor;// Settings.Default.PlotAxisWeightFactor;
            uint activeVarsMinus1 = p.ActiveVariables - 1;//calculate this once instead of multiple times
            relAtPair = new double[activeVarsMinus1][];
            relations = new double[Utility_General.Combination(p.ActiveVariables, 2)];
            int counter = 0;
            //double ratioCoeff = (p.rangeMin == 0)  ? p.rangeMax : p.rangeMax / p.rangeMin; //if min is zero then set to rangeMax
            for (uint p1 = 0; p1 < activeVarsMinus1; p1++)//last array is accounted for by earlier correlations e.g. [2,3] is same as [3,2]
            {
                uint p1plus1 = p1 + 1;//calculate this once instead of multiple times
                relAtPair[p1] = new double[p.ActiveVariables - (p1plus1)];//make a triangular matrix as shown below
                for (uint p2 = p1plus1; p2 < p.ActiveVariables; p2++)
                {
                    double aveP1 = p[p1].Average;
                    double aveP2 = p[p2].Average;
                    double[] covArray = new double[p.CaptureDepth];
                    double[] squaresArrayP1 = new double[p.CaptureDepth];//used to calculate standard devation of p1
                    double[] squaresArrayP2 = new double[p.CaptureDepth];//used to calculate standard devation of p2
                    for (uint cD = 0; cD < p.CaptureDepth; cD++)
                    {
                        double p1Val = p[p1][cD] - aveP1;
                        double p2Val = p[p2][cD] - aveP2;
                        covArray[cD] = ((p1Val) * (p2Val));
                        //take squares so we can calulate sum of squares req for standard devation
                        squaresArrayP1[cD] = Math.Pow(p1Val, 2);
                        squaresArrayP2[cD] = Math.Pow(p2Val, 2);
                    }
                    //double varianceP1 = squaresArrayP1.Average();
                    //double varianceP2 = squaresArrayP2.Average();
                    double covariance = covArray.Average();
                    double standDevP1 = Math.Sqrt(squaresArrayP1.Sum() / squaresArrayP1.Length);//calculate standard devation of p1
                    double standDevP2 = Math.Sqrt(squaresArrayP2.Sum() / squaresArrayP2.Length);//calculate standard devation of p1
                    double correlation = Math.Abs(covariance / (standDevP1 * standDevP2));//calc and store correlation, we only want magnitude of correlation
                    double normalizedDiffInRange = (aveP1 - Math.Min(p[p1].Minimum, p[p2].Minimum)) / (Math.Max(p[p1].Maximum, p[p2].Maximum) - Math.Min(p[p1].Minimum, p[p2].Minimum));
                    double relation = weight * normalizedDiffInRange + (1 - weight) * correlation;
                    if (Double.IsNaN(relation))//label correlation as bad (0) 
                    {
                        relation = 0; 
                    }
                    relation = CheckAdjust(relations, relation); // check and see if its unique, this method adjusts slightly if not
                    relAtPair[p1][p2 - (p1plus1)] = relation;//load matrix
                    relationToPair[relation] = new Pair<uint>(p1, p2);//load dictionary 
                    relations[counter++] = relation; //store the correlation in the array at counter then increment 

                    //[0] [          ] the code above is written so the matrix gets stored like this
                    //[1]  x [       ]
                    //[2]  x  x [    ]
                    //[3]  x  x  x  x
                }
            }
        }
        #endregion /Constructor

        #region Correlation
        /// <summary>
        /// This method is a helper that is intended to be used with 
        /// the class constructor, it minutely adjusts correlation until it is unique
        /// </summary>
        /// <param name="relations">list of all correlations between variables</param>
        /// <param name="variableRelation">correlation to be added</param>
        /// <returns></returns>
        private double CheckAdjust(double[] relations, double variableRelation)
        {
            foreach (double c in relations)
            {
                if (c == variableRelation)
                {
                    RandAdj(ref variableRelation);//we need small differences to make unique in dict
                    return CheckAdjust(relations, variableRelation);
                }
            }
            return variableRelation;
        }
        /// <summary>
        /// This method is a helper that is intended to be used with the 
        /// method checkAdj to make the random adjustment
        /// </summary>
        /// <param name="correlation"></param>
        private void RandAdj(ref double correlation)
        {
            int seed = Convert.ToInt32(DateTime.Now.Millisecond);
            Random random = new Random(seed);
            double randAdj = random.NextDouble() * .001;
            correlation += randAdj;
        }
        #endregion /Correlation

        #region At
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public Double At(uint p1, uint p2)
        {
            if (p1 == p2)
            {
                return 1;//self correlation is one
            }
            else
            {
                if (p1 < p2)
                {
                    if (p1 < relAtPair.Length)
                    {
                        p2 -= (1 + p1);//adjust for array structure
                        if (p2 <= relAtPair[p1].Length)
                        {
                            double value = relAtPair[p1][p2];
                            return value;
                        }
                    }
                }
                else
                {
                    if (p2 < relAtPair.Length)
                    {
                        p1 -= (1 + p2);//adjust for array structure
                        if (p1 <= relAtPair[p2].Length)
                        {
                            double value = relAtPair[p2][p1];
                            return value;
                        }
                    }
                }
            }
            //log
            return 0;//error
        }
        #endregion /At
    }
}
