using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace DriveEvents
{
    class DRIVE_EVENT_DETECTOR
    {
        private static DriveInfo? LastDrive = null;
        private static int TotalDrives = 0;
        private static List<DriveInfo> CurrentDrives = new List<DriveInfo> { };
        private static List<DriveInfo> AllDrives = new List<DriveInfo> { };

        private static bool Running = false;
        private static double Delay = 0.1;

        private static Action<DriveInfo?>? OnPlugAction = null;
        private static Action<DriveInfo?>? OnUnplugAction = null;

        public static string? LastEvent = null;

        private static void MainLoop()
        {
            while (Running)
            {
                List<DriveInfo> _AllFounds = new List<DriveInfo>();

                DriveInfo[] LastDrives = DriveInfo.GetDrives();

                foreach (DriveInfo drive in LastDrives)
                {
                    if (!CurrentDrives.Any(d => d.Name == drive.Name))
                    {
                        LastDrive = drive;

                        if (!AllDrives.Any(d => d.Name == drive.Name))
                            AllDrives.Add(drive);

                        CurrentDrives.Add(drive);
                        _AllFounds.Add(drive);

                        TotalDrives++;
                        LastEvent = "<PlugIn>";

                        OnPlugAction?.Invoke(drive);
                    }
                    else
                    {
                        _AllFounds.Add(drive);
                    }
                }

                foreach (DriveInfo drive in CurrentDrives.ToList())
                {
                    if (!_AllFounds.Any(d => d.Name == drive.Name))
                    {
                        CurrentDrives.RemoveAll(d => d.Name == drive.Name);
                        TotalDrives--;
                        LastEvent = "<Unplug>";
                        OnUnplugAction?.Invoke(drive);
                    }
                }

                Thread.Sleep(Convert.ToInt32(Delay * 1000));
            }
        }

        /// <summary>
        /// Start listening the drive plugs in/unplugs events
        /// </summary>
        public static void Start()
        {
            DriveInfo[] _Drives = DriveInfo.GetDrives();

            AllDrives.AddRange(_Drives);
            CurrentDrives.AddRange(_Drives);

            Running = true;

            MainLoop();
        }

        /// <summary>
        /// Stop listening the drives plugs/unplugs events
        /// </summary>
        public static void Stop()
        {
            Running = false;
        }

        /// <summary>
        /// Bind a function with an event.
        /// </summary>
        /// <param name="ActionName">
        /// The event name. Can be "&lt;OnPlugEvent&gt;" or "&lt;OnUnplugEvent&gt;"
        /// </param>
        /// <param name="Func">
        /// The function that will be called by the event. 
        /// The plugged/unplugged drive (type DriveInfo) is passed in argument.
        /// </param>
        public static void Bind(string ActionName, Action<DriveInfo> Func)
        {
            switch (ActionName)
            {
                case "<OnPlugEvent>":
                    {
                        OnPlugAction = Func;
                        break;
                    }
                    ;
                case "<OnUnplugEvent>":
                    {
                        OnUnplugAction = Func;
                        break;
                    }
                    ;
                default:
                    {
                        break;
                    }
            }
        }

        /// <summary>
        /// Unbind an event : the binded function will no longer be called.
        /// </summary>
        /// <param name="ActionName">
        /// The event name. Can be "&lt;OnPlugEvent&gt;" or "&lt;OnUnplugEvent&gt;"
        /// </param>
        public static void Unbind(string ActionName)
        {
            switch (ActionName)
            {
                case "<OnPlugEvent>":
                    {
                        OnPlugAction = null;
                        break;
                    }
                case "<OnUnplugEvent>":
                    {
                        OnUnplugAction = null;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        /// <summary>
        /// Change the delay between every iterations of the main loop.
        /// </summary>
        /// <param name="DelayInSeconds">
        /// The delay between every iterations of the main loop.
        /// ⚠️ Little warning : The delay (in seconds) will be converted in an int milliseconds value so can be least accurate for values with many decimal places.
        /// </param>
        public static void SetDelay(double DelayInSeconds)
        {
            Delay = DelayInSeconds;
        }

        /// <summary>
        /// Get all plugged drives.
        /// </summary>
        /// <param name="IncludeUnpluggedDrives">
        /// true if you want to include the recently plugged drives, event if they aren't plugged in anymore.
        /// </param>
        /// <returns>
        /// All the drives (DriveInfo[] - DriveInfo array)
        /// </returns>
        public static DriveInfo[] GetAllDrives(bool IncludeUnpluggedDrives = false)
        {
            if (IncludeUnpluggedDrives)
                return CurrentDrives.ToArray();
            else
                return AllDrives.ToArray();
        }

        /// <summary>
        /// Get the total drives count including recently plugged drives event if they aren't plugged in anymore.
        /// </summary>
        /// <returns>
        /// The total drives count including recently plugged drives event if they aren't plugged in anymore (int).
        /// </returns>
        public static int GetTotalDrivesCount() => TotalDrives;

        /// <summary>
        /// Get the last event. Can be "&lt;Plug&gt;", "&lt;Unplug&gt;", or null.
        /// </summary>
        /// <returns>
        /// The last event name (string) or null.
        /// </returns>
        public static string? GetLastEvent() => LastEvent;

        /// <summary>
        /// Get the last plugged in drive.
        /// </summary>
        /// <returns>
        /// The last plugged in drive (DriveInfo) or null.
        /// </returns>
        public static DriveInfo? GetLastDrive() => LastDrive;
    }
}