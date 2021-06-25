using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

using ThreadState = System.Threading.ThreadState;

namespace ZK.Core.Funcs
{
    public class ER
    {
        public static Thread EnhancedRadarThread = new Thread(T_ExecEnhancedRadar);

        [DllImport("ZK.Bridge.dll")]
        private static extern int GetProcessID();

        [DllImport("ZK.Bridge.dll")]
        private static extern int GetClientBaseAddress();

        [DllImport("ZK.Bridge.dll")]
        private static extern int ReadInt32(int ReadAddress);

        [DllImport("ZK.Bridge.dll")]
        private static extern bool ReadBool(int ReadAddress);

        [DllImport("ZK.Bridge.dll")]
        private static extern bool WriteBool(int WriteAddress, bool Value);


        private static void DoEnhancedRadar()
        {
            int LocalPlayer = ReadInt32(GetClientBaseAddress() + signatures.dwLocalPlayer);
            int LocalTeam = ReadInt32(LocalPlayer + netvars.m_iTeamNum);

            for(int i = 1; i < 64; i++)
            {
                int Entity = ReadInt32(GetClientBaseAddress() + signatures.dwEntityList + i * 0x10);
                int EntTeam = ReadInt32(Entity + netvars.m_iTeamNum);
                bool EntDormant = ReadBool(Entity + signatures.m_bDormant);
                bool EntSpotted = ReadBool(Entity + netvars.m_bSpotted);

                if (!EntDormant && EntTeam != LocalTeam)
                    if (!EntSpotted)
                        WriteBool(Entity + netvars.m_bSpotted, true);
            }

            Thread.Sleep(20);
        }

        private static void T_ExecEnhancedRadar()
        {
            bool AllowedToRun = true;

            while (AllowedToRun)
            {
                if (EnhancedRadarThread.ThreadState == ThreadState.AbortRequested || EnhancedRadarThread.ThreadState == ThreadState.StopRequested)
                    AllowedToRun = false;
                else
                {
                    try
                    {
                        if (GetProcessID() != 0 && Process.GetProcessById(GetProcessID()) != null)
                            DoEnhancedRadar();
                    }
                    catch { }
                }

                Thread.Sleep(10);
            }
        }
    }
}
