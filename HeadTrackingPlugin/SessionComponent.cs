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
        public bool TestMode { get; private set; }

        private static readonly string ConfigFilePath;

        public static SessionComponent Instance { get; private set; }

        static SessionComponent()
        {
            var appdata = Environment.GetEnvironmentVariable("appdata");
            ConfigFilePath = appdata + $"/SpaceEngineers/HeadTrackingPlugin.cfg";
        }

        public override void LoadData()
        {
            Instance = this;
            MyAPIGateway.Utilities.MessageEntered += Handle_Message;

            // Read TestMode from config file.
            if (File.Exists(ConfigFilePath))
            {
                var rx = new Regex(@"^\s*testmode\s*=\s*(on|true)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                foreach (var line in File.ReadAllLines(ConfigFilePath))
                {
                    if (rx.IsMatch(line)) TestMode = true;
                }
            }
        }

        protected override void UnloadData()
        {
            Instance = null;
            MyAPIGateway.Utilities.MessageEntered -= Handle_Message;

            // Only write config, if the file exists or defaults have changed.
            if (File.Exists(ConfigFilePath) || TestMode)
            {
                var mode = TestMode ? "on" : "off";
                File.WriteAllText(ConfigFilePath, $"testmode = {mode}\n");
            }
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
                    if (arg == "on" || arg == "true") TestMode = true;
                    if (arg == "off" || arg == "false") TestMode = false;
                    if (arg == "toggle") TestMode = !TestMode;

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
            var rotX = MatrixD.CreateRotationX(FreeTrackClient.Pitch);
            var rotY = MatrixD.CreateRotationY(-FreeTrackClient.Yaw);
            var rotZ = MatrixD.CreateRotationZ(-FreeTrackClient.Roll);

            var camera = (MyCamera)MyAPIGateway.Session.Camera;

            MatrixD m = camera.ViewMatrix * rotZ * rotY * rotX;
            camera.SetViewMatrix(m);
            camera.UploadViewMatrixToRender();
        }

        public static void DrawSync()
        {
            Instance?.DoDraw();
        }
    }
}
