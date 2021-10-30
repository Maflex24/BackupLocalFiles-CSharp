using System;
using System.IO;
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

                string[] files = Directory.GetFiles(sourceBackupPath);

                foreach (string file in files)
                {
                    string sourceFileName = Path.GetFileName(file);
                    string destinationFilePathAndName = Path.Combine(destinationBackupPath, sourceFileName);
                    File.Copy(file, destinationFilePathAndName, true);
                }
            }
            else
            {
                System.Console.WriteLine("souce path doesn't exist");
            }
        }
    }
}