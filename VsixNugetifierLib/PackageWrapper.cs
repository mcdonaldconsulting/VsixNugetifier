namespace VsixNugetifierLib
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Ionic.Zip;

    public class PackageWrapper : IDisposable
    {
        private readonly ZipFile package;
        private readonly string filePath;

        private bool disposed;

        protected PackageWrapper(string filePath, ZipFile package)
        {
            this.filePath = filePath;
            this.package = package;
        }

        public IEnumerable<PartWrapper> Parts
        {
            get { return this.package.Select(part => PartWrapper.Create(part)); }
        }

        public static PackageWrapper CreateNew(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            return new PackageWrapper(fileName, new ZipFile(fileName));
        }

        public static ExtractedPackage Extract(string fileName, string targetBasePath)
        {
            using (var package = Open(fileName))
            {
                return package.Extract(targetBasePath);
            }
        }

        public ExtractedPackage Extract(string targetBasePath)
        {
            var targetPath = Path.Combine(targetBasePath, Guid.NewGuid().ToString());

            var extractedParts = this.Parts.Select(p => p.Extract(targetPath, targetBasePath)).ToList();

            return new ExtractedPackage(targetPath, extractedParts);
        }

        public void AddDirectoryAndSave(string extractPath)
        {
            this.package.AddDirectory(extractPath);
            this.package.Save();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            try
            {
                if (disposing)
                {
                    this.package.Dispose();
                }
            }
            finally
            {
                this.disposed = true;
            }
        }

        private static PackageWrapper Open(string fileName)
        {
            return new PackageWrapper(fileName, ZipFile.Read(fileName));
        }
    }
}
