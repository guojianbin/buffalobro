using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Hardware myComputer = Hardware.i7 | Hardware.GTX580 | Hardware.DDR3;
            if ((myComputer | Hardware.i7) == myComputer)
            {
                Console.WriteLine("°üº¬i7");
            }
            if ((myComputer | Hardware.WD1T) == myComputer)
            {
                Console.WriteLine("°üº¬WD1T");
            }
        }
    }

    public enum Hardware 
    {
        i7=1,
        Z68=2,
        DDR3=4,
        WD1T=8,
        GTX580=16
    }
}
