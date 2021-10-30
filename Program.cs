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
            Console.Clear();
            string sourceBackupPath = Path.Combine(Directory.GetCurrentDirectory(), "sourceCopy");
            string destinationBackupPath = Path.Combine(Directory.GetCurrentDirectory(), "destinationCopy");

            // string sourceBackupPath = $"F:{Path.AltDirectorySeparatorChar}Photo";
            // string destinationBackupPath = $"F:{Path.AltDirectorySeparatorChar}PhotoCopyOne";

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
        }
        //solution was found in: https://stackoverflow.com/posts/3822913
        private static void CopyFilesBackup(string sourcePath, string destinationPath)
        {
            //create directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);
            }
        }
    }
}