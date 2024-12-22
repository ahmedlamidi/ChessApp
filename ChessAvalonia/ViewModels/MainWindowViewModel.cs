using System.Windows.Input;
using ChessAvalonia.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;
using ReactiveUI;

namespace ChessAvalonia.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<BoardSquareViewModel> BoardSquares { get; } = new();
    
    
    public MainWindowViewModel()
    {
        foreach (var col in Enumerable.Range(0, 8).ToList())
        {
            foreach (var row in Enumerable.Range(0, 8).ToList())
            {
                if ((col + row)% 2 == 0)
                {
                    BoardSquares.Add(new BoardSquareViewModel(col, row, "black"));
                }
                else
                {
                    BoardSquares.Add(new BoardSquareViewModel(col, row, "white"));
                }
            }
        }
        ;

    }
}