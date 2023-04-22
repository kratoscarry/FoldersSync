using System;
using System.IO;

namespace FoldersSync
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set the source and destination folders to sync
            string sourceFolder = @"C:\Users\be_ha\Desktop\folder1"; // =args[0] to execute from outside
            string destFolder = @"C:\Users\be_ha\Desktop\folder3"; // =args[1] to execute from outside

            // Set the interval for periodic syncing (in minutes)
            int syncInterval = 1; // args[2]

            // Set the log file path and create a StreamWriter object
            string logFilePath = @"C:\Users\be_ha\Desktop\log.txt"; // args[3]
            StreamWriter logFile = new StreamWriter(logFilePath, true);

            // Write the initial sync time to the log file
            logFile.WriteLine($"Sync started at {DateTime.Now}");
            Console.WriteLine($"Sync started at {DateTime.Now}");

            // Loop indefinitely for periodic syncing
            while (true)
            {
                try
                {
                    // Perform the sync operation using the SyncDirectories method
                    SyncDirectories(sourceFolder, destFolder, logFile);

                    // Write the sync success message to the log file
                    logFile.WriteLine($"Sync completed at {DateTime.Now}");
                    Console.WriteLine($"Sync completed at {DateTime.Now}");
                }
                catch (Exception ex)
                {
                    // Write the sync error message to the log file
                    logFile.WriteLine($"Sync failed at {DateTime.Now}: {ex.Message}");
                }

                // Wait for the specified interval before performing the next sync
                System.Threading.Thread.Sleep(syncInterval * 60 * 1000);
            }

            // Close the log file
            logFile.Close();
        }

        static void SyncDirectories(string sourceFolder, string destFolder, StreamWriter logFile)
        {
            // Get the subdirectories of the source folder
            DirectoryInfo sourceDir = new DirectoryInfo(sourceFolder);
            DirectoryInfo[] subDirs = sourceDir.GetDirectories();

            // Create the destination folder if it doesn't exist
            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }

            // Copy the files from the source folder to the destination folder
            foreach (FileInfo file in sourceDir.GetFiles())
            {
                string destFilePath = Path.Combine(destFolder, file.Name);
                if (File.Exists(destFilePath))
                {
                    if (file.LastWriteTime > File.GetLastWriteTime(destFilePath))
                    {
                        // Source file is newer than the target file, overwrite the target file
                        file.CopyTo(destFilePath, true);
                        logFile.WriteLine($"Copied {file.FullName} to {destFilePath}");
                        Console.WriteLine($"Copied {file.FullName} to {destFilePath}");
                    }
                }
                else
                {
                    // Target file doesn't exist, copy the source file
                    file.CopyTo(destFilePath);
                    logFile.WriteLine($"Copied {file.FullName} to {destFilePath}");
                    Console.WriteLine($"Copied {file.FullName} to {destFilePath}");
                }
            }

            // Delete the files from the destination folder that don't exist in the source folder
            foreach (FileInfo file in new DirectoryInfo(destFolder).GetFiles())
            {
                string sourceFilePath = Path.Combine(sourceFolder, file.Name);
                if (!File.Exists(sourceFilePath))
                {
                    // Source file doesn't exist, delete the target file
                    file.Delete();
                    logFile.WriteLine($"Deleted {file.FullName} from {destFolder}");
                    Console.WriteLine($"Deleted {file.FullName} from {destFolder}");
                }
            }

            // Recursively sync the subdirectories from the source folder to the destination folder
            foreach (DirectoryInfo subDir in subDirs)
            {
                string destSubDirPath = Path.Combine(destFolder, subDir.Name);
                SyncDirectories(subDir.FullName, destSubDirPath, logFile);
            }
        }
    }
}
