using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace maze
{
    class Program
    {
        static public void demonstr(mazefinder mz)
        {
            for (int i = 0; i < mz.fmaze.GetLength(0); i++)
            {
                for (int j = 0; j < mz.fmaze.GetLength(1); j++)
                    Console.Write(mz.fmaze[i, j]);
                Console.WriteLine();
            }
            if (mz.fpoints != 0) Console.WriteLine("You received " + mz.fpoints.ToString()+" points");
            Console.WriteLine("______________________");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("enter the path to maze");
            string p = Console.ReadLine();
            mazefinder pth = new mazefinder(p);
            demonstr(pth);
            try
            {
                pth.pathfinder();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine(pth.answer);
            Console.WriteLine("You recieve "+pth.fpoints+" points");
            Console.ReadKey();
        }
    }
}
