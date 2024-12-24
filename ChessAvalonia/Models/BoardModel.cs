using System;
using System.Collections.Generic;

namespace ChessAvalonia.Models;

public class BoardModel
{
    
    public List<String> BoardRepresentation { get; set; }


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

                        else if (!String.Equals(BoardRepresentation[((row + direction) * 8) + column], square[0],
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
            }
            return [Moves, Captures];
        }
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