using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            Maze maze = new Maze(@"C:\Users\Economist\documents\visual studio 2015\Projects\PathFinder\PathFinder\tests\maze1.txt");
            maze.Print(maze.GetMaze());
            maze.FindBestWay();
            Console.WriteLine(maze.way);
            Console.ReadKey();
        }

    }
}
