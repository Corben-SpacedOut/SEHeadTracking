using HarmonyLib;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.Utils;
using VRageMath;

namespace HeadTrackingPlugin
{
    [HarmonyPatch(typeof(MySession), "DrawSync")]
    public class Patch_MySession_DrawSync
    {
        private static bool first = true;

        public static void Postfix()
        {
            if (first)
            {
                first = false;
                Log.Info("MySession.DrawSync patched.");
            }

            SessionComponent.DrawSync();
        }
    }
}
