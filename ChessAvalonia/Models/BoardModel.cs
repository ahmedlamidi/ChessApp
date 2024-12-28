using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ChessAvalonia.Models;

public class BoardModel
{
    
    public List<String> BoardRepresentation { get; set; }

    public Tuple<int, int> findNextMove()
    {
        BasicChessEngine engine = new BasicChessEngine(null);
        // starting it from nothing anc creating a new 
        engine.DepthSearch(-1, -1, 1);
        // this means I should start a move with black and try to maximize for black
        // with a depth of one
        return engine.move_to_play;
    }

    public List<int> RepresenttoEngine()
    {
        var represent = new List<int>();
        foreach (var move in BoardRepresentation)
        {
            
        }
        return represent;
    }
    public List<List<int>> CalcuateBoardRepresentation(int row, int column)
    {
        // I want to calculate all the moves to go to given a position and 
        // the first list is all the moves that are not a capture
        // the second list is all the moves that are a capture
        List<int> Moves = new List<int>();
        List<int> Captures = new List<int>();

        
        var square = BoardRepresentation[(row* 8) + column].Split("-");
        if (square.Length != 2)
        {
            return [Moves, Captures];
        }
        else
        {
            
            switch (square[1])
            {
                case "Pawn":
                    // for the pawn
                    // look at not moved logic later
                    // for now 
                    // we just do one more forward
                    // check if the diagonal is occupied by a piece of different color
                {
                    int direction = square[0] == "white" ? -1 : 1;
                    // if we are white then the direction for movement should be downward 
                    // else the direction should be upward
                    {
                        if (string.IsNullOrEmpty(BoardRepresentation[((row + direction) * 8) + column]))
                        {
                            Moves.Add(((row+ direction) * 8) + column);
                        }

                        else if (!String.Equals(BoardRepresentation[((row + direction) * 8) + column].Split("-")[0], square[0],
                                StringComparison.InvariantCultureIgnoreCase))
                            // check if the piece is not the same color as us
                            // if it is not then we can capture it
                            // 
                        {
                            Captures.Add(((row + direction) * 8) + column);
                        }
                        
                    }
                }
                    break;
                case "Bishop":
                {
                    var Bishop_Possible_move = Diagonal(row, column, square[0]);
                    Moves = Bishop_Possible_move[0];
                    Captures = Bishop_Possible_move[1];
                    break;
                }
                case "Rook":
                    var Rook_Possible_move = Straight(row, column, square[0]);
                    Moves = Rook_Possible_move[0];
                    Captures = Rook_Possible_move[1];
                    break;
                case "Queen":
                    var Queen_Possible_move = Diagonal(row, column, square[0]);
                    Moves = Queen_Possible_move[0];
                    Captures = Queen_Possible_move[1];
                    Queen_Possible_move = Straight(row, column, square[0]);
                    Moves.AddRange(Queen_Possible_move[0]);
                    Captures.AddRange(Queen_Possible_move[1]);
                    // add the Diagonal Moves and the Straight Moves for the queen
                    break;
                case "Knight":
                    var Knight_Possible_move = Lshape(row, column, square[0]);
                    Moves = Knight_Possible_move[0];
                    Captures = Knight_Possible_move[1];
                    break;
            }
            return [Moves, Captures];
        }
    }


    public List<List<int>> Diagonal(int row, int column, string pieceColor)
    {
        var Captures = new List<int>();
        var noCapture = new List<int>();
        // I would first go downward and right
        // add one to column and one to row
        
        var current_row = row;
        var current_column = column;

        while (true)
        {
            current_row += 1;
            current_column += 1;
            if(CheckIfValidAndUpdate(current_row, current_column , pieceColor, ref Captures, ref noCapture) != 1) break;
        }
        
        current_row = row;
        current_column = column;

        // then I would go up and left
        while (true)
        {
            current_row -= 1;
            current_column -= 1;
            if(CheckIfValidAndUpdate(current_row, current_column , pieceColor, ref Captures, ref noCapture) != 1) break;
        }
        
        current_row = row;
        current_column = column;
        // then I would go left and down
        while (true)
        {
            current_row += 1;
            current_column -= 1;
            if(CheckIfValidAndUpdate(current_row, current_column , pieceColor, ref Captures, ref noCapture) != 1) break;
        }
        current_row = row;
        current_column = column;
        // then I would go up and right
        while (true)
        {
            current_row -= 1;
            current_column += 1;
            if(CheckIfValidAndUpdate(current_row, current_column , pieceColor, ref Captures, ref noCapture) != 1) break;
        }
        
        return [noCapture, Captures];
    }

     public List<List<int>> Straight(int row, int column, string pieceColor)
    {
        var Captures = new List<int>();
        var noCapture = new List<int>();
       
        
        var current_row = row;
        var current_column = column;

        while (true)
        {
            // going down
            current_row += 1;
            if(CheckIfValidAndUpdate(current_row, column , pieceColor, ref Captures, ref noCapture) != 1) break;
        }
        
        current_row = row;
       

        // then I would go up and left
        while (true)
        {
            // going up
            current_row -= 1;
            if (CheckIfValidAndUpdate(current_row, column  , pieceColor, ref Captures, ref noCapture) != 1) break;
        }
        
        current_column = column;
        while (true)
        {
            // going left
            current_column -= 1;
            if (CheckIfValidAndUpdate(row, current_column , pieceColor, ref Captures, ref noCapture) != 1) break;
        }
        current_column = column;
        while (true)
        {
            // then go right
            current_column += 1;
            if (CheckIfValidAndUpdate(row , current_column , pieceColor, ref Captures, ref noCapture) != 1) break;
        }
        
        return [noCapture, Captures];
    }


    public List<List<int>> Lshape(int row, int column, string pieceColor)
    {
        var Captures = new List<int>();
        var noCapture = new List<int>();
        // I would first go downward and right
        // add one to column and one to row
        
        {
            CheckIfValidAndUpdate(row + 1, column + 2 , pieceColor, ref Captures, ref noCapture);
        }
       
        {
            CheckIfValidAndUpdate(row - 1, column + 2 , pieceColor, ref Captures, ref noCapture);
        } 
        {
            CheckIfValidAndUpdate(row - 1, column - 2 , pieceColor, ref Captures, ref noCapture);
        
        }
        {
            CheckIfValidAndUpdate(row + 1, column - 2 , pieceColor, ref Captures, ref noCapture);
        
        }
        
        {
            CheckIfValidAndUpdate(row + 2, column + 1 , pieceColor, ref Captures, ref noCapture);
        
        }
        
        {
            CheckIfValidAndUpdate(row + 2, column - 1 , pieceColor, ref Captures, ref noCapture);
        
        }
        
        {
            CheckIfValidAndUpdate(row - 2, column + 1 , pieceColor, ref Captures, ref noCapture);
        
        }
        
        {
            CheckIfValidAndUpdate(row - 2, column -1 , pieceColor, ref Captures, ref noCapture);
        }
        
        return [noCapture, Captures];
    }
    
    
    /*
     * Return 0 if not in valid range
     * Return 1 if added to noCapture
     * Return 2 if added to capture
     */
    public int CheckIfValidAndUpdate(int current_row, int current_column, string pieceColor, ref List<int> capture,
        ref List<int> noCapture)
    {
        if (current_row >= 0 && current_row < 8 && current_column >= 0 && current_column < 8)
        {
            if (string.IsNullOrEmpty(BoardRepresentation[((current_row) * 8) + current_column]))
            {
                noCapture.Add(((current_row) * 8) + current_column);
                return 1;
            }
            if (!String.Equals(BoardRepresentation[((current_row) * 8) + current_column].Split("-")[0], pieceColor,
                         StringComparison.InvariantCultureIgnoreCase))
                // check if the piece is not the same color as us
                // if it is not then we can capture it
            {
                capture.Add(((current_row) * 8) + current_column);
                return 2;
            }
        }
        return 0;
        
    }
    
    
    
    public BoardModel(List<String>? boardRepresentation)
    {
        if (BoardRepresentation is null)
        {
            BoardRepresentation = boardRepresentation = new List<string>
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
        else
        {
            BoardRepresentation = boardRepresentation;
        }
        
    }
}
