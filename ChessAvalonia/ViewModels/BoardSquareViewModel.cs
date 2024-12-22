using Avalonia.Controls;
using ChessAvalonia.Views;

namespace ChessAvalonia.ViewModels;

public class BoardSquareViewModel:ViewModelBase
{
    public int row { get; set; }
    public int col { get; set; }
    public string color { get; set; }

    public BoardSquareViewModel(int _row, int _col, string _color)
    {
        row = _row;
        col = _col; 
        color = _color;
        
    }
    
    
}