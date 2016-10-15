using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using System.Windows.Threading;
using ZooBrowser.ViewModel;


namespace ZooBrowser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            Xceed.Wpf.Toolkit.MessageBox.Show(e.Exception.Message, "Exception");
            Messenger.Default.Send(new ExceptionOccured());
        }
    }
}
