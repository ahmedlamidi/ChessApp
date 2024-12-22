namespace ChessAvalonia.Models;

public class BoardSquares
{
 
        public int row { get; set; }
        public int col { get; set; }
        public string color { get; set; }

        public BoardSquares(int _row, int _col, string _color)
        {
            row = _row;
            col = _col; 
            color = _color;
        
        }
        
}