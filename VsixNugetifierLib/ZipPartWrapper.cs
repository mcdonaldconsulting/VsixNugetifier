namespace VsixNugetifierLib
{
    using Ionic.Zip;

    public class ZipPartWrapper : PartWrapper
    {
        public ZipPartWrapper(ZipEntry part)
            : base(part)
        {
        }

        public override bool IsZip
        {
            get { return true; }
        }

        public override string UriDescription
        {
            get { return this.UriString + " (Zip)"; }
        }
    }
}
