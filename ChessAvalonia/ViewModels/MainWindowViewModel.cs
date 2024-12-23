using System.Windows.Input;
using ChessAvalonia.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData.Binding;
using ReactiveUI;

namespace ChessAvalonia.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<BoardSquareViewModel> BoardSquares { get; set; } = new();
    


    public void NewBoardState(List<string> boardRepresentation)
    {

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
                        BoardSquares.Add(new BoardSquareViewModel(col, row, "silver", peiceColor, peiceImage, ShowNext));
                }
                else
                {
                        BoardSquares.Add(new BoardSquareViewModel(col, row,"brown",  peiceColor, peiceImage, ShowNext));
                }
            }
        }
        ;
        
    }

    public void ShowNext()
    {
       BoardSquares[1].Color = "green";
    }
    
    public MainWindowViewModel()
    {
        NewBoardState(new List<string>());
        
        
    }
}