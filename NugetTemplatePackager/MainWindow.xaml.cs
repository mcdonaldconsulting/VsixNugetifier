using System.Windows;
using System;
using System.IO;

namespace NugetTemplatePackager
{
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
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var tempPath = Path.GetTempPath();

            string path = @"C:\Users\Patrick\Documents\Visual Studio 2010\My Exported Templates\WebApiConsole.vsix.zip";
            var x = new NugetTemplateTools.ProjectTemplate(path);

            var files = x.Unpack(@"C:\temp");

            foreach (var file in files)
            {
                textBox1.Text += file + "\r\n";
            }
        }
    }
}
