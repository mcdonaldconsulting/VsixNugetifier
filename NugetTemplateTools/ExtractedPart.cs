namespace NugetTemplateTools
{
    public class ExtractedPart
    {
        private readonly ExtractedPackage package;

        public ExtractedPart(PartWrapper part, string extractedFileName, ExtractedPackage package)
        {
            this.Part = part;
            this.ExtractedFileName = extractedFileName;
            this.package = package;
        }

        public PartWrapper Part { get; private set; }

        public string ExtractedFileName { get; private set; }

        public ExtractedPackage Package
        {
            get { return this.package; }
        }
    }
}
