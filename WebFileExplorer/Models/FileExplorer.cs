using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebFileExplorer.Models
{
    public class FileExplorer
    {
        public List<Volume> Volumes { get; set; }
        public List<Folder> Folders { get; set; }
        public List<File> Files { get; set; }
        public string CurrentPath { get; set; }

        public int SmallFilesInCurFolder { get; set; }
        public int MediumFilesInCurFolder { get; set; }
        public int LargeFilesInCurFolder { get; set; }
        public int SmallFilesInSubFolders { get; set; }
        public int MediumFilesInSubFolders { get; set; }
        public int LargeFilesInSubFolders { get; set; }

        public FileExplorer()
        {
            Volumes = new List<Volume>();
            Folders = new List<Folder>();
            Files = new List<File>();
        }

        /*
         * Calculates files in current folder depending on file size using pattern:
         *      small file with size <= 10mb;
         *      medium file with size > 10mb AND <= 50mb;
         *      large file with size >= 100mb.
         */
        public void CalculateFilesInCurFolder()
        {

        }

        /*
        * Calculates files in all subfolers of current folder depending on file size using pattern:
        *      small file with size <= 10mb;
        *      medium file with size > 10mb AND <= 50mb;
        *      large file with size >= 100mb.
        */
        public void CalculateFilesInSubFolders()
        {

        }

        // Check if file is small
        private bool IsSmallFile(long fileSize)
        {
            return fileSize <= 10 * 1024;
        }

        // Check if file is medium
        private bool IsMediumFile(long fileSize)
        {
            return fileSize > 10 * 1024 && fileSize / 1024 <= 50 * 1024;
        }

        // Check if file is large
        private bool IsLargeFile(long fileSize)
        {
            return fileSize >= 100 * 1024;
        }

        // Gets all folders and files, except that which require system access rights, in current directory
        public void GetCurrentDirectory()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(CurrentPath);
            GetFolders(directoryInfo);
            GetFiles(directoryInfo);
        }

        // Gets all folders, except that which require system access rights, in current directory
        private void GetFolders(DirectoryInfo directoryInfo)
        {
            Folders.Add(new Folder("..", directoryInfo.Parent?.FullName));

            try
            {
                Folders = (from directory in directoryInfo.GetDirectories()
                           select new Folder(directory.Name, directory.FullName)).ToList();
            }
            catch (UnauthorizedAccessException ex){}
        }

        // Gets all files, except that which require system access rights, in current directory
        private void GetFiles(DirectoryInfo directoryInfo)
        {
            try
            {
                Files = (from file in directoryInfo.GetFiles()
                         select new File(file.Name, file.FullName)).ToList();
            }
            catch (UnauthorizedAccessException ex){}
        }

    }
}