using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq.Expressions;

namespace kMHCheat
{
    class Program
    {
        [DllImport("0000006607542421.dll")]
        private static extern void Aimbot();

        [DllImport("0000010651230651.dll")]
        private static extern void Bunnyhop();

        [DllImport("0000015603670007.dll")]
        private static extern void AntiFlash();

        [DllImport("0000006607542421.dll")]
        private static extern void GlowESP();

        [DllImport("0000021226151332.dll")]
        private static extern void Radar();

        [DllImport("0000016767417047.dll")]
        private static extern void AutoShoot();

        static void Main(string[] args)
        {
            Console.ReadKey();
            Console.ReadKey();
            Console.WriteLine("Hack.");

            new Thread(Aimbot).Start();
            new Thread(Bunnyhop).Start();
            new Thread(AntiFlash).Start();
            new Thread(GlowESP).Start();
            new Thread(Radar).Start();
            new Thread(AutoShoot).Start();


            Console.ReadKey();
            Console.ReadKey();
            Console.WriteLine("Done.");
            Console.ReadKey();
        }
    }
}
