using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PathFinder
{
    class Maze
    {
        private List<Cell> StringPointers;
        private Cell exitCell;
        private Cell entryCell;
        private List<Cell> points;
        public string way;
        public Maze(string mazepath)
        {
            this.way = string.Empty;
            this.points = new List<Cell>();
            this.StringPointers = new List<Cell>();
            var filedata = File.ReadLines(mazepath).ToList();
            Cell currentCell;
            foreach (string s in filedata)
            {
                currentCell = new Cell(s[0]);
                StringPointers.Add(currentCell);
                for (int i = 1; i < s.Length; i++)
                {
                    currentCell.right = new Cell(s[i]);
                    if (s[i] == 'e')
                        this.exitCell = currentCell.right;
                    if (s[i] == '*')
                        this.entryCell = currentCell.right;
                    if ("123456789".Contains(s[i]))
                        points.Add(currentCell.right);
                    currentCell.right.left = currentCell;
                    currentCell = currentCell.right;
                }
            }
            Cell nextLineCurrentCell;
            for (int i = 0; i < StringPointers.Count-1; i++)
            {
                currentCell = StringPointers[i];
                nextLineCurrentCell = StringPointers[i + 1];
                while (currentCell != null | nextLineCurrentCell != null)
                {
                    currentCell.lower = nextLineCurrentCell;
                    nextLineCurrentCell.upper = currentCell;
                    currentCell = currentCell.right;
                    nextLineCurrentCell = nextLineCurrentCell.right;
                }
            }
        }
        public List<string> GetMaze()
        {
            List<string> maze = new List<string>();
            foreach (Cell c in StringPointers)
            {
                maze.Add(c.GetLine());
            }
            return maze;
        }
        public List<string> GetDistances()
        {
            List<string> maze = new List<string>();
            foreach (Cell c in StringPointers)
            {
                maze.Add(c.GetDistances());
            }
            return maze;
        }
        private void Locate(Cell cellPointer)
        {
            cellPointer.SetDistance(0);
            this.Print(this.GetDistances());
        }
        public void FindBestWay()
        {
            List<Cell> unavailable = new List<Cell>();
            Cell startCell = this.entryCell;
            Cell finishCell = null;
            bool target_found = false;
            string partOfWay = string.Empty;
            while (finishCell != this.exitCell)
            {
                this.Locate(startCell);
                if (points.Count == 0)
                    finishCell = exitCell;
                else
                {
                    unavailable = points.Where(x => x.GetDistance() == int.MaxValue - 1).ToList();
                    foreach (Cell c in unavailable)
                    {
                        points.Remove(c);
                    }
                    points = points.OrderBy(x => x.GetDistance()).ToList();
                    finishCell = points.First();
                    points.Remove(finishCell);
                }
                finishCell.Walk(finishCell.GetDistance() + 1, ref partOfWay, ref target_found);
                startCell = finishCell;
                this.SetWay(partOfWay);
                partOfWay = string.Empty;
                target_found = false;
                this.ResetDistances();
            }
        }
        private void SetWay(string part)
        {
            way += part;
        }
        private void ResetDistances()
        {
            foreach (Cell c in this.StringPointers)
            {
                c.ResetDistances();
            }
        }
        public void Print(List<string> printable)
        {
            foreach (string s in printable)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine();
        }
    }
}
