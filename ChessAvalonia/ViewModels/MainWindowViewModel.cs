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
using ChessAvalonia.Models;
using ChessAvalonia.ViewModels;
using DynamicData.Binding;
using ReactiveUI;

namespace ChessAvalonia.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<BoardSquareViewModel> BoardSquares { get; set; } = new();
    
    private BoardSquareViewModel? _selectedBoardSquare;

    private BoardModel _selectedBoard;
    
    public BoardSquareViewModel? PreviousBoardSquare { get; set; }
    
    public List<int> ChangedSquares { get; set; } = new();
    
    public bool MoveMade { get; set; } = true;
    
    public List<int> noCapture = new List<int>();
    public List<int> capture = new List<int>();
    public BoardSquareViewModel? SelectedSquare
    {
        get => _selectedBoardSquare;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedBoardSquare, value);
            // we need to keep the last selected square
            // if the selected sqaure is green then we move to that square from another square
            
            if (String.Equals(SelectedSquare.Color, "green") || String.Equals(SelectedSquare.Color, "red")){
            _selectedBoardSquare.Piece = PreviousBoardSquare.Piece;
            _selectedBoardSquare.PieceColor = PreviousBoardSquare.PieceColor;
            _selectedBoardSquare.Color = (_selectedBoardSquare.Row + _selectedBoardSquare.Col ) % 2 == 0 ? "silver" : "brown";
            _selectedBoard.BoardRepresentation[(_selectedBoardSquare.Row * 8) + _selectedBoardSquare.Col] =
                _selectedBoard.BoardRepresentation[(PreviousBoardSquare.Row * 8) + PreviousBoardSquare.Col];
            _selectedBoard.BoardRepresentation[(PreviousBoardSquare.Row * 8) + PreviousBoardSquare.Col] = "";
            PreviousBoardSquare.Piece = null;
            //     BoardSquares[(_selectedBoardSquare.Row * 8) + _selectedBoardSquare.Col].Piece = previousBoardSquare.Piece;
            }
            // if we select a square we need to clear the rest of them 
            {
                foreach (var move  in capture)
                {
                    BoardSquares[move].Color = (BoardSquares[move].Col + BoardSquares[move].Row)% 2 == 0 ? "silver" : "brown";
                }
                capture.Clear();
                
                foreach (var move  in noCapture)
                {
                    BoardSquares[move].Color = (BoardSquares[move].Col + BoardSquares[move].Row) % 2 == 0 ? "silver" : "brown";
                }
                noCapture.Clear();
                
                
            }
            // If we do a move we still want to 
            {
                var possibleMoves = _selectedBoard.CalcuateBoardRepresentation(value.Row, value.Col);

                if (possibleMoves.Any())
                {
                    noCapture = possibleMoves[0];
                    capture = possibleMoves[1];

                    foreach (var move in noCapture)
                    {
                        Debug.WriteLine($"{move}");
                        BoardSquares[move].Color = "green";
                    }

                    foreach (var move in capture)
                    {
                        BoardSquares[move].Color = "red";
                    }
                }
            }


            PreviousBoardSquare = _selectedBoardSquare;
        }
        
    }
    
    public MainWindowViewModel()
    {

        _selectedBoard = new BoardModel(null);
        
        
        foreach (var row in Enumerable.Range(0, 8).ToList())
        {
            foreach (var col in Enumerable.Range(0, 8).ToList())
            {
                var peiceColor = "Transparent";
                var peiceImage = "None";
                var peice = _selectedBoard.BoardRepresentation[(row * 8) + col];
                if (!String.IsNullOrEmpty(peice))
                {
                    var peiceList = peice.Split("-");
                    peiceColor = peiceList[0];
                    peiceImage = peiceList[1];
                }

                if ((col + row)% 2 == 0)
                {
                        BoardSquares.Add(new BoardSquareViewModel(row, col, "silver", peiceColor, peiceImage , MoveMade));
                }
                else
                {
                        BoardSquares.Add(new BoardSquareViewModel(row, col, "brown",  peiceColor, peiceImage, MoveMade));
                }
            }
        } ;
        
        
    }

}