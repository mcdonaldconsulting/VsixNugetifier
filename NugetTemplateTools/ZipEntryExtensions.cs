namespace NugetTemplateTools
{
    using System.IO;
    using Ionic.Zip;

    public static class ZipEntryExtensions
    {
        public static bool IsZip(this ZipEntry zipEntry)
        {
            return Path.GetExtension(zipEntry.FileName) == ".zip";
        }
    }
}
