using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Input;
using ChessAvalonia.ViewModels;

namespace ChessAvalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.DataContext = new MainWindowViewModel();
    }
    
}