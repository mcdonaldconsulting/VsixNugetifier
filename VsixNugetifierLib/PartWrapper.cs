namespace VsixNugetifierLib
{
    using System.IO;
    using Ionic.Zip;

    public class PartWrapper
    {
        private readonly ZipEntry part;
        private readonly string uri;

        public PartWrapper(ZipEntry part)
        {
            this.part = part;
            this.uri = part.FileName;
        }

        public string ExtractedFileName { get; set; }

        public virtual bool IsZip
        {
            get { return false; }
        }

        public string UriString
        {
            get { return this.uri.ToString(); }
        }

        public virtual string UriDescription
        {
            get { return this.UriString; }
        }

        public static PartWrapper Create(ZipEntry part)
        {
            if (part.IsZip())
            {
                return new ZipPartWrapper(part);
            }

            return new PartWrapper(part);
        }

        public ExtractedPart Extract(string targetPath, string targetBasePath)
        {
            var outFileName = Path.Combine(targetPath, this.UriString);

            this.part.Extract(targetPath);

            var extractedPackage = this.part.IsZip()
                ? PackageWrapper.Extract(outFileName, targetBasePath)
                : null;

            return new ExtractedPart(this, outFileName, extractedPackage);
        }
    }
}
