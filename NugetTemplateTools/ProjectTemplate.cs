namespace NugetTemplateTools
{
    using System.Collections.Generic;
    using System.IO.Packaging;

    public class ProjectTemplate
    {
        private readonly string filePath;

        public ProjectTemplate(string templateFilePath)
        {
            this.filePath = templateFilePath;
        }

        public IEnumerable<string> Unpack(string targetPath)
        {
            using (var package = Package.Open(this.filePath))
            {
                var parts = package.GetParts();

                foreach (var part in parts)
                {
                    if (part.ContentType == "application/zip")
                    {
                        yield return part.Uri.ToString() + " (Zip)";
                    }
                    else
                    {
                        yield return part.Uri.ToString();
                    }
                }
            }
        }
    }
}
