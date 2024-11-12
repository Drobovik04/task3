using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task3
{
    internal class Dice
    {
        private readonly int[] faces;
        public int[] Faces
        {
            get { return faces; }
        }
        public Dice(int[] faces)
        {
            if (faces.Length != 6)
                throw new ArgumentException("Each dice must have exactly six faces.");

            this.faces = faces;
        }

        public int this[int index] 
        { 
            get
            {
                return faces[index];
            }
        }

        public override string ToString()
        {
            return string.Join(",", faces);
        }
    }
}
