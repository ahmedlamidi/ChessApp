using System;
using System.Collections.Generic;

namespace ChessAvalonia.Models;


// I need an evaluation function
// A check should be inf
// Then after this I would prune
// get a depth of three

// A get all next moves function
public class BasicChessEngine
{

    public List<Tuple<int, int>> NextMoves(int player)
    {
        // make white equal to 1 and black equal to -1
        // based on the board and the player we want to get all possible moves
        // assuming that the board is in the state of 64 strings with 0 meaning empty negative meaning black
        // positive meaning white
        // then pawn - 1, bishop - 2, knight - 3, rook - 4, queen - 5, king - 6
        // we need 64 * 2 to represent the whole board
            var Moves = new List<Tuple<int, int>>();
            int position = -1;
            foreach (var piece in BoardRepresentation)
            {
                position++;
                if (piece == 0) continue;
                int peice_type = Math.Abs(piece);
                int piece_color = piece > 0 ? 1 : -1;
                // we basically just make the piece type the  actual value
                // then we make the piece color into 
                if (piece_color == player)
                {
                    switch (peice_type)
                    {
                        case 1:// case of pawn
                            PawnMove(position/ 8, position % 8, ref Moves, player, position);
                            break;
                        case 2: // case of bishop 
                            // I want to make moves a list of the start to end positions I can do for the bishop
                            Diagonal(position / 8, position % 8, ref Moves, player, position);
                            break;
                        case 3: // case of the knight
                            Lshape(position / 8, position % 8, ref Moves, player, position);
                            break;
                        case 4: // case of rook
                            Straight(position / 8, position % 8, ref Moves, player, position);
                            break;
                        case 5:// case of queen
                            Diagonal(position / 8, position % 8, ref Moves, player, position);
                            Straight(position / 8, position % 8, ref Moves, player, position);
                            break;
                        case 6:
                            // all the moves for the king
                            BoxAround(position / 8, position % 8, ref Moves, player, position);
                            break;
                    }
                }
            }

            return Moves;  
    }
    
    
    public int CheckIfValidAndUpdate(int current_row, int current_column, ref List<Tuple<int, int>> moves, int player, int position)
    {
        if (current_row >= 0 && current_row < 8 && current_column >= 0 && current_column < 8)
        {
            if (BoardRepresentation[((current_row) * 8) + current_column] == 0)
            {
                var newMove = new Tuple<int, int>((current_row * 8) + current_column , position);
                if (!moves.Contains(newMove)) moves.Add((new Tuple<int, int>((current_row * 8) + current_column , position)));
                return 1;
            }
            if (BoardRepresentation[((current_row) * 8) + current_column] * player < 0)
                // check if the piece is not the same color as us
                // if it is not then we can capture it
            {
                var newMove = new Tuple<int, int>((current_row * 8) + current_column , position);
                if (!moves.Contains(newMove)) moves.Add((new Tuple<int, int>((current_row * 8) + current_column , position)));
                return -1;
            }
        }
        return -1;
    }
    
    public void Diagonal(int row, int column, ref List<Tuple<int, int>> moves, int player, int position)
    {
        // I would first go downward and right
        // add one to column and one to row
        
        var current_row = row;
        var current_column = column;

        while (true)
        {
            current_row += 1;
            current_column += 1;
            if(CheckIfValidAndUpdate(current_row, current_column , ref moves, player, position ) != 1) break;
        }
        
        current_row = row;
        current_column = column;

        // then I would go up and left
        while (true)
        {
            current_row -= 1;
            current_column -= 1;
            if(CheckIfValidAndUpdate(current_row, current_column , ref moves, player, position) != 1) break;
        }
        
        current_row = row;
        current_column = column;
        // then I would go left and down
        while (true)
        {
            current_row += 1;
            current_column -= 1;
            if(CheckIfValidAndUpdate(current_row, current_column , ref moves, player, position) != 1) break;
        }
        current_row = row;
        current_column = column;
        // then I would go up and right
        while (true)
        {
            current_row -= 1;
            current_column += 1;
            if(CheckIfValidAndUpdate(current_row, current_column , ref moves, player, position) != 1) break;
        }
        
    }
    
    public void Lshape(int row, int column, ref List<Tuple<int, int>> moves, int player, int position)
    {
        var Captures = new List<int>();
        var noCapture = new List<int>();
        // I would first go downward and right
        // add one to column and one to row
        
        {
            CheckIfValidAndUpdate(row + 1, column + 2 , ref moves, player, position);
        }
       
        {
            CheckIfValidAndUpdate(row - 1, column + 2 ,ref moves, player, position);
        } 
        {
            CheckIfValidAndUpdate(row - 1, column - 2 , ref moves, player, position);
        
        }
        {
            CheckIfValidAndUpdate(row + 1, column - 2 , ref moves, player, position);
        
        }
        
        {
            CheckIfValidAndUpdate(row + 2, column + 1 , ref moves, player, position);
        
        }
        
        {
            CheckIfValidAndUpdate(row + 2, column - 1 , ref moves, player, position);
        
        }
        
        {
            CheckIfValidAndUpdate(row - 2, column + 1 , ref moves, player, position);
        
        }
        
        {
            CheckIfValidAndUpdate(row - 2, column -1 , ref moves, player, position);
        }
        
    }

    public void BoxAround(int row, int column, ref List<Tuple<int, int>> moves, int player, int position)
    {
        CheckIfValidAndUpdate(row + 1, column  , ref moves, player, position );
        CheckIfValidAndUpdate(row + 1, column + 1 , ref moves, player, position );
        CheckIfValidAndUpdate(row + 1, column - 1 , ref moves, player, position );
        CheckIfValidAndUpdate(row - 1, column  , ref moves, player, position );
        CheckIfValidAndUpdate(row - 1, column + 1 , ref moves, player, position );
        CheckIfValidAndUpdate(row - 1, column - 1 , ref moves, player, position );
        CheckIfValidAndUpdate(row , column + 1 , ref moves, player, position );
        CheckIfValidAndUpdate(row, column - 1 , ref moves, player, position );
    }
    
    public void Straight(int row, int column, ref List<Tuple<int, int>> moves, int player, int position)
    {
        var Captures = new List<int>();
        var noCapture = new List<int>();
       
        
        var current_row = row;
        var current_column = column;

        while (true)
        {
            // going down
            current_row += 1;
            if(CheckIfValidAndUpdate(current_row, column ,  ref moves, player, position) != 1) break;
        }
        
        current_row = row;
       

        // then I would go up and left
        while (true)
        {
            // going up
            current_row -= 1;
            if (CheckIfValidAndUpdate(current_row, column  ,  ref moves, player, position) != 1) break;
        }
        
        current_column = column;
        while (true)
        {
            // going left
            current_column -= 1;
            if (CheckIfValidAndUpdate(row, current_column , ref moves, player, position) != 1) break;
        }
        current_column = column;
        while (true)
        {
            // then go right
            current_column += 1;
            if (CheckIfValidAndUpdate(row , current_column ,  ref moves, player, position) != 1) break;
        }
        
    }

    public void PawnMove(int row, int column, ref List<Tuple<int, int>> moves, int player, int position)
    {
        // we can always check if the + 1 is a valid move
        CheckIfValidAndUpdate(row - player, column, ref moves, player, position);
        // remember that white is 1 and black is -1 
        // white is going up

        // we can then check if we are at the starting row
        if ((player == 1 && row == 6) || (player == -1 && row == 1))
        {
            CheckIfValidAndUpdate(row - (2 * player), column , ref moves, player, position);
        }
        // covers the case where we are on the starting row

        // if we have someone on the front right or front left, we can check that too

        if (BoardRepresentation[((row - player) * 8) + column + 1] != 0)
        {
            CheckIfValidAndUpdate(row - player, column + 1 , ref moves, player, position);
        }

        if (BoardRepresentation[((row - player) * 8) + column - 1] != 0)
        {
            CheckIfValidAndUpdate(row - player, column + 1 , ref moves, player, position);
        }
        // no enpassant // It would be a pain to keep track of who just moved twice
    }
    
    public List<int> BoardRepresentation { get; set; } = new List<int>( [-4, -3, -2, -5, -6, -2, -3, -4, 
        -1, -1, -1, -1, -1, -1, -1, -1,  
        0,  0,  0,  0,  0,  0,  0,  0,  
        0,  0,  0,  0,  0,  0,  0,  0,  
        0,  0,  0,  0,  0,  0,  0,  0,  
        0,  0,  0,  0,  0,  0,  0,  0,  
        1,  1,  1,  1,  1,  1,  1,  1,  
        4,  3,  2,  5,  6,  2,  3,  4]);
    
    public double EvaluatePosition()
    {
        return 0.0;
    }
}