using System.Windows;
using System.Windows.Input;

namespace AnalysisCountsSteps
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        ///обработчики события
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            MainWindowAction main = new MainWindowAction();
            main.Show();
            this.Close();
        }

        private void Button_Close(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
        private void Button_Minimize(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void GridMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }
    }
}
