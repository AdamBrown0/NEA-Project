using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Assets.ConsoleStuff
{
    public class DisplayOnConsole
    {

        public void displayEntitiesOnConsoleExternal(object[,] entityGridD)
        {
            for (int row = 0; row < entityGridD.GetLength(0); row++)
            {
                for (int col = 0; col < entityGridD.GetLength(1); col++)
                {
                    Type t;
                    if (entityGridD[row, col] != null)
                    {
                        t = entityGridD[row, col].GetType();
                        if (t == typeof(Prey))
                        {
                            Prey prey = (Prey)entityGridD[row, col];
                            Console.Write($" Prey: {prey.getName()} ");
                        }
                        else if (t == typeof(Predator))
                        {
                            Predator predator = (Predator)entityGridD[row, col];
                            Console.Write($" Predator: {predator.getName()} ");
                        }
                        else
                        {
                            Console.Write("   ");
                        }
                    }

                }
            }
        }
    }
}
