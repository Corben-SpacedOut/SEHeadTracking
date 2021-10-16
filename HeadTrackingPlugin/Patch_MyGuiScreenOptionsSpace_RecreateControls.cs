using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.Graphics.GUI;
using SpaceEngineers.Game.GUI;
using Sandbox.Game.Gui;
using HarmonyLib;
using System.Reflection;
using VRageMath;
using VRage.Game;
using VRage.Utils;
using VRage;

namespace HeadTrackingPlugin
{
    [HarmonyPatch]
    public class Patch_MyGuiScreenOptionsSpace_RecreateControls
    {
        public static void Postfix(MyGuiScreenBase __instance)
        {
            var self = __instance;
            Log.Info("Options menu: " + self);

            var credits = self.Controls.Where(c => c.GetType() == typeof(MyGuiControlButton)).Last();
            var separator = self.Controls.Where(c => c.GetType() == typeof(MyGuiControlSeparatorList)).Last();

            var position = credits.Position;

            credits.Position += MyGuiConstants.MENU_BUTTONS_POSITION_DELTA;
            separator.Position += MyGuiConstants.MENU_BUTTONS_POSITION_DELTA;
            self.Size += MyGuiConstants.MENU_BUTTONS_POSITION_DELTA;

            var button = new MyGuiControlButton(
                position: position,
                visualStyle: MyGuiControlButtonStyleEnum.Default,
                size: null,
                colorMask: null,
                originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER,
                toolTip: null,
                text: new StringBuilder("Head Tracking"),
                textScale: 0.8f,
                textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER,
                highlightType: MyGuiControlHighlightType.WHEN_CURSOR_OVER,
                onButtonClick: (_) => {
                    MyGuiSandbox.AddScreen(new HeadTrackingSettingsGui());
                });

            self.Controls.Add(button);
        }

        public static MethodBase TargetMethod()
        {
            Type t = Type.GetType("SpaceEngineers.Game.GUI.MyGuiScreenOptionsSpace, SpaceEngineers.Game");
            var m = t.GetMethod("RecreateControls");
            Log.Info("Method: " + m);
            return m;
        }
    }
}
