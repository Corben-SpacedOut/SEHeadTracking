using HarmonyLib;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VRage.Plugins;

namespace HeadTrackingPlugin
{
    public class HeadTrackingPlugin : IPlugin
    {
        public void Dispose()
        {
        }

        public void Init(object gameInstance)
        {
            Log.Info("Plugin Init.");

            Log.Info("Type: " + Type.GetType("SpaceEngineers.Game.GUI.MyGuiScreenOptionsSpace, SpaceEngineers.Game"));

            new Harmony("com.corben.spacedout.HeadTrackingPlugin").PatchAll(Assembly.GetExecutingAssembly());
        }

        public void Update()
        {
        }
    }
}
