using System.Windows;
using DbwViewer.ViewModels;

namespace DbwViewer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();
        }

        private async void Window_OnLoaded(object sender, RoutedEventArgs e)
        {
            await ((MainWindowViewModel)DataContext).LoadAreasAsync();
        }
    }
}