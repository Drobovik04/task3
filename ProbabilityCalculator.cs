using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task3
{
    internal class ProbabilityCalculator
    {
        public static double[,] CalculateProbabilities(List<Dice> dices)
        {
            double[,] probabilities = new double[dices.Count, dices.Count];

            for (int i = 0; i < dices.Count; i++)
            {
                for (int j = 0; j < dices.Count; j++)
                {
                    double winProbability = i==j?0.5:CalculateProbability(dices[i].Faces, dices[j].Faces);
                    probabilities[i, j] = winProbability;
                }
            }

            return probabilities;
        }

        private static double CalculateProbability(int[] dice1, int[] dice2)
        {
            int wins = 0;
            int totalRounds = dice1.Length * dice2.Length;
            for (int i = 0; i < dice1.Length; i++)
            {
                for (int j = 0; j < dice2.Length; j++)
                {
                    if (dice1[i] > dice2[j])
                    {
                        wins++;
                    }
                }
            }

            return (double)wins / totalRounds;
        }
    }
}
