namespace VsixNugetifierLib
{
    public class Manifest
    {
        public Manifest(string packageId, string projectName)
        {
            this.PackageId = packageId;
            this.ProjectName = projectName;
        }

        public string PackageId { get; private set; }

        public string ProjectName { get; private set; }
    }
}
