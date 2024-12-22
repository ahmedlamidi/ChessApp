using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Input;

namespace ChessAvalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void InputElement_OnTapped(object? sender, TappedEventArgs e)
    {
       Debug.Print("InputElement_OnTapped");
    }
}