using AisUriProviderApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Task2Application;
namespace Task2.Tests
{
    [TestClass]
    public class FileSynchronizerTests
    {
        private readonly string _localFilePath = Path.Combine(Environment.CurrentDirectory, "Files");
        private readonly string[] _testFiles = { "file1.txt", "file2.txt", "file3.txt", "file4.txt", "file5.txt", "file6.txt", "file7.txt", "file8.txt", "file9.txt", "file10.txt" };
        private FileSynchronizer _fileSynchronizer;

       
        [TestInitialize]
        public void TestInitialize()
        {

            try
            {
                foreach (var file in Directory.GetFiles(_localFilePath))
                {
                    File.Delete(file);

                }

            }
            catch (Exception ex)
            {

            }
            _fileSynchronizer = new FileSynchronizer();
        }


        [TestMethod]
        public void SynchronizeFiles_ShouldDownloadAllFiles()
        {
            _fileSynchronizer.SynchronizeFiles();

            var localFiles = Directory.GetFiles(_localFilePath);
            Assert.AreEqual(_testFiles.Length, localFiles.Length);
        }
    
    }
}
