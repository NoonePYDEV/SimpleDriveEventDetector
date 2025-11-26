# Drive Event Detector

**Drive Event Detector** is a C# library that monitors drive plug-in and unplug events on Windows. It provides a simple API to handle real-time notifications whenever a drive is connected or removed. This library is designed for educational, monitoring, or automation purposes.

## Table of Contents

* [Features](#features)
* [Project Structure](#project-structure)
* [Usage](#usage)
* [Notes](#notes)
* [Quick Setup](#quick-setup)
* [License](#license)

## Features

* Detect drive plug-in and unplug events in real-time.
* Bind custom actions to plug and unplug events.
* Retrieve all currently connected drives or a history of drives.
* Configure the polling delay between drive checks.
* Retrieve information about the last event and last plugged drive.

## Project Structure

```
DriveEventDetector/
│
├─ DriveEventDetector.cs        # Main library containing the DRIVE_EVENT_DETECTOR class
├─ DriveEventDetector.csproj    # Project file
├─ Program.cs                   # Example usage of the library with sample functions
├─ Quick Setup.bat              # Optional batch file for quick setup/build
│
├─ bin/
│   └─ Debug/
│       └─ net9.0/
│
└─ obj/
```

## Usage

1. Include the `DriveEventDetector.cs` in your project.
2. Initialize the detector and bind custom actions:

```csharp
// Bind plug event
DRIVE_EVENT_DETECTOR.Bind("<OnPlugEvent>", drive =>
{
    Console.WriteLine($"Drive plugged: {drive.Name}");
});

// Bind unplug event
DRIVE_EVENT_DETECTOR.Bind("<OnUnplugEvent>", drive =>
{
    Console.WriteLine($"Drive unplugged: {drive.Name}");
});

// Start the detector
DRIVE_EVENT_DETECTOR.Start();
```

3. To stop monitoring drives:

```csharp
DRIVE_EVENT_DETECTOR.Stop();
```

4. You can also access helper functions:

```csharp
// Get all drives (optionally include recently unplugged)
var allDrives = DRIVE_EVENT_DETECTOR.GetAllDrives(includeUnpluggedDrives: true);

// Get last plugged drive
var lastDrive = DRIVE_EVENT_DETECTOR.GetLastDrive();

// Get total drive count
int total = DRIVE_EVENT_DETECTOR.GetTotalDrivesCount();

// Get last event ("<PlugIn>" or "<Unplug>")
string? lastEvent = DRIVE_EVENT_DETECTOR.GetLastEvent();
```

5. Optional: Set the polling delay (default: 0.1 seconds):

```csharp
DRIVE_EVENT_DETECTOR.SetDelay(0.5); // 0.5 seconds between checks
```

## Notes

* The library uses a polling mechanism with a configurable delay. Very short delays may affect performance.
* Designed for Windows platforms; may not be compatible with other operating systems.
* Example functions are provided in `Program.cs` as commented code for reference.

## Quick Setup

To quickly build the project, run the provided `Quick Setup.bat` file. This will restore dependencies and build the project in debug mode.

## License

This project is intended for educational purposes. Modify and use responsibly.
