
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AisUriProviderApi;

namespace Task2
{
    public class FileSynchronizer
    {
        private readonly string _localFilePath = Path.Combine(Environment.CurrentDirectory, "Files");
        public AisUriProvider _aisUriProviderApi = new AisUriProvider();
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(3, 3);
        private readonly Timer _timer;
        public List<string> _currentFiles = new List<string>();

        public FileSynchronizer()
        {
            LoadLocalFiles();
            _timer = new Timer(SynchronizeFiles, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
        }

        public void SynchronizeFiles(object state = null)
        {
            var uris = _aisUriProviderApi.Get();
            var tasks = new List<Task>();

            foreach (var uri in uris)
            {
                _semaphoreSlim.Wait();

                if (!_currentFiles.Contains(uri.ToString()))
                {
                    tasks.Add(DownloadFileAsync(uri));
                }
                else
                {
                    _semaphoreSlim.Release();
                }
            }
            Task.WhenAll(tasks);
            DeleteOldFiles();
            Console.WriteLine("File synchronization completed");
        }

        public async Task DownloadFileAsync(Uri uri)
        {
            try
            {
                using (var client = new System.Net.WebClient())
                {
                    var fileName = Path.GetFileName(uri.LocalPath);
                    var filePath = Path.Combine(_localFilePath, fileName);
                    await client.DownloadFileTaskAsync(uri, filePath);
                    _currentFiles.Add(fileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading file {uri}: {ex.Message}");
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        private void DeleteOldFiles()
        {
            var filesToDelete = _currentFiles.Skip(10).ToList();

            foreach (var file in filesToDelete)
            {
                var filePath = Path.Combine(_localFilePath, file);
                File.Delete(filePath);
                _currentFiles.Remove(file);
            }
        }

        private void LoadLocalFiles()
        {
            if (!Directory.Exists(_localFilePath))
            {
                Directory.CreateDirectory(_localFilePath);
            }
            _currentFiles = Directory.GetFiles(_localFilePath).ToList();
        }

    }

}
