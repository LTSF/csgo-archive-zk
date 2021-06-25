using System;
using System.ServiceProcess;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net.Http.Headers;
using System.Security.Permissions;

namespace Client_0000001243341361.Libs
{
    public class Client
    {
        public static Point LastPositionOfMSet;

        public static bool IsRunning = false;
        public static bool FirstRun = true;
        public enum DriverStatus
        {
            Running,
            Stopped,
            Missing,
            Waiting,
            Unknown
        }

        private static Thread AimAssist;
        private static Thread AutoFire;
        private static Thread VisAssist;
        private static Thread EnhancedRad;
        private static Thread ConstJump;
        private static Thread FlashGlasses;

        public static void Terminate()
        {
            Properties.Settings.Default.Save();

            try
            {
                if (AimAssist.IsAlive)
                    AimAssist.Abort();
            }
            catch { }
            try
            {
                if (AutoFire.IsAlive)
                    AutoFire.Abort();
            }
            catch { }
            try
            {
                if (VisAssist.IsAlive)
                    VisAssist.Abort();
            }
            catch { }
            try
            {
                if (EnhancedRad.IsAlive)
                    EnhancedRad.Abort();
            }
            catch { }
            try
            {

                if (ConstJump.IsAlive)
                    ConstJump.Abort();
            }
            catch { }
            try
            {
                if (FlashGlasses.IsAlive)
                    FlashGlasses.Abort();
            }
            catch { }

            Environment.Exit(0);
        }

        public static DriverStatus GetDriverStatus()
        {
            try
            {
                ServiceController dSC = new ServiceController("ZeroKore");

                switch (dSC.Status)
                {
                    case ServiceControllerStatus.Running:
                        return DriverStatus.Running;
                    case ServiceControllerStatus.StopPending:
                        return DriverStatus.Waiting;
                    case ServiceControllerStatus.Stopped:
                        return DriverStatus.Stopped;
                    case ServiceControllerStatus.StartPending:
                        return DriverStatus.Waiting;
                    default:
                        return DriverStatus.Unknown;
                }
            }
            catch
            {
                return DriverStatus.Missing;
            }
        }

        private static Stopwatch sw = new Stopwatch();

        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        public static void StartZeroKore()
        {
            if(!sw.IsRunning)
                sw.Start();

            AimAssist = new Thread(Aimbot);
            AimAssist.IsBackground = true;

            AutoFire = new Thread(AutoShoot);
            AutoFire.IsBackground = true;

            VisAssist = new Thread(GlowESP);
            VisAssist.IsBackground = true;

            EnhancedRad = new Thread(Radar);
            EnhancedRad.IsBackground = true;

            ConstJump = new Thread(Bunnyhop);
            ConstJump.IsBackground = true;

            FlashGlasses = new Thread(AntiFlash);
            FlashGlasses.IsBackground = true;

            if(Properties.Settings.Default.AimAssist)
                AimAssist.Start();

            if(Properties.Settings.Default.AutoShoot)
                AutoFire.Start();

            if(Properties.Settings.Default.VisualAssistance)
                VisAssist.Start();

            if(Properties.Settings.Default.EnhancedRadar)
                EnhancedRad.Start();

            if(Properties.Settings.Default.ConstJump)
                ConstJump.Start();

            if(Properties.Settings.Default.FlashGlasses)
                FlashGlasses.Start();     
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        public static void PauseZeroKore()
        {
            try
            {
                AimAssist.Interrupt();
                AimAssist.Join(100);
                AimAssist.Abort();
                AimAssist = null;            
            }
            catch { }
            try
            {
                AutoFire.Interrupt();
                AutoFire.Join(100);
                AutoFire.Abort();
                AutoFire = null;
            }
            catch { }
            try
            {
                VisAssist.Interrupt();
                VisAssist.Join(100);
                VisAssist.Abort();
                VisAssist = null;
            }
            catch { }
            try
            {
                EnhancedRad.Interrupt();
                EnhancedRad.Join(100);
                EnhancedRad.Abort();
                EnhancedRad = null;
            }
            catch { }
            try
            {
                ConstJump.Interrupt();
                ConstJump.Join(100);
                ConstJump.Abort();
                ConstJump = null;
            }
            catch { }
            try
            {
                FlashGlasses.Interrupt();
                FlashGlasses.Join(100);
                FlashGlasses.Abort();
                FlashGlasses = null;
            }
            catch { }
        }

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

        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        public static int StopZeroKore()
        {
            sw.Stop();

            try
            {
                AimAssist.Interrupt();
                AimAssist.Join(100);
                AimAssist.Abort();
                AimAssist = null;
            }
            catch { }
            try
            {
                AutoFire.Interrupt();
                AutoFire.Join(100);
                AutoFire.Abort();
                AutoFire = null;
            }
            catch { }
            try
            {
                VisAssist.Interrupt();
                VisAssist.Join(100);
                VisAssist.Abort();
                VisAssist = null;
            }
            catch { }
            try
            {
                EnhancedRad.Interrupt();
                EnhancedRad.Join(100);
                EnhancedRad.Abort();
                EnhancedRad = null;
            }
            catch { }
            try
            {
                ConstJump.Interrupt();
                ConstJump.Join(100);
                ConstJump.Abort();
                ConstJump = null;
            }
            catch { }
            try
            {
                FlashGlasses.Interrupt();
                FlashGlasses.Join(100);
                FlashGlasses.Abort();
                FlashGlasses = null;
            }
            catch { }

            int Result = (int)sw.Elapsed.TotalMinutes;

            sw.Reset();
            return Result;
        }

    }
}
