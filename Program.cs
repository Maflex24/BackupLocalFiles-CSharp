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

            string sourcePath = Path.Combine(Directory.GetCurrentDirectory(), "sourceCopy");
            string destinationPath = Path.Combine(Directory.GetCurrentDirectory(), "destinationCopy");

            // string sourcePath = $"D:{Path.AltDirectorySeparatorChar}DEVs";
            // string destinationPath = $"E:{Path.AltDirectorySeparatorChar}Documents{Path.AltDirectorySeparatorChar}DEVs copy";


            if (Directory.Exists(sourcePath))
            {
                if (Directory.Exists(destinationPath))
                {
                    System.Console.WriteLine("destination directory exist");
                }
                else
                {
                    Directory.CreateDirectory(destinationPath);
                    System.Console.WriteLine("destination directory not existed, but it was created");
                }

                CopyFilesBackup(sourcePath, destinationPath);
            }
            else
            {
                System.Console.WriteLine("souce path doesn't exist");
            }

            // Console.WriteLine("Press any key to exit.");
            // Console.ReadKey();
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
            foreach (string newSourcePath in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
            {
                try
                {
                    DateTime sourceFileEditedTime = File.GetLastWriteTimeUtc(newSourcePath);
                    DateTime backupFileEditedTime = File.GetLastWriteTimeUtc(newSourcePath.Replace(sourcePath, destinationPath));
                    // System.Console.WriteLine(sourceFileEditedTime);
                    // System.Console.WriteLine(backupFileEditedTime);

                    if (sourceFileEditedTime > backupFileEditedTime)
                    {
                        System.Console.WriteLine($"Copy file: {newSourcePath}");
                        File.Copy(newSourcePath, newSourcePath.Replace(sourcePath, destinationPath), true);
                    }
                    else
                    {
                        System.Console.WriteLine($"File {newSourcePath} is up to date in copy directory");
                    }
                }
                catch (System.UnauthorizedAccessException)
                {
                    System.Console.WriteLine("System.UnauthorizedAccessException");
                }

            }
        }

        static string getFileOrDirNameFromPath(string path)
        {
            string[] pathElements = path.Split(Path.DirectorySeparatorChar);
            int pathLength = pathElements.Length;

            string fileName = pathElements[pathLength - 1];

            return fileName;
        }
    }
}