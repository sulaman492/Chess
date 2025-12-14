using System.Configuration;
using System.Data;
using System.Windows;
using Chess.UI;

namespace Chess
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var win = new Window1();
            win.Show();
        }
    }

}
