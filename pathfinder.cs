using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace maze
{
    public class mazefinder
    {
        public char[,] fmaze { get; private set; }
        public string fpath { get; set;}
        public int[] fdim = new int[2];
        public int[] fexit = new int[2];
        public int[] fbeg = new int[2];
        public int points { get; private set; }
        public mazefinder(string newpath)
        {
            fpath = newpath;
            points = 0;
            using (StreamReader sr = new StreamReader(fpath))
            {
                fdim[0] = File.ReadAllLines(fpath).Length;
                string buf= sr.ReadLine();
                fdim[1] = buf.Length;
                fmaze = new char[fdim[0], fdim[1]];
                for (int k=0;k< fdim[0]; k++)
                {
                    for (int l = 0; l < fdim[1]; l++)
                        fmaze[k, l] = buf[l];
                    buf = sr.ReadLine();
                }
            }
        }

        private void move(int i, int j, int val)
        {
            switch (fmaze[i,j])
            {
                case ' ':
                    fmaze[i, j] = Convert.ToChar(val);
                    move(i - 1, j, val + 1);
                    move(i, j + 1, val + 1);
                    move(i + 1, j, val + 1);
                    move(i, j - 1, val + 1);
                    break;

                case 'e':
                    break;

                case '*':
                    fbeg[0] = i;
                    fbeg[1] = j;
                    break;

                default:
                    if ((int)fmaze[i, j] > val && (int)fmaze[i, j] > 57)
                    {
                        fmaze[i, j] = Convert.ToChar(val);
                        move(i - 1, j, val + 1);
                        move(i, j + 1, val + 1);
                        move(i + 1, j, val + 1);
                        move(i, j - 1, val + 1);
                    }
                    if ((int)fmaze[i, j] > 48 && (int)fmaze[i, j] < 58)
                    {
                        points += (int)fmaze[i, j] - 48;
                        fmaze[i, j] = ' ';
                        move(i, j, val);
                    }
                    break;
            }
        }

        public void pathfinder()
        {
            fexit = FindExit();
            if (fexit == null) throw new Exception("Exit is not found");
            move(fexit[0] - 1, fexit[1], 128);
            move(fexit[0], fexit[1] + 1, 128);
            move(fexit[0] + 1, fexit[1], 128);
            move(fexit[0], fexit[1] - 1, 128);
            short_way(first_step()[0], first_step()[1],(int)fmaze[first_step()[0],first_step()[1]]+1);
        }

        private void clean_maze ()
        {
            for (int i = 0; i < fdim[0]; i++)
                for (int j = 0; j < fdim[1]; j++)
                    if (!"#xe*".Contains<char>(fmaze[i, j])) fmaze[i, j] = ' ';
        }

        public void short_way(int i, int j, int val)
        {
            if (fmaze[i,j]=='e')
            {
                clean_maze();
                throw new Exception("the exit is found, take a look");
            }
            if ((int)fmaze[i, j] - val == -1)
            {
                fmaze[i, j] = 'x';
                short_way(i - 1, j, val - 1);
                short_way(i, j + 1, val - 1);
                short_way(i, j - 1, val - 1);
                short_way(i + 1, j, val - 1);
            }

        }

        private int[] first_step()
        {
            int[] res = new int[2];
            List<char> ways = new List<char>();
            ways.Add(fmaze[fbeg[0] - 1, fbeg[1]]);
            ways.Add(fmaze[fbeg[0], fbeg[1] + 1]);
            ways.Add(fmaze[fbeg[0] + 1, fbeg[1]]);
            ways.Add(fmaze[fbeg[0], fbeg[1] - 1]);
            char puta = ways.Min<char>();
            while (puta == '#')
            {
                ways.Remove(puta);
                puta = ways.Min<char>();
            }
            if (fmaze[fbeg[0] - 1, fbeg[1]]==puta)
            {
                res[0] = fbeg[0] - 1;
                res[1] = fbeg[1];
            }
            if (fmaze[fbeg[0] + 1, fbeg[1]] == puta)
            {
                res[0] = fbeg[0] + 1;
                res[1] = fbeg[1];
            }
            if (fmaze[fbeg[0], fbeg[1] + 1] == puta)
            {
                res[0] = fbeg[0];
                res[1] = fbeg[1] + 1;
            }
            if (fmaze[fbeg[0], fbeg[1] - 1] == puta)
            {
                res[0] = fbeg[0];
                res[1] = fbeg[1] - 1;
            }
            return res;
        }

        private int[] FindExit()
        {
            for (int i = 0; i < fdim[0]; i++)
                for (int j = 0; j < fdim[1]; j++)
                    if (fmaze[i, j] == 'e' | fmaze[i, j] == 'E')
                        return new int[2] { i, j };
            return null;
        }
    }
}
