using System;
using System.Runtime.InteropServices;
using ZK.Core;
using ZeroKore.Client.Properties;
using ZK.Core.Funcs;
using System.Threading;
using System.Diagnostics;
using System.Drawing;

namespace ZeroKore.Client
{
    public class Client
    {
        public static void Terminate()
        {
            if (IsRunning())
                Stop();

            Environment.Exit(0);
        }

        public static Point LastMenuPos;
        public static Stopwatch timePlayed;

        public static bool IsRunning()
        {
            if (AA.AimAssistThread.IsAlive || AS.AutoShootThread.IsAlive || VA.VisualAssistanceThread.IsAlive || CJ.ConstJumpThread.IsAlive || FG.FlashGlassesThread.IsAlive || ER.EnhancedRadarThread.IsAlive)
                return true;
            else
                return false;
        }

        [DllImport("ZK.NativeCore.dll", EntryPoint = "VALite")]
        private static extern void PerfGlow();

        [DllImport("ZK.NativeCore.dll", EntryPoint = "AALite")]
        private static extern void PerfAim();

        [DllImport("ZK.NativeCore.dll", EntryPoint = "CJLite")]
        private static extern void PerfJump();

        [DllImport("ZK.NativeCore.dll", EntryPoint = "FGLite")]
        private static extern void PerfShades();

        [DllImport("ZK.NativeCore.dll", EntryPoint = "ERLite")]
        private static extern void PerfRadar();

        [DllImport("ZK.NativeCore.dll", EntryPoint = "ASLite")]
        private static extern void PerfShoot();

        public static void Start()
        {
            if (Settings.Default.AA)
                FunctionManager.AimAssist.Start();
            if (Settings.Default.AS)
                FunctionManager.AutoShoot.Start();
            if (Settings.Default.VA)
                FunctionManager.VisualAssistance.Start();
            if (Settings.Default.CJ)
                FunctionManager.ConstJump.Start();
            if (Settings.Default.FG)
                FunctionManager.FlashGlasses.Start();
            if (Settings.Default.ER)
                FunctionManager.EnhancedRadar.Start();
        }
        public static void Stop()
        {
            if (AA.AimAssistThread.IsAlive)
                FunctionManager.AimAssist.Stop();
            if (AS.AutoShootThread.IsAlive)
                FunctionManager.AutoShoot.Stop();
            if (VA.VisualAssistanceThread.IsAlive)
                FunctionManager.VisualAssistance.Stop();
            if (CJ.ConstJumpThread.IsAlive)
                FunctionManager.ConstJump.Stop();
            if (FG.FlashGlassesThread.IsAlive)
                FunctionManager.FlashGlasses.Stop();
            if (ER.EnhancedRadarThread.IsAlive)
                FunctionManager.EnhancedRadar.Stop();
        }

        public enum Function
        {
            AimAssist,
            AutoShoot,
            VisAssist,
            ConstJump,
            FlashGlasses,
            EnhancedRadar
        }

        public static void Modify(Function func, bool start)
        {
            switch (func)
            {
                case Function.AimAssist:
                    if (start)
                        FunctionManager.AimAssist.Start();
                    else
                        FunctionManager.AimAssist.Stop();
                    break;
                case Function.AutoShoot:
                    if (start)
                        FunctionManager.AutoShoot.Start();
                    else
                        FunctionManager.AutoShoot.Stop();
                    break;
                case Function.VisAssist:
                    if (start)
                        FunctionManager.VisualAssistance.Start();
                    else
                        FunctionManager.VisualAssistance.Stop();
                    break;
                case Function.ConstJump:
                    if (start)
                        FunctionManager.ConstJump.Start();
                    else
                        FunctionManager.ConstJump.Stop();
                    break;
                case Function.FlashGlasses:
                    if (start)
                        FunctionManager.FlashGlasses.Start();
                    else
                        FunctionManager.FlashGlasses.Stop();
                    break;
                case Function.EnhancedRadar:
                    if (start)
                        FunctionManager.EnhancedRadar.Start();
                    else
                        FunctionManager.EnhancedRadar.Stop();
                    break;
                default:
                    break;
            }
        }



        public static void StartPerf()
        {
            if (Settings.Default.AA)
                new Thread(PerfAim).Start();
            if (Settings.Default.AS)
                new Thread(PerfShoot).Start();
            if (Settings.Default.VA)
                new Thread(PerfGlow).Start();
            if (Settings.Default.CJ)
                new Thread(PerfJump).Start();
            if (Settings.Default.FG)
                new Thread(PerfShades).Start();
            if (Settings.Default.ER)
                new Thread(PerfRadar).Start();
        }


        public static bool IsGameRunning()
        {
            return Process.GetProcessesByName("csgo").Length > 0 ? true : false;
        }
    }
}
