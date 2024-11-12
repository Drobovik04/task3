using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task3
{
    internal class TableGenerator
    {
        List<Dice> dices;
        double[,] probabilities;
        static int tableWidth = 70;
        public TableGenerator(List<Dice> dices, double[,] probabilities) 
        {
            this.probabilities = probabilities;
            this.dices = dices;
        }
        
        public void PrintTable()
        {
            Console.WriteLine("Probability of the win for the user:");
            var corner = new List<string>() { "User dice v" };
            corner.AddRange(dices.Select(x => x.ToString()));
            string[] cols = corner.ToArray();
            PrintLine();
            PrintRow(cols);
            PrintLine();
            for (int i = 0; i < dices.Count; i++)
            {
                List<string> probs = new List<string>() { dices[i].ToString() };
                for (int j = 0; j < dices.Count; j++)
                {
                    probs.Add(probabilities[i, j]==0.5?"- (0.5)":Math.Round(probabilities[i, j], 4).ToString());
                }
                PrintRow(probs.ToArray());
                PrintLine();
            }
        }

        static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        static void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}
