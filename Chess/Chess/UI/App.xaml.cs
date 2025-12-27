using Chess.UI;
using System.Windows;

namespace Chess
{
    public partial class App : Application
    {
        // Add this Main method
        [STAThread]
        public static void Main()
        {
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var win = new Window1();
            win.Show();
        }
    }
}