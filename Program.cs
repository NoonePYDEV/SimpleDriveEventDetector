using System;
using System.IO;
using System.Threading;

namespace DriveEvents
{

    class Program
    {
        static void Main(string[] Args)
        {
            // Explained at line 39

            void OnDrivePlug(DriveInfo PluggedDrive) => Console.WriteLine($"[ PLUGGED ] Name : {PluggedDrive.Name}");

            // Explained at line 40

            void OnDriveUnplug(DriveInfo UnpluggedDrive) => Console.WriteLine($"[ UNPLUGGED ] Name : {UnpluggedDrive.Name}");

            // Display the last event. Can be null / "<Unplug>" / "<PlugIn>"

            void DisplayLastEvent() => Console.WriteLine(DRIVE_EVENT_DETECTOR.GetLastEvent());

            // Display the last drive plugged in. Can be null / DriveInfo

            void DisplayLastPluggedDrive() => Console.WriteLine(DRIVE_EVENT_DETECTOR.GetLastDrive().Name);

            // Display all drives plugged/recently plugged drives

            void DisplayAllDrives()
            {
                foreach (DriveInfo Drive in DRIVE_EVENT_DETECTOR.GetAllDrives(IncludeUnpluggedDrives: true))
                    Console.WriteLine($"- {Drive.Name}");
            }

            // Display all currently plugged drives, including the system root (e.g : C:\, D:\, ...)

            void DisplayAllPluggedDrives()
            {
                foreach (DriveInfo Drive in DRIVE_EVENT_DETECTOR.GetAllDrives(IncludeUnpluggedDrives: false))
                {
                    Console.WriteLine($"- {Drive.Name}");
                }
            }

            // Display the currently plugged drives count

            void DisplayPluggedDrivesCount() => Console.WriteLine(DRIVE_EVENT_DETECTOR.GetTotalDrivesCount());

            DRIVE_EVENT_DETECTOR.Bind("<OnPlugEvent>", OnDrivePlug); // OnDrivePlug() will be executed on every plug in event with the plugged drive in first argument
            DRIVE_EVENT_DETECTOR.Bind("<OnUnplugEvent>", OnDriveUnplug);// OnDriveUnplug() will be executed on every unplug event with the unplugged drive in first argument

            DRIVE_EVENT_DETECTOR.SetDelay(0.1); // Change the delay between every iterations of the main loop

            new Thread(DRIVE_EVENT_DETECTOR.Start).Start(); // new Thread(DRIVE_EVENT_DETECTOR.Start).Start() is for Non blocking listening

            DRIVE_EVENT_DETECTOR.Unbind("<OnPlugEvent>"); // OnDrivePlug() will no longer be called for plug in event
            DRIVE_EVENT_DETECTOR.Unbind("<OnUnplugEvent>"); // OnDriveUnplug() will no longer be called for unplug event

            DRIVE_EVENT_DETECTOR.Stop(); // Stop listening plugs/unplugs events
        }
    } 
}