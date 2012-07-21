namespace VsixNugetifierLib
{
    using System.IO;

    public static class MiscellaneousExtensions
    {
        public static void CopyToFile(this Stream partStream, string targetFileName)
        {
            EnsureDirectoryExists(targetFileName);

            using (var outFileStream = new FileStream(targetFileName, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                partStream.CopyTo(outFileStream);
            }
        }

        private static void EnsureDirectoryExists(string fileName)
        {
            var folder = Path.GetDirectoryName(fileName);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }
    }
}
