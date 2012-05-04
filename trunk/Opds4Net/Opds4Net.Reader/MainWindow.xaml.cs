using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using Opds4Net.Model;
using Opds4Net.Util;

namespace Opds4Net.Reader
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

        private void OnSourceBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var opdsLoader = new BackgroundWorker();
            opdsLoader.DoWork += OnOpdsLoaderDoWork;
            opdsLoader.RunWorkerCompleted += OnOpdsLoaderRunWorkerCompleted;
            opdsLoader.RunWorkerAsync(opdsSourceBox.SelectedItem);
        }

        private void OnItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var opdsLoader = new BackgroundWorker();
            opdsLoader.DoWork += OnOpdsLoaderDoWork;
            opdsLoader.RunWorkerCompleted += OnOpdsLoaderRunWorkerCompleted;
            var link = (itemBox.SelectedItem as OpdsItem).Links.GetLinkValue("alternate", "application/atom+xml");
            if (link == null && (itemBox.SelectedItem as OpdsItem).Links.Count > 0)
            {
                link = (itemBox.SelectedItem as OpdsItem).Links[0].Uri.ToString();
            }
            opdsLoader.RunWorkerAsync(link);
        }

        private void OnOpdsLoaderRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                itemBox.ItemsSource = e.Result as IEnumerable;
            }
        }

        private void OnOpdsLoaderDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                e.Result = OpdsFeed.Load(new XmlTextReader(e.Argument as string)).Items;
            }
            catch (XmlException)
            {
                e.Result = new[] { OpdsItem.Load(new XmlTextReader(e.Argument as string)) };
            }
        }
    }
}
