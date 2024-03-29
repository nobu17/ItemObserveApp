﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace ItemObserveApp.Util
{
    public static class FileExtensions
    {
        private static readonly string LocalFolder;

        static FileExtensions()
        {
            // Gets the target platform's valid save location
            LocalFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }

        // Byte[] extension methods

        public static async Task<string> SaveToLocalFolderAsync(this byte[] dataBytes, string fileName)
        {
            return await Task.Run(() =>
            {
                if (!Directory.Exists(LocalFolder))
                {
                    Directory.CreateDirectory(LocalFolder);
                }

                // Use Combine so that the correct file path slashes are used
                var filePath = Path.Combine(LocalFolder, fileName);

                if (File.Exists(filePath))
                    File.Delete(filePath);

                File.WriteAllBytes(filePath, dataBytes);

                return filePath;
            });
        }

        public static async Task<byte[]> LoadFileBytesAsync(string filePath)
        {
            return await Task.Run(() => File.ReadAllBytes(filePath));
        }

        // Stream extension methods

        public static async Task<string> SaveToLocalFolderAsync(this Stream dataStream, string fileName)
        {
            if (!Directory.Exists(LocalFolder))
            {
                Directory.CreateDirectory(LocalFolder);
            }

            // Use Combine so that the correct file path slashes are used
            var filePath = Path.Combine(LocalFolder, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

            using (var fileStream = File.OpenWrite(filePath))
            {
                if (dataStream.CanSeek)
                    dataStream.Position = 0;

                await dataStream.CopyToAsync(fileStream);

                return filePath;
            }
        }

        public static async Task<Stream> LoadFileStreamAsync(string filePath)
        {
            return await Task.Run(() =>
            {
                if (!Directory.Exists(LocalFolder))
                {
                    Directory.CreateDirectory(LocalFolder);
                }

                using (var fileStream = File.OpenRead(filePath))
                {
                    return fileStream;
                }
            });
        }

        // string to text
        public static async Task<string> SaveToLocalFolderAsync(this string text, string fileName)
        {
            return await Task.Run(() =>
            {
                if (!Directory.Exists(LocalFolder))
                {
                    Directory.CreateDirectory(LocalFolder);
                }

                // Use Combine so that the correct file path slashes are used
                var filePath = Path.Combine(LocalFolder, fileName);

                if (File.Exists(filePath))
                    File.Delete(filePath);

                File.WriteAllText(filePath, text);

                return filePath;
            });
        }

        public static async Task<string> LoadFileStringAsync(string fileName)
        {
            var filePath = Path.Combine(LocalFolder, fileName);
            return await Task.Run(() => File.ReadAllText(filePath));
        }

    }
}
