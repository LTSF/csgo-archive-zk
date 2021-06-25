using System;

namespace ZK.Core.Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();
            Console.WriteLine("Started");
            FunctionManager.AimAssist.Start();
            FunctionManager.AutoShoot.Start();
            FunctionManager.ConstJump.Start();
            FunctionManager.EnhancedRadar.Start();
            FunctionManager.FlashGlasses.Start();
            FunctionManager.VisualAssistance.Start();
            Console.ReadLine();
            Console.ReadLine();
            FunctionManager.AimAssist.Stop();
            FunctionManager.AutoShoot.Stop();
            FunctionManager.ConstJump.Stop();
            FunctionManager.EnhancedRadar.Stop();
            FunctionManager.FlashGlasses.Stop();
            FunctionManager.VisualAssistance.Stop();
            Console.WriteLine("Stopped");
            Console.ReadLine();
            Console.ReadLine();
        }
    }
}
