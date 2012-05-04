using System.Windows;
using Opds4Net.Util;

namespace Opds4Net.Reader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            WebRequestHelper.SetAllowUnsafeHeaderParsing();
        }
    }
}
