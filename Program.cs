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
            // path for files and directories, which was deleted in source, propably doesn't need anymore, but for safety reason and to keep order, ther are move to this path:
            string deletedFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "BackupLocalFiles-DeleteFiles");

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
                checkingDeletedFiles(sourcePath, destinationPath, deletedFilesPath);
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
                // checking is directory exist in backup copy, if not, creating the directory
                if (!Directory.Exists(dirPath.Replace(sourcePath, destinationPath)))
                {
                    System.Console.WriteLine($"Cloning dir: {dirPath}");
                    Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));
                }

            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newSourcePath in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
            {
                try
                {
                    DateTime sourceFileEditedTime = File.GetLastWriteTimeUtc(newSourcePath);
                    DateTime backupFileEditedTime = File.GetLastWriteTimeUtc(newSourcePath.Replace(sourcePath, destinationPath));

                    if (sourceFileEditedTime > backupFileEditedTime)
                    {
                        System.Console.WriteLine($"Copy file: {newSourcePath}");
                        File.Copy(newSourcePath, newSourcePath.Replace(sourcePath, destinationPath), true);
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

        // That function check files exist in backup, but not in source, so, it was deleted. 
        private static void checkingDeletedFiles(string sourcePath, string backupPath, string deletedFilesPath)
        {
            if (!Directory.Exists(deletedFilesPath))
            {
                Directory.CreateDirectory(deletedFilesPath);
                System.Console.WriteLine("Created directory for deleted files");
            }

            foreach (string directory in Directory.GetDirectories(backupPath, "*", SearchOption.AllDirectories))
            {
                string cutPath = directory.Replace(backupPath, "");
                string newPath = Path.Combine(backupPath, deletedFilesPath);
                newPath += cutPath;
                Directory.CreateDirectory(newPath);
            }
            foreach (string file in Directory.GetFiles(backupPath, "*", SearchOption.AllDirectories))
            {
                if (!File.Exists(file.Replace(backupPath, sourcePath)))
                {
                    string cutPath = file.Replace(backupPath, "");
                    string newPath = Path.Combine(backupPath, deletedFilesPath);
                    newPath += cutPath;

                    File.Move(file, newPath, true);
                    System.Console.WriteLine($"{file} does not exist in source. It was moved to the deleted directory");
                }
            }

        }
    }
}