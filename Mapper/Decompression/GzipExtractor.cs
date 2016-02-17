using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Mapper.Decompression
{
    public class GzipExtractor
    {
        public static bool RemoveAllChangedNames(string sourceFolder, string destinationFolder, string extensionToRemove)
        {
            bool succeeded = true;

            if (Directory.Exists(sourceFolder))
            {
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                ClearDestinationFilesAndDirectories(destinationFolder);
                CreateDirectoriesRelative(sourceFolder, destinationFolder);

                List<string> allFolders = new List<string> { sourceFolder };
                allFolders.AddRange(Directory.GetDirectories(sourceFolder, "*", SearchOption.AllDirectories));

                foreach (string folder in allFolders)
                {
                    foreach (string file in Directory.GetFiles(folder))
                    {
                        string extension = Path.GetExtension(file);
                        string originalFileName = Path.GetDirectoryName(file) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(file);

                        if (extension == extensionToRemove)
                        {
                            try
                            {
                                FileStream originalFile = File.OpenRead(file);
                                FileStream changedFile = File.Create(originalFileName.Replace(sourceFolder, destinationFolder));

                                byte[] buffer = new byte[originalFile.CanSeek ? Math.Min((int)(originalFile.Length - originalFile.Position), 8192) : 8192];
                                int count;
                                do
                                {
                                    count = originalFile.Read(buffer, 0, buffer.Length);
                                    changedFile.Write(buffer, 0, count);
                                }
                                while (count != 0);

                                originalFile.Close();
                                changedFile.Close();
                            }
                            catch (Exception ex)
                            {
                                CimTools.CimToolsHandler.CimToolBase.DetailedLogger.LogError("Failed on " + file);
                                CimTools.CimToolsHandler.CimToolBase.DetailedLogger.LogError(ex.Message + "\n\t" + ex.StackTrace);
                                succeeded = false;
                            }
                        }
                    }
                }
            }

            return succeeded;
        }

        public static bool ExtractZippedFolder(string sourceFolder, string destinationFolder)
        {
            bool succeeded = true;

            if (Directory.Exists(sourceFolder))
            {
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                ClearDestinationFilesAndDirectories(destinationFolder);
                CreateDirectoriesRelative(sourceFolder, destinationFolder);

                List<string> allFolders = new List<string> { sourceFolder };
                allFolders.AddRange(Directory.GetDirectories(sourceFolder, "*", SearchOption.AllDirectories));

                foreach(string folder in allFolders)
                {
                    foreach(string file in Directory.GetFiles(folder))
                    {
                        string extension = Path.GetExtension(file);
                        string originalFileName = Path.GetDirectoryName(file) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(file);

                        if (extension == ".gz")
                        {
                            try
                            {
                                FileStream originalFile = File.OpenRead(file);
                                FileStream decompressedFile = File.Create(originalFileName.Replace(sourceFolder, destinationFolder));

                                GZipStream zipStream = new GZipStream(originalFile, CompressionMode.Decompress);

                                byte[] buffer = new byte[zipStream.CanSeek ? Math.Min((int)(zipStream.Length - zipStream.Position), 8192) : 8192];
                                int count;
                                do
                                {
                                    count = zipStream.Read(buffer, 0, buffer.Length);
                                    decompressedFile.Write(buffer, 0, count);
                                }
                                while (count != 0);

                                decompressedFile.Close();
                                zipStream.Close();
                                originalFile.Close();
                            }
                            catch(Exception ex)
                            {
                                CimTools.CimToolsHandler.CimToolBase.DetailedLogger.LogError("Failed on " + file);
                                CimTools.CimToolsHandler.CimToolBase.DetailedLogger.LogError(ex.Message + "\n\t" + ex.StackTrace);
                                succeeded = false;
                            }
                        }
                    }
                }
            }

            return succeeded;
        }

        internal static void ClearDestinationFilesAndDirectories(string path, bool topLevel = true)
        {
            if(Directory.Exists(path))
            {
                foreach(string directory in Directory.GetDirectories(path, "*", SearchOption.AllDirectories))
                {
                    ClearDestinationFilesAndDirectories(directory, false);
                }

                foreach(string file in Directory.GetFiles(path))
                {
                    File.Delete(file);
                }

                if (!topLevel)
                {
                    Directory.Delete(path);
                }
            }
        }

        internal static void CreateDirectoriesRelative(string sourcePath, string newPath)
        {
            foreach (string directory in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(directory.Replace(sourcePath, newPath));
            }
        }
    }
}
