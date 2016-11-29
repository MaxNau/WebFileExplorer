using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebFileExplorer.Models
{
    public class FileExplorer
    {
        private const long SmallFileUpperBoundSize = 10 * 1024 * 1024;
        private const long MediumFileUpperBoundSize = 50 * 1024 * 1024;
        private const long LargeFileUpperBoundSize = 100 * 1024 * 1024;

        public List<Volume> Volumes { get; set; }
        public List<Folder> Folders { get; set; }
        public List<File> Files { get; set; }
        public string CurrentPath { get; set; }
        public string AccessDenied { get; set; }

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
            SmallFilesInCurFolder = MediumFilesInCurFolder = LargeFilesInCurFolder = 0;
            foreach (File file in Files)
            {
                if (IsSmallFile(file.Size))
                    SmallFilesInCurFolder++;
                else if (IsMediumFile(file.Size))
                    MediumFilesInCurFolder++;
                else if (IsLargeFile(file.Size))
                    LargeFilesInCurFolder++;
            }
        }

        /*
        * Calculates files in all subfolers of current folder depending on file size using pattern:
        *      small file with size <= 10mb;
        *      medium file with size > 10mb AND <= 50mb;
        *      large file with size >= 100mb.
        */
        public void CalculateFilesInSubFolders(string path)
        {
            IEnumerable<string> folders;

            try
            {
                folders = Directory.EnumerateDirectories(path);
                foreach (string directory in folders)
                {
                    // Process each directory recursively
                    CalculateFilesInSubFolders(directory);
                }
            }
            catch { }
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] files = di.EnumerateFiles().ToArray();
            foreach (FileInfo file in files)
            {
                if (IsSmallFile(file.Length))
                    SmallFilesInSubFolders++;
                else if (IsMediumFile(file.Length))
                    MediumFilesInSubFolders++;
                else if (IsLargeFile(file.Length))
                    LargeFilesInSubFolders++;
            }
        }

        // Check if file is small
        private bool IsSmallFile(long fileSize)
        {
            return fileSize <= SmallFileUpperBoundSize;
        }

        // Check if file is medium
        private bool IsMediumFile(long fileSize)
        {
            return fileSize > SmallFileUpperBoundSize && fileSize <= MediumFileUpperBoundSize;
        }

        // Check if file is large
        private bool IsLargeFile(long fileSize)
        {
            return fileSize > LargeFileUpperBoundSize;
        }

        // Gets all volumes on the hard drive. Returns current instance of File Explorer
        public FileExplorer GetVolumes()
        {
            AccessDenied = null;
            Volumes = (from drive in DriveInfo.GetDrives()
                       select new Volume(drive.Name, drive.Name)).ToList();

            return this;
        }

        // Gets all folders and files, except that which require system access rights, in current directory
        public void GetCurrentDirectory()
        {
            AccessDenied = null;
            DirectoryInfo directoryInfo = new DirectoryInfo(CurrentPath);
            try
            {
                GetFolders(directoryInfo);
                GetFiles(directoryInfo);

                CalculateFilesInCurFolder();

                SmallFilesInSubFolders = MediumFilesInSubFolders = LargeFilesInSubFolders = 0;
                CalculateFilesInSubFolders(CurrentPath);
            }
            catch (UnauthorizedAccessException ex)
            {
                AccessDenied = ex.Message;
                Folders.Add(new Folder("..", directoryInfo.Parent?.FullName, ""));
                Files.Clear();
            }
            catch (IOException ex)
            {
                Folders.Add(new Folder("..", directoryInfo.Parent?.FullName, ""));
                AccessDenied = ex.Message;
            }
        }

        // Gets all folders, except that which require system access rights, in current directory
        private void GetFolders(DirectoryInfo directoryInfo)
        {
            Folders.Clear();
            
            Folders = (from directory in directoryInfo.GetDirectories()
                           select new Folder(directory.Name, directory.FullName)).ToList();

            Folders.Insert(0, new Folder("..", directoryInfo.Parent?.FullName));

        }

        // Gets all files, except that which require system access rights, in current directory
        private void GetFiles(DirectoryInfo directoryInfo)
        {
            try
            {
                Files = (from file in directoryInfo.GetFiles()
                         select new File(file.Name, file.FullName, file.Extension) { Size = file.Length }).ToList();
            }
            catch (UnauthorizedAccessException ex){}
        }
    }
}