using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

using ThreadState = System.Threading.ThreadState;

namespace ZK.Core.Funcs
{
    public class FG
    {
        public static Thread FlashGlassesThread = new Thread(T_ExecFlashGlasses);

        [DllImport("ZK.Bridge.dll")]
        private static extern int GetProcessID();

        [DllImport("ZK.Bridge.dll")]
        private static extern int GetClientBaseAddress();

        [DllImport("ZK.Bridge.dll")]
        private static extern int ReadInt32(int ReadAddress);

        [DllImport("ZK.Bridge.dll")]
        private static extern int WriteInt32(int WriteAddress, int Value);

        private static void DoFlashGlasses()
        {
            int Duration = 0;

            int LocalPlayer = ReadInt32(GetClientBaseAddress() + signatures.dwLocalPlayer);
            Duration = ReadInt32(LocalPlayer + netvars.m_flFlashDuration);

            if (Duration > 0)
                WriteInt32(LocalPlayer + netvars.m_flFlashDuration, 0);

            Thread.Sleep(50);
        }

        private static void T_ExecFlashGlasses()
        {
            bool AllowedToRun = true;

            while (AllowedToRun)
            {
                if (FlashGlassesThread.ThreadState == ThreadState.AbortRequested || FlashGlassesThread.ThreadState == ThreadState.StopRequested)
                    AllowedToRun = false;
                else
                {
                    try
                    {
                        if (GetProcessID() != 0 && Process.GetProcessById(GetProcessID()) != null)
                            DoFlashGlasses();
                    }
                    catch { }
                }

                Thread.Sleep(10);
            }
        }
    }
}
