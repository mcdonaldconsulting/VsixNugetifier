namespace VsixNugetifier
{
    using System;
    using System.IO;
    using System.Windows;
    using Microsoft.Win32;
    using VsixNugetifierLib;

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
            try
            {
                string vsixFileName = txtFileName.Text;
                string solutionPath = Path.GetDirectoryName(txtSolution.Text);

                var appTempPath = Path.Combine(Path.GetTempPath(), "VsixNugetifier");

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Title);
            }
        }

        private static ExtractedPackage ExtractPackage(string path, string appTempPath)
        {
            using (var vsixPackage = PackageWrapper.Open(path))
            {
                return vsixPackage.Extract(appTempPath);
            }
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

        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "VSIX File|*.vsix|All Files (*.*)|*.*";
            dialog.FileName = string.Empty;
            dialog.InitialDirectory = @"C:\Users\Patrick\Documents\Visual Studio 2010\My Exported Templates";

            if ((dialog.ShowDialog(this) ?? false))
            {
                txtFileName.Text = dialog.FileName;
            }
        }

        private void btnChooseSolution_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Solution File|*.sln|All Files (*.*)|*.*";
            dialog.FileName = string.Empty;
            dialog.InitialDirectory = @"C:\Work";

            if ((dialog.ShowDialog(this) ?? false))
            {
                txtSolution.Text = dialog.FileName;
            }
        }

        private void txtFileName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            button1.IsEnabled =
                !string.IsNullOrWhiteSpace(txtFileName.Text)
                && !string.IsNullOrWhiteSpace(txtSolution.Text);
        }
    }
}
