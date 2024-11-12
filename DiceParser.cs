using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task3
{
    internal class DiceParser
    {
        public static List<Dice> Parse(string[] args)
        {
            if (args.Length < 3)
                throw new ArgumentException("Please specify at least three dice. Example args: 2,2,4,4,9,9 6,8,1,1,8,6 7,5,3,7,5,3");

            var diceList = new List<Dice>();
            foreach (var arg in args)
            {
                var face = arg.Split(',').ToArray();
                for (int i = 0; i < face.Length; i++)
                {
                    int temp;
                    bool parse = int.TryParse(face[i], out temp);
                    if (!parse)
                        throw new ArgumentException("Each dice must have exactly six integer faces. Example args: 2,2,4,4,9,9 6,8,1,1,8,6 7,5,3,7,5,3");
                }
                var faces = arg.Split(',').Select(int.Parse).ToArray();

                if (faces.Length != 6)
                    throw new ArgumentException("Each dice must have exactly six integer faces. Example args: 2,2,4,4,9,9 6,8,1,1,8,6 7,5,3,7,5,3");
                if (faces.Where(x => x < 0).Count() > 0)
                {
                    throw new ArgumentException("Only positive values ​​are allowed. Example args: 2,2,4,4,9,9 6,8,1,1,8,6 7,5,3,7,5,3");
                }
                diceList.Add(new Dice(faces));
            }

            return diceList;
        }
    }
}
