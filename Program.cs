using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace BackupLocalFiles_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            try { Console.Clear(); }
            catch { System.Console.WriteLine("console can't be cleared now"); }

            string sourceBackupPath = Path.Combine(Directory.GetCurrentDirectory(), "sourceCopy");
            string destinationBackupPath = Path.Combine(Directory.GetCurrentDirectory(), "destinationCopy");

            // string sourceBackupPath = $"D:{Path.AltDirectorySeparatorChar}DEVs";
            // string destinationBackupPath = $"E:{Path.AltDirectorySeparatorChar}Documents{Path.AltDirectorySeparatorChar}DEVs copy";

            System.Console.WriteLine(getFileNameFromPath(Path.Combine(sourceBackupPath, "longtext.txt")));

            if (Directory.Exists(sourceBackupPath))
            {
                if (Directory.Exists(destinationBackupPath))
                {
                    System.Console.WriteLine("destination directory exist");
                }
                else
                {
                    Directory.CreateDirectory(destinationBackupPath);
                    System.Console.WriteLine("destination directory not existed, but it was created");
                }

                CopyFilesBackup(sourceBackupPath, destinationBackupPath);
            }
            else
            {
                System.Console.WriteLine("souce path doesn't exist");
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
        //solution was found in: https://stackoverflow.com/posts/3822913
        private static void CopyFilesBackup(string sourcePath, string destinationPath)
        {
            //create directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Debug.Print(dirPath);
                System.Console.WriteLine($"Cloning dir: {dirPath}");
                Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
            {
                try
                {
                    System.Console.WriteLine($"Copy file: {newPath}");
                    File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);
                }
                catch (System.UnauthorizedAccessException)
                {
                    System.Console.WriteLine("System.UnauthorizedAccessException");
                }

            }
        }

        static string getFileNameFromPath(string path)
        {
            string[] pathElements = path.Split(Path.DirectorySeparatorChar);
            int pathLength = pathElements.Length;

            string fileName = pathElements[pathLength - 1];

            return fileName;
        }
    }
}