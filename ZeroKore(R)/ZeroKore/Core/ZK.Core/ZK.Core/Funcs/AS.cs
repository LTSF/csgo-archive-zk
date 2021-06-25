using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

using ThreadState = System.Threading.ThreadState;

namespace ZK.Core.Funcs
{
    public class AS
    {
        public static Thread AutoShootThread = new Thread(T_ExecAutoShoot);

        [DllImport("ZK.Bridge.dll")]
        private static extern int GetProcessID();

        [DllImport("ZK.Bridge.dll")]
        private static extern int GetClientBaseAddress();

        [DllImport("ZK.Bridge.dll")]
        private static extern int ReadInt32(int ReadAddress);

        [DllImport("ZK.Bridge.dll")]
        private static extern int WriteInt64(int WriteAddress, long Value);

        private static void DoAutoShoot()
        {
            int LocalPlayer = ReadInt32(GetClientBaseAddress() + signatures.dwLocalPlayer);
            int LocalTeam = ReadInt32(LocalPlayer + netvars.m_iTeamNum);
            int CrossHair = ReadInt32(LocalPlayer + netvars.m_iCrosshairId);

            if(CrossHair != 0 && CrossHair < 64)
            {
                int TargetPlayer = ReadInt32(GetClientBaseAddress() + signatures.dwEntityList + (CrossHair - 1) * 0x10);

                if(TargetPlayer != 0)
                {
                    int TargetHealth = ReadInt32(TargetPlayer + netvars.m_iHealth);
                    int TargetTeam = ReadInt32(TargetPlayer + netvars.m_iTeamNum);

                    if(LocalTeam != TargetTeam && TargetHealth > 0)
                    {
                        WriteInt64(GetClientBaseAddress() + signatures.dwForceAttack, 0x5);
                        Thread.Sleep(10);
                        WriteInt64(GetClientBaseAddress() + signatures.dwForceAttack, 0x4);
                        Thread.Sleep(90);
                    }
                }
            }
        }
        private static void T_ExecAutoShoot()
        {
            bool AllowedToRun = true;

            while (AllowedToRun)
            {
                if (AutoShootThread.ThreadState == ThreadState.AbortRequested || AutoShootThread.ThreadState == ThreadState.StopRequested)
                    AllowedToRun = false;
                else
                {
                    try
                    {
                        if (GetProcessID() != 0 && Process.GetProcessById(GetProcessID()) != null)
                            DoAutoShoot();
                    }
                    catch { }
                }

                Thread.Sleep(10);
            }
        }
    }
}
