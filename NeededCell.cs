using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maze
{
    public class NeededCell
    {
        public int[] flocation { get; set; } // Индексы элемента
        public int fvalue { get; private set; } // Ценность
        public int fvalforstep { get; set; } // Его дальность от начала
        public Stack<char> fway = new Stack<char>(); // Стек для записи траектории
        public NeededCell(int[] newloc,int newvalue)
        {
            flocation = newloc;
            fvalue = newvalue;
        }
        public void takemindist(char[,] mz, int i, int j, char direct, int val, int[] target)
        {
            if (i == target[0] && j == target[1])
            {
                fway.Push(reverse(direct));
                throw new Exception(); // Выйти из рекурсии, если цель найдена
            }
            if ((int)mz[i, j] == val - 1)
            {
                fway.Push(reverse(direct)); // Добавить элемент в стек
                takemindist(mz, i - 1, j, 'u', val - 1, target);
                takemindist(mz, i, j + 1, 'r', val - 1, target);
                takemindist(mz, i + 1, j, 'd', val - 1, target);
                takemindist(mz, i, j - 1, 'l', val - 1, target);
            }
        }
        private char reverse(char dir) // Изменить направление наоборот, так как путь ищется в обратном направлении
        {
            switch (dir)
            {
                case 'u':
                    return 'd';
                case 'l':
                    return 'r';
                case 'r':
                    return 'l';
                case 'd':
                    return 'u';
            }
            return '0';
        }
    }
}
