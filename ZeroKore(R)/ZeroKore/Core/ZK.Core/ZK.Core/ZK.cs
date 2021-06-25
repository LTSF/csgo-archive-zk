using System.Drawing;
using ZK.Core.Funcs;

namespace ZK.Core
{
    public class FunctionManager
    {
        public class AimAssist
        {
            public static void Start()
            {
                AA.AimAssistThread.Start();
            }
            public static void Stop()
            {
                AA.AimAssistThread.Abort();
            }
        }
        public class ConstJump
        {
            public static void Start()
            {
                CJ.ConstJumpThread.Start();
            }
            public static void Stop()
            {
                CJ.ConstJumpThread.Abort();
            }
        }
        public class FlashGlasses
        {
            public static void Start()
            {
                FG.FlashGlassesThread.Start();
            }
            public static void Stop()
            {
                FG.FlashGlassesThread.Abort();
            }
        }
        public class VisualAssistance
        {
            public static void Start()
            {
                VA.VisualAssistanceThread.Start();
            }
            public static void Stop()
            {
                VA.VisualAssistanceThread.Abort();
            }

            public static void SetTeamColor(Color col)
            {
                VA.TeamColor = col;
            }
            public static void SetEenemyColor(Color col)
            {
                VA.EnemyColor = col;
            }
        }
        public class EnhancedRadar
        {
            public static void Start()
            {
                ER.EnhancedRadarThread.Start();
            }
            public static void Stop()
            {
                ER.EnhancedRadarThread.Abort();
            }
        }
        public class AutoShoot
        {
            public static void Start()
            {
                AS.AutoShootThread.Start();
            }
            public static void Stop()
            {
                AS.AutoShootThread.Abort();
            }
        }

    }
}
