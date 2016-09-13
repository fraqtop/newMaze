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
            string path = Console.ReadLine();
            Maze maze;
            try
            {
                maze = new Maze(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                maze = new Maze(@"C:\Users\Economist\documents\visual studio 2015\Projects\PathFinder\PathFinder\tests\maze1.txt");
            }
            maze.Print(maze.GetMaze());
            maze.FindBestWay();
            Console.WriteLine(maze.GetWay());
            Console.ReadKey();
        }

    }
}
