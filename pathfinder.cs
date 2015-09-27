using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace maze
{
    public class mazefinder
    {
        public char[,] fmaze { get; private set; } // Лабиринт
        public string fpath { get; set;} // Путь к файлу
        private int[] fexit { get; set; } // Индексы выхода из лабиринта
        private int[] fbeg { get; set; } // Индексы начала лабиринта
        public int fpoints { get; set; } // Набранные очки
        private bool fl { get; set; } // Флаг, говорящий о том, можно ли достичь выхода
        public StringBuilder answer = new StringBuilder(); // Траектория
        private List<NeededCell> felems = new List<NeededCell>(); // Коллекция очковых ячеек, которые нужн посетить
        public mazefinder(string newpath)
        {
            fpath = newpath;
            fpoints = 0;
            fl = false;
            using (StreamReader sr = new StreamReader(fpath))
            {
                int[] fdim = new int[2];
                string[] puta = File.ReadAllLines(fpath);
                fdim[0] = File.ReadAllLines(fpath).Length;
                fdim[1] = puta.OrderByDescending(x => x.Length).First().Length; // Определение размерности
                fmaze = new char[fdim[0], fdim[1]];
                for (int k=0;k< fdim[0]; k++)
                {
                    for (int l = 0; l < fdim[1]; l++)
                    {
                        try // try на случай, если лабиринт неправильной формы
                        {
                            fmaze[k, l] = puta[k][l];
                        }
                        catch
                        {
                            fmaze[k, l] = ' ';
                            goto outofrange;
                        }
                        if (puta[k][l] == '*')
                        {
                            fbeg = new int[2] { k, l };
                        }
                        if (puta[k][l] == 'e')
                        {
                            fexit = new int[2] { k, l };
                        }
                        if ((int)puta[k][l] > 48 && (int)puta[k][l] < 58)
                        {
                            felems.Add(new NeededCell(new int[2] { k, l }, (int)puta[k][l] - 48)); // Внесение нового элемента в список
                        }
                    outofrange:; // Метка для обхода ошибки
                    }
                }
            }
        }

        public char finddirection (int i,int j)
        {
            if (fbeg[0] > i) return 'u';
            if (fbeg[0] < i) return 'd';
            if (fbeg[1] > j) return 'l';
            return 'r';
        }

        private void move(int i, int j, int val) // Рекурсивная процедура расстановки цифр
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
                    fl = true;
                    if (Math.Abs((i + j) - (fbeg[0] + fbeg[1])) == 1) // Если выход - соседняя ячейка
                    {
                        answer.Append(finddirection(i, j));
                        fpoints = 20;
                        throw new Exception("Exit is found"); // Исключение для выхода из рекурсии
                    }
                    break;

                case '*':
                    break;

                default:
                    if ((int)fmaze[i, j] > val && (int)fmaze[i, j] > 128) goto case ' '; // Если найден более короткий путь до ячейки
                    break;
            }
        }

        private void cleanmaze() // Очистка от коэффицентов и отработанных очков
        {
            fl = false;
            for (int i = 0; i < fmaze.GetLength(0); i++)
            {
                for (int j = 0; j < fmaze.GetLength(1); j++)
                {
                    if (!"#e123456789".Contains<char>(fmaze[i, j]) || i==fbeg[0] & j==fbeg[1])
                    {
                        fmaze[i, j] = ' ';
                    }
                }
            }
        }


        public void pathfinder()
        {
            List<NeededCell> bannedelems = new List<NeededCell>(); // Ячейки с очками, до которых пока что нельзя добраться
            NeededCell shortest = null;
            int stackcount;
            int[] fst;
            if (fexit == null) throw new Exception("Exit is not found");
            felems.Add(new NeededCell(new int[2] { 0, 0 }, 0)); //фиктивный элемент
            while (felems.Count()!=0)
            {
                if (fl == false & felems.Count() == 1 & felems.First().flocation[0] == 0 & felems.First().flocation[1] == 0) // Если не найдено ни очков, ни выхода
                    throw new Exception("Exit is found, but I cant reach it");
                move(fbeg[0] - 1, fbeg[1], 128);
                move(fbeg[0], fbeg[1] + 1, 128);
                move(fbeg[0] + 1, fbeg[1], 128);
                move(fbeg[0], fbeg[1] - 1, 128);
                felems.AddRange(bannedelems); //Добавление недосягаемых ячеек для повторной проверки в новой иттерации
                foreach (NeededCell ndc in felems)
                {
                    ndc.fway.Clear(); // Очистка стека.
                    try
                    {
                        fst = first_step(ndc.flocation[0], ndc.flocation[1]); //Найти наименьшую соседнюю ячейку
                    }
                    catch
                    {
                        goto bodo; //обойти ошибку
                    }
                        ndc.fvalforstep = (int)fmaze[fst[0], fst[1]] + 1;
                    try
                    {
                        ndc.takemindist(this.fmaze, fst[0], fst[1], (char)fst[2], ndc.fvalforstep, fbeg); // найти наименьший путь
                    }
                    catch
                    {}
                bodo:;
                }
                bannedelems = felems.Where(x => x.fway.Count() == 0).ToList(); // Убрать недосягаемые элементы
                if (bannedelems.Exists(x => x.fvalue == 0))
                    bannedelems.Remove(bannedelems.Where(x => x.fvalue == 0).First()); // Убрать фиктивный элемент
                felems = felems.Where(x => x.fway.Count() != 0).ToList();
                if (!felems.Any())
                {
                    goto newiteration;
                }
                felems = felems.OrderBy(x => x.fway.Count()).ToList();
                shortest = felems.First();
                fbeg[0] = shortest.flocation[0];
                fbeg[1] = shortest.flocation[1];
                fpoints += shortest.fvalue;
                stackcount = shortest.fway.Count();
                for (int i = 0; i < stackcount; i++) answer.Append(shortest.fway.Pop()); //Запись траектории из стека
                if (shortest.flocation == fexit) break;
                felems.Remove(shortest);
                newiteration:
                if (!felems.Any()) felems.Add(new NeededCell(fexit, 20)); // Если нет досягаемых элементов искать выход
                cleanmaze();
            }
        }

        private int[] first_step(int i, int j)
        {
            int[] res = new int[3];
            List<char> ways = new List<char>();
            ways.Add(fmaze[i - 1, j]);
            ways.Add(fmaze[i, j + 1]);
            ways.Add(fmaze[i + 1, j]);
            ways.Add(fmaze[i, j - 1]);
            ways = ways.Where(x => !"#e*".Contains(x) && (int)x != 32).ToList();
            char puta = ways.Min<char>();
            if (fmaze[i - 1, j] == puta)
            {
                res[0] = i - 1;
                res[1] = j;
                res[2] = (int)'u';
            }
            if (fmaze[i + 1, j] == puta)
            {
                res[0] = i + 1;
                res[1] = j;
                res[2] = (int)'d';
            }
            if (fmaze[i, j + 1] == puta)
            {
                res[0] = i;
                res[1] = j + 1;
                res[2] = (int)'r';
            }
            if (fmaze[i, j - 1] == puta)
            {
                res[0] = i;
                res[1] = j - 1;
                res[2] = (int)'l';
            }
            return res;
        }
    }
}
