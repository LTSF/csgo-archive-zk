using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.Drawing;

using ThreadState = System.Threading.ThreadState;


namespace ZK.Core.Funcs
{
    public class VA
    {
        public static Color TeamColor { get; set; } = Color.FromArgb(0, 255, 0);
        public static Color EnemyColor { get; set; } = Color.FromArgb(255, 0, 0);

        public static Thread VisualAssistanceThread = new Thread(T_ExecVisualAssistance);

        [DllImport("ZK.Bridge.dll")]
        private static extern int GetProcessID();

        [DllImport("ZK.Bridge.dll")]
        private static extern int GetClientBaseAddress();

        [DllImport("ZK.Bridge.dll")]
        private static extern int ReadInt32(int ReadAddress);

        [DllImport("ZK.Bridge.dll")]
        private static extern bool ReadBool(int ReadAddress);

        [DllImport("ZK.Bridge.dll")]
        private static extern bool WriteFloat(int WriteAddress, float Value);

        [DllImport("ZK.Bridge.dll")]
        private static extern bool WriteBool(int WriteAddress, bool Value);

        private static void DoVisualAssistance()
        {
            int LocalPlayer = ReadInt32(GetClientBaseAddress() + signatures.dwLocalPlayer);
            int LocalTeam = ReadInt32(LocalPlayer + netvars.m_iTeamNum);
            int LocalHealth = ReadInt32(LocalPlayer + netvars.m_iHealth);

            int GlowObj = ReadInt32(GetClientBaseAddress() + signatures.dwGlowObjectManager);

            if (LocalTeam == 0x1 || LocalTeam == 0x0 || LocalHealth > 100 || LocalHealth < 1)
                return;

            for (int i = 1; i < 64; i++)
            {
                int Entity = ReadInt32(GetClientBaseAddress() + signatures.dwEntityList + i * 0x10);

                if(Entity != 0)
                {
                    bool EntDormant = ReadBool(Entity + signatures.m_bDormant);

                    if (EntDormant)
                        return;

                    int EntTeam = ReadInt32(Entity + netvars.m_iTeamNum);
                    int EntGlow = ReadInt32(Entity + netvars.m_iGlowIndex);

                    if (LocalTeam == EntTeam)
                    {
                        WriteFloat(GlowObj + EntGlow * 0x38 + 0x4, TeamColor.R / 255);
                        WriteFloat(GlowObj + EntGlow * 0x38 + 0x8, TeamColor.G / 255);
                        WriteFloat(GlowObj + EntGlow * 0x38 + 0xC, TeamColor.B / 255);
                        WriteFloat(GlowObj + EntGlow * 0x38 + 0x10, 0.7f);

                        WriteBool(GlowObj + EntGlow * 0x38 + 0x24, true);
                        WriteBool(GlowObj + EntGlow * 0x38 + 0x25, false);
                    }
                    else
                    {
                        WriteFloat(GlowObj + EntGlow * 0x38 + 0x4, EnemyColor.R / 255);
                        WriteFloat(GlowObj + EntGlow * 0x38 + 0x8, EnemyColor.G / 255);
                        WriteFloat(GlowObj + EntGlow * 0x38 + 0xC, EnemyColor.B / 255);
                        WriteFloat(GlowObj + EntGlow * 0x38 + 0x10, 0.7f);

                        WriteBool(GlowObj + EntGlow * 0x38 + 0x24, true);
                        WriteBool(GlowObj + EntGlow * 0x38 + 0x25, false);
                    }
                }
            }

        }

        private static void T_ExecVisualAssistance()
        {
            bool AllowedToRun = true;

            while (AllowedToRun)
            {
                if (VisualAssistanceThread.ThreadState == ThreadState.AbortRequested || VisualAssistanceThread.ThreadState == ThreadState.StopRequested)
                    AllowedToRun = false;
                else
                {
                    try
                    {
                        if (GetProcessID() != 0 && Process.GetProcessById(GetProcessID()) != null)
                            DoVisualAssistance();
                    }
                    catch { }
                }

                Thread.Sleep(10);
            }
        }
    }
}
