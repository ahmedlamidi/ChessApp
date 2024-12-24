using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using ChessAvalonia.Views;
using DynamicData.Binding;
using ReactiveUI;
namespace ChessAvalonia.ViewModels;

public class BoardSquareViewModel:ReactiveObject
{
    public int Row { get; set; }
    public int Col { get; set; }

    private string _color;
    private Object? _piece;
    private string _pieceColor;
    
    public string? PieceType {get; set;}
    
    public string Color
    {
        get => _color;
        set
        {
            this.RaiseAndSetIfChanged(ref _color, value);
        }
    }

    public string PieceColor { 
        get => _pieceColor;
        set
        {
            this.RaiseAndSetIfChanged(ref _pieceColor, value);
        }
    }
    public object? Piece { 
        get => _piece;
        set {
            this.RaiseAndSetIfChanged(ref _piece, value);
        }
        
    }
    
    public bool CanMove { get; set; }
    

    public BoardSquareViewModel(int row, int col, string color, string peice_color, string piece, bool can_domove)
    {
        Row = row;
        Col = col; 
        _color = color;
        _pieceColor = peice_color;
        CanMove = can_domove;
        try
        {
            if (Application.Current != null) _piece = Application.Current.FindResource(piece);
            PieceType = piece;
        }
        catch
        {
            Piece = null;
            Debug.Print($"Could not find resource {_piece}");
            PieceType = null;
        }
        
    }
    
    
}