using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using VRage.FileSystem;

namespace HeadTrackingPlugin
{
    public class HeadTrackingSettings
    {
        private const string FileName = "HeadTrackingSettings.xml";
        private static string FilePath => Path.Combine(MyFileSystem.UserDataPath, FileName);

        public bool Enabled = true;
        public bool TestModeEnabled = false;
        public bool EnabledInCharacter = true;
        public bool EnabledInFirstPerson = false;

        public bool InvertPitch = false;
        public bool InvertYaw = false;
        public bool InvertRoll = false;

        public static HeadTrackingSettings Load()
        {
            string file = FilePath;
            if (File.Exists(file))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(HeadTrackingSettings));
                    using (XmlReader xml = XmlReader.Create(file))
                    {
                        return (HeadTrackingSettings)serializer.Deserialize(xml);
                    }
                }
                catch(Exception ex) {
                    Log.Error("Failed to deserialize settings. Using defaults.\nException:\n" + ex);
                }
            }

            return new HeadTrackingSettings();
        }

        public void Save()
        {
            try
            {
                string file = FilePath;
                Directory.CreateDirectory(Path.GetDirectoryName(file));
                XmlSerializer serializer = new XmlSerializer(typeof(HeadTrackingSettings));
                using (StreamWriter stream = File.CreateText(file))
                {
                    serializer.Serialize(stream, this);
                }
            }
            catch(Exception ex) {
                Log.Error("Failed to save settings:\n" + ex);
            }
        }
    }
}
