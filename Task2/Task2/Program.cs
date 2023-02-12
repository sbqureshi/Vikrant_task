
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AisUriProviderApi;
using Task2;

namespace Task2Application
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting file synchronization");

            var fileSynchronizer = new FileSynchronizer();
            fileSynchronizer.SynchronizeFiles();

            Console.ReadLine();
        }
    }

}