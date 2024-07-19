using ColorPicker.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ColorPicker.View;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainWindowViewModel vm = new MainWindowViewModel();
        this.DataContext = vm;
    }

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    void granualScrollBar_Scroll(object sender, ScrollEventArgs e)
    {
        //HistoryView.ScrollToHorizontalOffset(Math.Round(e.NewValue / 80) * 80);
        ColorsHistory.ScrollIntoView(Math.Round(e.NewValue / 80) * 80);
    }
}