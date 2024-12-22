using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using ChessAvalonia.Views;

namespace ChessAvalonia.ViewModels;

public class BoardSquareViewModel:ViewModelBase
{
    public int Row { get; set; }
    public int Col { get; set; }
    public string Color { get; set; }
    public string PieceColor { get; set; }
    public Object Piece { get; set; }

    public BoardSquareViewModel(int _row, int _col, string _color, string _peice_color, string _piece)
    {
        Row = _row;
        Col = _col; 
        Color = _color;
        PieceColor = _peice_color;
        try
        {
            Piece = Application.Current.FindResource(_piece);
        }
        catch
        {
            Piece = null;
            Debug.Print($"Could not find resource {_piece}");
        }
        
    }
    
    
}