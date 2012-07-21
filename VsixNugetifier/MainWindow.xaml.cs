namespace VsixNugetifier
{
    using System.IO;
    using System.Windows;
    using NugetTemplateTools;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //string vsixFileName = @"C:\Users\Patrick\Documents\Visual Studio 2010\My Exported Templates\WebApiConsole.vsix";
            //string solutionPath = @"C:\Work\WebApiConsole";

            string vsixFileName = @"C:\Users\Patrick\Documents\Visual Studio 2010\My Exported Templates\MvcTwitterBootstrap.vsix";
            string solutionPath = @"C:\Work\MvcBootstrap";

            var appTempPath = Path.Combine(Path.GetTempPath(), "NugetTemplate");

            var extractedPackage = ExtractPackage(vsixFileName, appTempPath);

            DispayExtractResults(extractedPackage);

            var packagePath = Path.Combine(solutionPath, "packages");

            extractedPackage.UpdatePackages(packagePath);

            extractedPackage.UpdateContentTypes();

            var packageId = extractedPackage.UpdateManifest(packagePath);

            extractedPackage.RepackageTemplates(packageId, solutionPath);

            // Recreate the vsix
            var newVsixFileName = Path.Combine(appTempPath, Path.GetFileName(vsixFileName));
            var newPackage = PackageWrapper.Create(newVsixFileName);
            newPackage.AddDirectoryAndSave(extractedPackage.ExtractPath);
        }

        private static ExtractedPackage ExtractPackage(string path, string appTempPath)
        {
            ExtractedPackage extractedPackage;
            using (var vsixPackage = PackageWrapper.Open(path))
            {
                extractedPackage = vsixPackage.Extract(appTempPath);
            }
            return extractedPackage;
        }

        private void DispayExtractResults(ExtractedPackage extractedPackage)
        {
            foreach (var part in extractedPackage.ExtractedParts)
            {
                textBox1.Text += part.ExtractedFileName + "\r\n";
                textBox1.Text += part.Part.UriDescription + "\r\n";

                if (part.Package != null)
                {
                    foreach (var zipPart in part.Package.ExtractedParts)
                    {
                        textBox1.Text += "    " + zipPart.ExtractedFileName + "\r\n";
                    }
                }
            }
        }
    }
}
