namespace NugetTemplateTools
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Ionic.Zip;

    public class PackageWrapper : IDisposable
    {
        private readonly ZipFile package;

        private bool disposed;

        protected PackageWrapper(string filePath, ZipFile package)
        {
            this.FilePath = filePath;
            this.package = package;
        }

        public string FilePath { get; private set; }

        public IEnumerable<PartWrapper> Parts
        {
            get
            {
                return from part in this.package
                       select PartWrapper.Create(part);
            }
        }

        public static PackageWrapper Create(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            return new PackageWrapper(fileName, new ZipFile(fileName));
        }

        public static PackageWrapper Open(string fileName)
        {
            return new PackageWrapper(fileName, ZipFile.Read(fileName));
        }

        public static ExtractedPackage Extract(string fileName, string targetBasePath)
        {
            using (var zip = Open(fileName))
            {
                return zip.Extract(targetBasePath);
            }
        }

        public ExtractedPackage Extract(string targetBasePath)
        {
            var targetPath = Path.Combine(targetBasePath, Guid.NewGuid().ToString());

            var extractedParts = this.Parts.Select(p => p.Extract(targetPath, targetBasePath)).ToList();

            return new ExtractedPackage(targetPath, extractedParts);
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

        public void AddDirectoryAndSave(string extractPath)
        {
            this.package.AddDirectory(extractPath);
            this.package.Save();
        }
    }
}
