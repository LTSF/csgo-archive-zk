using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

using ThreadState = System.Threading.ThreadState;

namespace ZK.Core.Funcs
{
    public class CJ
    {
        public static Thread ConstJumpThread = new Thread(T_ExecConstJump);

        [DllImport("ZK.Bridge.dll")]
        private static extern int GetProcessID();

        [DllImport("ZK.Bridge.dll")]
        private static extern int GetClientBaseAddress();

        [DllImport("ZK.Bridge.dll")]
        private static extern int ReadInt32(int ReadAddress);

        [DllImport("ZK.Bridge.dll")]
        private static extern bool ReadBool(int ReadAddress);

        [DllImport("ZK.Bridge.dll")]
        private static extern int WriteInt64(int WriteAddress, long Value);


        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);


        private static void DoConstJump()
        {
            int LocalPlayer = ReadInt32(GetClientBaseAddress() + signatures.dwLocalPlayer);
            bool IsOnGround = ReadBool(LocalPlayer + netvars.m_fFlags);

            if(((GetAsyncKeyState(Keys.Space) & 0x8000) != 0) && (IsOnGround & 1 == 1))
            {
                WriteInt64(GetClientBaseAddress() + signatures.dwForceJump, 0x5);
                Thread.Sleep(30);
                WriteInt64(GetClientBaseAddress() + signatures.dwForceJump, 0x4);
            }

            Thread.Sleep(20);
        }

        private static void T_ExecConstJump()
        {
            bool AllowedToRun = true;

            while (AllowedToRun)
            {
                if (ConstJumpThread.ThreadState == ThreadState.AbortRequested || ConstJumpThread.ThreadState == ThreadState.StopRequested)
                    AllowedToRun = false;
                else
                {
                    try
                    {
                        if (GetProcessID() != 0 && Process.GetProcessById(GetProcessID()) != null)
                            DoConstJump();
                    }
                    catch { }
                }

                Thread.Sleep(10);
            }
        }
    }
}
