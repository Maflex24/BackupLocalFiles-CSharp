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

                System.Collections.Generic.IEnumerable<string> allFilesInAllFolders = Directory.EnumerateFiles(sourceBackupPath, "*", SearchOption.AllDirectories);
                System.Console.WriteLine("All files in source backup folders:");
                foreach (var file in allFilesInAllFolders)
                {
                    System.Console.WriteLine(file);
                    System.Console.WriteLine(Directory.GetCurrentDirectory());
                    System.Console.WriteLine("");

                    string sourceFileName = Path.GetFileName(file);
                    string destinationFilePathAndName = Path.Combine(destinationBackupPath, sourceFileName);
                    File.Copy(file, destinationFilePathAndName, true);
                }

                // string[] files = Directory.GetFiles(sourceBackupPath);

                // foreach (string file in files)
                // {
                //     string sourceFileName = Path.GetFileName(file);
                //     string destinationFilePathAndName = Path.Combine(destinationBackupPath, sourceFileName);
                //     File.Copy(file, destinationFilePathAndName, true);
                // }
            }
            else
            {
                System.Console.WriteLine("souce path doesn't exist");
            }
        }
    }
}