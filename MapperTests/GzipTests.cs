using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace MapperTests
{
    [TestClass]
    public class GzipTests
    {
        /*[TestMethod]
        public void CompressZip()
        {
            bool directoryExists = Directory.Exists("Maperitive");

            Assert.IsTrue(directoryExists);

            if (directoryExists)
            {
                CreateDirectoriesRelative("Maperitive/", "Gzipped/");
                string[] directories = Directory.GetDirectories("Maperitive", "*", SearchOption.AllDirectories);

                List<string> directoryList = new List<string> { "Maperitive" };
                directoryList.AddRange(directories);

                foreach (string directory in directoryList)
                {
                    string[] files = Directory.GetFiles(directory);

                    foreach (string file in files)
                    {
                        string extension = Path.GetExtension(file);

                        if (extension != ".gz")
                        {
                            FileStream originalFile = File.OpenRead(file);
                            FileStream compressedFile = File.Create(file.Replace("Maperitive\\", "Gzipped\\") + ".gz");

                            GZipStream zipStream = new GZipStream(compressedFile, CompressionMode.Compress);

                            originalFile.CopyTo(zipStream);
                            originalFile.Close();
                            zipStream.Close();
                            originalFile.Close();
                        }
                    }
                }
            }
        }*/

        [TestMethod]
        public void AddExtension()
        {
            bool directoryExists = Directory.Exists("Maperitive");

            Assert.IsTrue(directoryExists);

            if (directoryExists)
            {
                CreateDirectoriesRelative("Maperitive/", "Extension/");
                string[] directories = Directory.GetDirectories("Maperitive", "*", SearchOption.AllDirectories);

                List<string> directoryList = new List<string> { "Maperitive" };
                directoryList.AddRange(directories);

                foreach (string directory in directoryList)
                {
                    string[] files = Directory.GetFiles(directory);

                    foreach (string file in files)
                    {
                        string extension = Path.GetExtension(file);

                        if (extension != ".mapper")
                        {
                            FileStream originalFile = File.OpenRead(file);
                            FileStream compressedFile = File.Create(file.Replace("Maperitive\\", "Extension\\") + ".mapper");

                            originalFile.CopyTo(compressedFile);
                            compressedFile.Close();
                            originalFile.Close();
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void RemoveExtension()
        {
            bool succeeded = Mapper.Decompression.GzipExtractor.RemoveAllChangedNames("Extension\\", "NoExtension\\", ".mapper");
            Assert.IsTrue(succeeded);

            byte[] originalFile = File.ReadAllBytes("Maperitive/Maperitive.exe");
            byte[] decompressedFile = File.ReadAllBytes("Unzipped/Maperitive.exe");

            bool equal = originalFile.Length == decompressedFile.Length;

            Assert.IsTrue(equal);

            if (equal)
            {
                for (int index = 0; index < originalFile.Length; ++index)
                {
                    Assert.AreEqual(originalFile[index], decompressedFile[index]);
                }
            }
        }

        /*[TestMethod]
        public void DecompressZip()
        {
            bool succeeded = Mapper.Decompression.GzipExtractor.ExtractZippedFolder("Gzipped\\", "Unzipped\\");
            Assert.IsTrue(succeeded);

            byte[] originalFile = File.ReadAllBytes("Maperitive/Maperitive.exe");
            byte[] decompressedFile = File.ReadAllBytes("Unzipped/Maperitive.exe");

            bool equal = originalFile.Length == decompressedFile.Length;

            Assert.IsTrue(equal);

            if (equal)
            {
                for (int index = 0; index < originalFile.Length; ++index)
                {
                    Assert.AreEqual(originalFile[index], decompressedFile[index]);
                }
            }
        }*/

        internal void CreateDirectoriesRelative(string sourcePath, string newPath)
        {
            foreach(string directory in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(directory.Replace(sourcePath, newPath));
            }
        }
    }
}
