using System;
using System.Runtime.InteropServices;
using System.Numerics;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

using ThreadState = System.Threading.ThreadState;

namespace ZK.Core.Funcs
{
    public class AA
    {
        public static Thread AimAssistThread = new Thread(T_ExecAimAssist);

        [DllImport("ZK.Bridge.dll")]
        private static extern int GetProcessID();

        [DllImport("ZK.Bridge.dll")]
        private static extern int GetClientBaseAddress();

        [DllImport("ZK.Bridge.dll")]
        private static extern int GetEngineBaseAddress();

        [DllImport("ZK.Bridge.dll")]
        private static extern int ReadInt32(int ReadAddress);

        [DllImport("ZK.Bridge.dll")]
        private static extern bool ReadBool(int ReadAddress);

        [DllImport("ZK.Bridge.dll")]
        private static extern Vector3 ReadVector(int ReadAddress);

        [DllImport("ZK.Bridge.dll")]
        private static extern BoneMatrix ReadBM(int ReadAddress);

        [DllImport("ZK.Bridge.dll")]
        private static extern ViewMatrix ReadVM(int ReadAddress);

        [DllImport("ZK.Bridge.dll")]
        private static extern bool WriteFloat(int WriteAddress, float Value);

        [DllImport("ZK.Bridge.dll")]
        private static extern bool WriteBool(int WriteAddress, bool Value);


        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int x, int y);


        private static int SC_WIDTH = Screen.PrimaryScreen.Bounds.Width;
        private static int SC_HEIGHT = Screen.PrimaryScreen.Bounds.Height;

        private static int CH_POSX = SC_WIDTH / 2;
        private static int CH_POSY = SC_HEIGHT / 2;

        private static int GetTeam(int Player)
        {
            return ReadInt32(Player + netvars.m_iTeamNum);
        }
        private static int GetLocalPlayer()
        {
            return ReadInt32(GetClientBaseAddress() + signatures.dwLocalPlayer);
        }
        private static int GetPlayerByIndex(int Index)
        {
            return ReadInt32(GetClientBaseAddress() + signatures.dwEntityList + (Index * 0x10));
        }
        private static int GetHealth(int Player)
        {
            return ReadInt32(Player + netvars.m_iHealth);
        }
        private static bool IsDormant(int Player)
        {
            return ReadBool(Player + signatures.m_bDormant);
        }
        private static bool IsInvuln(int Player)
        {
            return ReadBool(Player + netvars.m_bGunGameImmunity);
        }
        private static Vector3 GetLocation(int Player)
        {
            return ReadVector(Player + netvars.m_vecOrigin);
        }

        private unsafe struct BoneMatrix
        {
            public fixed byte pad3[12];
            public float x;
            public fixed byte pad1[12];
            public float y;
            public fixed byte pad2[12];
            public float z;
        }
        private static unsafe Vector3 GetHeadPosition(int Player)
        {
            int BM_Base = ReadInt32(Player + netvars.m_dwBoneMatrix);
            BoneMatrix BM = ReadBM(BM_Base + (sizeof(BoneMatrix) * 8));

            return new Vector3(BM.x, BM.y, BM.z);
        }

        private unsafe struct ViewMatrix
        {
            public fixed float matrix[16];
        }
        private static ViewMatrix vm;

        private static unsafe Vector3 WorldToScreen(Vector3 Position, ViewMatrix V_Matrix)
        {
            Vector3 Result;

            float tX = V_Matrix.matrix[0] * Position.X + V_Matrix.matrix[1] * Position.Y + V_Matrix.matrix[2] * Position.Z + V_Matrix.matrix[3];
            float tY = V_Matrix.matrix[4] * Position.X + V_Matrix.matrix[5] * Position.Y + V_Matrix.matrix[6] * Position.Z + V_Matrix.matrix[7];
            Result.Z = V_Matrix.matrix[12] * Position.X + V_Matrix.matrix[13] * Position.Y + V_Matrix.matrix[14] * Position.Z + V_Matrix.matrix[15];

            tX *= 1.0f / Result.Z;
            tY *= 1.0f / Result.Z;

            Result.X = SC_WIDTH * 0.5f;
            Result.Y = SC_HEIGHT * 0.5f;

            Result.X += 0.5f * tX * SC_WIDTH + 0.5F;
            Result.Y -= 0.5f * tY * SC_HEIGHT + 0.5f;

            return Result;
        }

        private static float PythagorasFunc(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        private static float GetDistance(Vector3 from, Vector3 to)
        {
            Vector3 delta = new Vector3(from.X - to.X, from.Y - to.Y, from.Z - to.Z);
            return (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y + delta.Z * delta.Z);
        }

        private static int FindClosestEnemyIndex()
        {
            float Finish;
            int ClosestEntity = 0;
            float Closest = float.MaxValue;

            int LocalTeam = GetTeam(GetLocalPlayer());

            for (int i = 1; i < 64; i++)
            {
                int CurEnt = GetPlayerByIndex(i);
                int CurTeam = GetTeam(CurEnt);
                int CurHealth = GetHealth(CurEnt);
                bool CurDormant = IsDormant(CurEnt);
                Vector3 CurHB = WorldToScreen(GetHeadPosition(CurEnt), vm);
                bool CurIsImmune = IsInvuln(CurEnt);

                if (CurTeam == LocalTeam || CurDormant || CurHealth > 100 || CurHealth < 1 || CurIsImmune)
                    continue;

                Finish = PythagorasFunc(CurHB.X, CurHB.Y, CH_POSX, CH_POSY);

                if(Finish < Closest)
                {
                    Closest = Finish;
                    ClosestEntity = i;
                }
            }

            return ClosestEntity;
        }

        private static void DoAimAssist()
        {
            vm = ReadVM(GetClientBaseAddress() + signatures.dwViewMatrix);
            int ClosestTarget = FindClosestEnemyIndex();

            int GlowObj = ReadInt32(GetClientBaseAddress() + signatures.dwGlowObjectManager);
            int Entity = GetPlayerByIndex(ClosestTarget);
            int EntGlow = ReadInt32(Entity + netvars.m_iGlowIndex);

            Vector3 TheirHead = WorldToScreen(GetHeadPosition(Entity), vm);

            WriteFloat(GlowObj + EntGlow * 0x38 + 0x4, 1);
            WriteFloat(GlowObj + EntGlow * 0x38 + 0x8, 1);
            WriteFloat(GlowObj + EntGlow * 0x38 + 0xC, 1);
            WriteFloat(GlowObj + EntGlow * 0x38 + 0x10, 0.7f);

            WriteBool(GlowObj + EntGlow * 0x38 + 0x24, true);
            WriteBool(GlowObj + EntGlow * 0x38 + 0x25, false);

            if(GetAsyncKeyState(Keys.Menu) != 0 && TheirHead.Z >= 0.001f && ClosestTarget != 0)
            {
                while(GetAsyncKeyState(Keys.Menu) != 0 && GetHealth(Entity) > 0)
                {
                    vm = ReadVM(GetClientBaseAddress() + signatures.dwViewMatrix);

                    WriteFloat(GlowObj + EntGlow * 0x38 + 0x4, 0);
                    WriteFloat(GlowObj + EntGlow * 0x38 + 0x8, 1);
                    WriteFloat(GlowObj + EntGlow * 0x38 + 0xC, 1);
                    WriteFloat(GlowObj + EntGlow * 0x38 + 0x10, 0.7f);

                    WriteBool(GlowObj + EntGlow * 0x38 + 0x24, true);
                    WriteBool(GlowObj + EntGlow * 0x38 + 0x25, false);

                    TheirHead = WorldToScreen(GetHeadPosition(Entity), vm);

                    Vector3 MyPos = GetLocation(GetLocalPlayer());
                    Vector3 TheirPos = GetLocation(Entity);
                    float Dist = GetDistance(TheirPos, MyPos);

                    float HeightDiff = MyPos.Y - TheirPos.Y;

                    if (Dist > 1000)
                        SetCursorPos((int)TheirHead.X, (int)TheirHead.Y + (int)(HeightDiff / Dist));
                    else if (Dist > 500 && Dist < 1000)
                        SetCursorPos((int)TheirHead.X, (int)TheirHead.Y + (int)(HeightDiff / Dist) / 2);
                    else if (Dist > 100 && Dist < 500)
                        SetCursorPos((int)TheirHead.X, (int)TheirHead.Y + (int)(HeightDiff / Dist) / 4);
                    else
                        SetCursorPos((int)TheirHead.X, (int)TheirHead.Y);

                    Thread.Sleep(70);
                }
            }
        }
        
        private static void T_ExecAimAssist()
        {
            bool AllowedToRun = true;

            while(AllowedToRun)
            {
                if (AimAssistThread.ThreadState == ThreadState.AbortRequested || AimAssistThread.ThreadState == ThreadState.StopRequested)
                    AllowedToRun = false;
                else
                {
                    try
                    {
                        if (GetProcessID() != 0 && Process.GetProcessById(GetProcessID()) != null)
                            DoAimAssist();
                    }
                    catch { }
                }

                Thread.Sleep(10);
            }
        }
           
    }
}
