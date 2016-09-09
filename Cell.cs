using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinder
{
    public class Cell
    {
        public Cell upper;
        public Cell lower;
        public Cell right;
        public Cell left;
        private Char value;
        private Int32 distance;
        public Cell(Char newvalue)
        {
            this.value = newvalue;
            this.distance = Int32.MaxValue - 1;
        }
        public void SetDistance(Int32 newDistance)
        {
            if (this.distance > newDistance & this.value != '#')
            {
                this.distance = newDistance;
                if (this.upper != null) this.upper.SetDistance(newDistance+1);
                if (this.right != null) this.right.SetDistance(newDistance+1);
                if (this.lower != null) this.lower.SetDistance(newDistance+1);
                if (this.left != null) this.left.SetDistance(newDistance+1);
            }
        }
        public int GetDistance()
        {
            return this.distance;
        }
        public string GetValue() { return this.value.ToString(); }
        public string GetLine( string line = "")
        {
            line += this.value;
            if (this.right == null) return line;
            return this.right.GetLine(line);
        }
        public string GetDistances(string line = "")
        {
            if (this.distance < Int32.MaxValue - 1)
            {
                if (this.distance < 10)
                    line += "0";
                line += this.distance.ToString() + " ";
            }
            else
                line += "## ";
            if (this.right == null) return line;
            return this.right.GetDistances(line);
        }
        public string GetDirection(Cell bestNeightbour)
        {
            if (bestNeightbour == this.upper) return "D";
            if (bestNeightbour == this.right) return "L";
            if (bestNeightbour == this.lower) return "U";
            return "R";
        }
        public void Walk(int prevDistance, ref string partOfWay, ref bool target_found, string direction = "")
        {
            if (this.distance == 0)
            {
                target_found = true;
                partOfWay += direction;
            }
            if (this.distance + 1 == prevDistance && !target_found)
            {
                this.upper.Walk(this.distance, ref partOfWay,ref target_found, "D");
                this.right.Walk(this.distance, ref partOfWay,ref target_found, "L");
                this.lower.Walk(this.distance, ref partOfWay, ref target_found, "U");
                this.left.Walk(this.distance, ref partOfWay, ref target_found, "R");
                partOfWay += direction;
            }
        }
        public void ResetDistances()
        {
            if (this.right == null)
                return;
            this.right.ResetDistances();
            this.distance = int.MaxValue - 1;
        }
    }
}
