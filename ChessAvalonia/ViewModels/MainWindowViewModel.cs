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


    public void NewBoardState(List<string> boardRepresentation)
    {
        BoardSquares.Clear();

        if (boardRepresentation.Count == 0)
        {
            boardRepresentation = new List<string>
            {
                "black-Rook", "black-Knight", "black-Bishop", "black-Queen", "black-Kings", "black-Bishop", "black-Knight", "black-Rook",
                "black-Pawn", "black-Pawn", "black-Pawn", "black-Pawn", "black-Pawn", "black-Pawn", "black-Pawn", "black-Pawn",
                "", "", "", "", "", "", "", "",
                "", "", "", "", "", "", "", "",
                "", "", "", "", "", "", "", "",
                "", "", "", "", "", "", "", "",
                "white-Pawn", "white-Pawn", "white-Pawn", "white-Pawn", "white-Pawn", "white-Pawn", "white-Pawn", "white-Pawn",
                "white-Rook", "white-Knight", "white-Bishop", "white-Queen", "white-Kings", "white-Bishop", "white-Knight", "white-Rook"
            };
        }
        
        
        foreach (var row in Enumerable.Range(0, 8).ToList())
        {
            foreach (var col in Enumerable.Range(0, 8).ToList())
            {
                var peiceColor = "Transparent";
                var peiceImage = "None";
                var peice = boardRepresentation[(row * 8) + col];
                if (!String.IsNullOrEmpty(peice))
                {
                    var peiceList = peice.Split("-");
                    peiceColor = peiceList[0];
                    peiceImage = peiceList[1];
                }

                if ((col + row)% 2 == 0)
                {
                        BoardSquares.Add(new BoardSquareViewModel(col, row, "silver", peiceColor, peiceImage));
                }
                else
                {
                        BoardSquares.Add(new BoardSquareViewModel(col, row,"brown",  peiceColor, peiceImage));
                }
            }
        }
        ;
        
    }
    
    public MainWindowViewModel()
    {
        NewBoardState(new List<string>());
    }
}