using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IrbAnalyser
{
    class Zip
    {
        /// <summary>
        /// UnZip the files to a temp folder and return the path to the folder
        /// </summary>
        /// <param name="zipFile"></param>
        /// <returns></returns>
        public static string UnZip(string zipFile)
        {
            string directory = Path.GetTempPath();
            directory = directory + "IRBreport\\";

            UnZip(zipFile, directory);

            return directory;
        }

        /// <summary>
        /// Unzip a file to a specific folder
        /// </summary>
        /// <param name="zipFile"></param>
        /// <param name="folderPath"></param>
        public static void UnZip(string zipFile, string folderPath)
        {
            //from : http://www.fluxbytes.com/csharp/unzipping-files-using-shell32-in-c/
            if (!File.Exists(zipFile))
                throw new FileNotFoundException();

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            Shell32.Shell objShell = new Shell32.Shell();
            Shell32.Folder destinationFolder = objShell.NameSpace(folderPath);
            Shell32.Folder sourceFile = objShell.NameSpace(zipFile);

            foreach (var file in sourceFile.Items())
            {
                destinationFolder.CopyHere(file, 4 | 16);
            }
        }

        public static void CleanUpFile(string filepath)
        {
            DirectoryInfo directory = new DirectoryInfo(filepath);
            foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }

    }
}
