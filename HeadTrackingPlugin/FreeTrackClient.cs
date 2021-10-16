using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HeadTrackingPlugin
{
    internal class FreeTrackClient
    {
        private static FreeTrackClient _instance;

        private static FreeTrackClient instance
        {
            get
            {
                lock (typeof(FreeTrackClient))
                    if (_instance == null)
                    {
                        _instance = new FreeTrackClient();
                        _instance.Start();
                    }
                return _instance;
            }
        }

        public static float Pitch { get { return instance.pitch_; } }
        public static float Yaw { get { return instance.yaw_; } }
        public static float Roll { get { return instance.roll_; } }

        private float pitch_, yaw_, roll_;

        private double startTime = 1.0 * System.DateTime.Now.Ticks / TimeSpan.TicksPerSecond;

        private readonly long updateTimeout = TimeSpan.TicksPerSecond * 2;

        private long lastUpdateTime = 0;

        private uint previousDataID = 0xFFFFFFFF;

#pragma warning disable 0649
        // https://github.com/opentrack/opentrack/blob/master/freetrackclient/fttypes.h
        private struct FTData
        {
            // DataID is updated when data changes.
            public uint DataID;
            public int CamWidth;
            public int CamHeight;

            /* virtual pose */
            public float Yaw;   /* positive yaw to the left */
            public float Pitch; /* positive pitch up */
            public float Roll;  /* positive roll to the left */
            public float X;
            public float Y;
            public float Z;
        }
#pragma warning restore 0649

        private int SIZEOF_FTDATA = sizeof(int) * 3 + sizeof(float) * 6; // sizeof(FTData)

        private string FT_MUTEX = "FT_Mutext";
        private string FT_MEM = @"FT_SharedMem";

        private Mutex mutex;

        private MemoryMappedViewAccessor viewAccessor;

        private MemoryMappedFile mappedFile;

        private Thread thread;
        private volatile bool running;
        private void Start()
        {
            lock (this)
                if (thread == null)
                {
                    thread = new Thread(ThreadRun);
                    thread.IsBackground = true;
                    running = true;
                    thread.Start();
                }
        }

        private void ThreadRun()
        {
            try
            {
                while (running)
                {
                    if (Update())
                        Thread.Sleep(16);
                    else
                        Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                Log.Error("FreeTrack client thread exception: " + ex);
                Log.Info(ex.StackTrace);
                lock (this)
                {
                    thread = null;
                }
            }
            Log.Error("Head Tracking Thread Stopped!");
        }

        private void Acquire()
        {
            if (mutex is null && Mutex.TryOpenExisting(FT_MUTEX, out mutex))
            {
                mappedFile = MemoryMappedFile.OpenExisting(FT_MEM, MemoryMappedFileRights.Read);
                viewAccessor = mappedFile.CreateViewAccessor(0, SIZEOF_FTDATA, MemoryMappedFileAccess.Read);

                Log.Info("Client connected!");
            }
        }
        private void Release()
        {
            if (mutex != null)
            {
                Log.Info("Client disconnected.");

                mutex.Dispose();
                mutex = null;

                viewAccessor.Dispose();
                viewAccessor = null;
            }
        }

        private bool Update()
        {
            bool updated = false;

            float pitch = 0, roll = 0, yaw = 0;

            Acquire();

            if (mutex != null && viewAccessor != null)
            {
                if (mutex.WaitOne(1000))
                {
                    FTData data;
                    viewAccessor.Read(0, out data);

                    if (data.DataID != previousDataID)
                    {
                        lastUpdateTime = System.DateTime.Now.Ticks;
                        previousDataID = data.DataID;
                    }

                    yaw = data.Yaw;
                    pitch = data.Pitch;
                    roll = data.Roll;

                    updated = true;

                    mutex.ReleaseMutex();
                }
                else
                {
                    Log.Info("Mutex timed out!");
                    Release();
                }
            }
            else
            {
                yaw = 0;
                pitch = 0;
                roll = 0;
            }

            if (SessionComponent.Instance?.TestMode ?? false)
            {
                double time = 1.0 * System.DateTime.Now.Ticks / TimeSpan.TicksPerSecond;

                yaw += 0.5f * (float)Math.Sin(time - startTime);
                pitch += 0.5f * (float)Math.Cos(time - startTime);

                updated = true;
            }

            roll_ = roll;
            pitch_ = pitch;
            yaw_ = yaw;

            if (System.DateTime.Now.Ticks - lastUpdateTime > updateTimeout)
            {
                Release();
            }

            return updated;
        }
    }
}
