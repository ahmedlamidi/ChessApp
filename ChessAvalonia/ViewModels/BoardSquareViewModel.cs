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
    
    public string? PieceType {get; set;}

    private readonly Action action;
    
    public string Color
    {
        get => _color;
        set
        {
            this.RaiseAndSetIfChanged(ref _color, value);
        }
    }

    public string PieceColor { get; set; }
    public object? Piece { get; set; }
    
    public bool CanMove { get; set; }
    

    public ReactiveCommand<Unit, Unit> Domove { get; }
    
    private void ChangeColor()
    {
        if (CanMove)
        {
            Color = "Green";
            action.Invoke();
        }
        // Change the color or perform other logic
        
    }

    public BoardSquareViewModel(int _row, int _col, string color, string _peice_color, string _piece, Action _action, bool can_domove)
    {
        Row = _row;
        Col = _col; 
        _color = color;
        PieceColor = _peice_color;
        action = _action;
        Domove = ReactiveCommand.Create(ChangeColor);
        CanMove = can_domove;
        try
        {
            if (Application.Current != null) Piece = Application.Current.FindResource(_piece);
            PieceType = _piece;
        }
        catch
        {
            Piece = null;
            Debug.Print($"Could not find resource {_piece}");
            PieceType = null;
        }
        
    }
    
    
}