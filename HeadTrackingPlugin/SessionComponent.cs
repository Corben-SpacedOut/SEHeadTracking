using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VRage.Game.Components;
using VRage.Game.Utils;
using VRageMath;

namespace HeadTrackingPlugin
{
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    internal class SessionComponent : MySessionComponentBase
    {
        public bool TestMode => Settings.TestModeEnabled;

        private static readonly string ConfigFilePath;

        public static SessionComponent Instance { get; private set; }

        public HeadTrackingSettings Settings { get; private set; }

        static SessionComponent()
        {
            var appdata = Environment.GetEnvironmentVariable("appdata");
            ConfigFilePath = appdata + $"/SpaceEngineers/HeadTrackingPlugin.cfg";
        }

        public override void LoadData()
        {
            Settings = HeadTrackingSettings.Load();

            Instance = this;

            MyAPIGateway.Utilities.MessageEntered += Handle_Message;
        }

        protected override void UnloadData()
        {
            Settings.Save();

            Instance = null;
            MyAPIGateway.Utilities.MessageEntered -= Handle_Message;
        }

        private void Handle_Message(string rawMessage, ref bool sendToOthers)
        {
            var message = rawMessage.ToLower();
            if (message.StartsWith("/ht_testmode"))
            {
                sendToOthers = false;

                var split = message.Split();
                if (split.Length == 2)
                {
                    var arg = split[1];
                    if (arg == "on" || arg == "true") Settings.TestModeEnabled= true;
                    if (arg == "off" || arg == "false") Settings.TestModeEnabled = false;
                    if (arg == "toggle") Settings.TestModeEnabled = !Settings.TestModeEnabled;

                }
            }

            var mode = TestMode ? "ON" : "OFF";
            MyAPIGateway.Utilities.ShowMessage("HeadTracking", $"Test mode is {mode}");
        }

        public override void Draw()
        {
            // For a MySessionComponentBase in a plugin, Draw() is called by MySession.DrawAsync().
            // This causes flickering that is most likely due to camera view matrix being updated at the wrong time.
            // For mods, Draw() is called by MySession.DrawSync(). 
            // MySession.DrawSync() is patched to avoid screen flickering.
        }

        private void DoDraw()
        {
            bool isCharacter = MyAPIGateway.Session.Player.Character != MyAPIGateway.Session.CameraController;
            bool isFps = MyAPIGateway.Session.CameraController.IsInFirstPersonView;

            if (isCharacter && isFps)
            {
                var rotX = MatrixD.CreateRotationX(FreeTrackClient.Pitch);
                var rotY = MatrixD.CreateRotationY(-FreeTrackClient.Yaw);
                var rotZ = MatrixD.CreateRotationZ(-FreeTrackClient.Roll);

                var camera = (MyCamera)MyAPIGateway.Session.Camera;

                MatrixD m = camera.ViewMatrix * rotZ * rotY * rotX;
                camera.SetViewMatrix(m);
                camera.UploadViewMatrixToRender();
            }
        }

        public static void DrawSync()
        {
            Instance?.DoDraw();
        }
    }
}
