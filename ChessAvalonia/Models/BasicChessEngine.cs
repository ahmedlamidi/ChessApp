
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;

namespace ChessAvalonia.Models;


// I need an evaluation function
// A check should be inf
// Then after this I would prune
// get a depth of three

// A get all next moves function

// so I want to create a new board -> then get the depth for a certain player
// then each time I would update the best move for that board 
// at the end I would take the best move which is a variable of the board
public class BasicChessEngine
{
    public enum PieceValues : int
    {
        Pawn = 1,
        Bishop = 4,
        Knight = 3,
        Rook = 5,
        Queen = 9, 
        King = 15
    }
      public static readonly List<int> PawnTable = new List<int>
      {
          1,  1,  1,  1,  1,  1,  1,  1,
          6, 11, 11,  1,  1, 11, 11,  6,
          6,  1,  1,  5,  5,  1,  1,  6,
          1,  1,  1, 21, 21,  1,  1,  1,
          6,  6, 11, 26, 26, 11,  6,  6,
          11, 11, 21, 31, 31, 21, 11, 11,
          51, 51, 51, 51, 51, 51, 51, 51,
          1,  1,  1,  1,  1,  1,  1,  1
      };


    public static readonly List<int> KnightTable = new List<int>
    {
        1,  1,  1,  1,  1,  1,  1,  1,
        1, 21, 31, 31, 31, 31, 21,  1,
        1, 31, 41, 46, 46, 41, 31,  1,
        1, 26, 46, 51, 51, 46, 26,  1,
        1, 31, 46, 51, 51, 46, 31,  1,
        1, 26, 41, 46, 46, 41, 26,  1,
        1, 21, 31, 36, 36, 31, 21,  1,
        1,  1,  1,  1,  1,  1,  1,  1
    };


    public static readonly List<int> BishopTable = new List<int>
    {
        1,  1,  1,  1,  1,  1,  1,  1,
        1, 21, 21, 21, 21, 21, 21,  1,
        1, 21, 26, 31, 31, 26, 21,  1,
        1, 26, 26, 31, 31, 26, 26,  1,
        1, 21, 31, 31, 31, 31, 21,  1,
        1, 31, 31, 31, 31, 31, 31,  1,
        1, 26, 21, 21, 21, 21, 26,  1,
        1,  1,  1,  1,  1,  1,  1,  1
    };

    public static readonly List<int> RookTable = new List<int>
    {
        1,  1,  1,  6,  6,  1,  1,  1,
        6, 11, 11, 11, 11, 11, 11,  6,
        6, 11, 11, 11, 11, 11, 11,  6,
        6, 11, 11, 11, 11, 11, 11,  6,
        6, 11, 11, 11, 11, 11, 11,  6,
        6, 11, 11, 11, 11, 11, 11,  6,
        11, 21, 21, 21, 21, 21, 21, 11,
        1,  1,  1,  1,  1,  1,  1,  1
    };


    public static readonly List<int> QueenTable = new List<int>
    {
        1,  1,  1,  6,  6,  1,  1,  1,
        6, 11, 11, 11, 11, 11, 11,  6,
        6, 11, 16, 16, 16, 16, 11,  6,
        11, 11, 16, 16, 16, 16, 11,  6,
        11, 11, 16, 16, 16, 16, 11,  6,
        6, 11, 16, 16, 16, 16, 11,  6,
        6, 11, 11, 11, 11, 11, 11,  6,
        1,  1,  1,  6,  6,  1,  1,  1
    };


    public static readonly List<int> KingTable = new List<int>
    {
        11,  1,  1,  1,  1,  1,  1, 11,
        11, 21, 21,  1,  1, 21, 21, 11,
        11, 21, 21,  1,  1, 21, 21, 11,
        11, 21, 21,  1,  1, 21, 21, 11,
        11, 21, 21,  1,  1, 21, 21, 11,
        11, 21, 21, 21, 21, 21, 21, 11,
        11, 31, 31, 31, 31, 31, 31, 11,
        21, 31, 21, 11, 11, 21, 31, 21
    };

    public List<Tuple<int, int>> NextMoves(int player)
    {
        // make white equal to 1 and black equal to -1
        // based on the board and the player we want to get all possible moves
        // assuming that the board is in the state of 64 strings with 0 meaning empty negative meaning black
        // positive meaning white
        // then pawn - 1, bishop - 5 , knight - 7, rook - 12, queen - 25, king - 100
        // we need 64 * 2 to represent the whole board
        var Moves = new List<Tuple<int, int>>();
        int position = -1;
        foreach (var piece in BoardRepresentation)
        {
            position++;
            if (piece == 0) continue;
            double peice_type = Math.Abs(piece);
            int piece_color = piece > 0 ? 1 : -1;
            // we basically just make the piece type the  actual value
            // then we make the piece color into 
            if (piece_color == player)
            {
                switch (peice_type)
                {
                    case 1: // case of pawn
                        PawnMove(position / 8, position % 8, ref Moves, player, position);
                        break;
                    case 3.25: // case of bishop 
                        // I want to make moves a list of the start to end positions I can do for the bishop
                        Diagonal(position / 8, position % 8, ref Moves, player, position);
                        break;
                    case 3: // case of the knight
                        Lshape(position / 8, position % 8, ref Moves, player, position);
                        break;
                    case 5: // case of rook
                        Straight(position / 8, position % 8, ref Moves, player, position);
                        break;
                    case 9: // case of queen
                        Diagonal(position / 8, position % 8, ref Moves, player, position);
                        Straight(position / 8, position % 8, ref Moves, player, position);
                        break;
                    case 15:
                        // all the moves for the king
                        BoxAround(position / 8, position % 8, ref Moves, player, position);
                        break;
                }
            }
        }

        return Moves;
        // returning the next moves works correctly
    }


    public int CheckIfValidAndUpdate(int current_row, int current_column, ref List<Tuple<int, int>> moves, int player,
        int position)
    {
        if (current_row >= 0 && current_row < 8 && current_column >= 0 && current_column < 8)
        {
            if (BoardRepresentation[((current_row) * 8) + current_column] == 0)
            {
                var newMove = new Tuple<int, int>((current_row * 8) + current_column, position);
                if (!moves.Contains(newMove))
                    moves.Add((new Tuple<int, int>((current_row * 8) + current_column, position)));
                return 1;
            }

            if (BoardRepresentation[((current_row) * 8) + current_column] * player < 0)
                // check if the piece is not the same color as us
                // if it is not then we can capture it
            {
                var newMove = new Tuple<int, int>((current_row * 8) + current_column, position);
                if (!moves.Contains(newMove))
                    moves.Add((new Tuple<int, int>((current_row * 8) + current_column, position)));
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
            if (CheckIfValidAndUpdate(current_row, current_column, ref moves, player, position) != 1) break;
        }

        current_row = row;
        current_column = column;

        // then I would go up and left
        while (true)
        {
            current_row -= 1;
            current_column -= 1;
            if (CheckIfValidAndUpdate(current_row, current_column, ref moves, player, position) != 1) break;
        }

        current_row = row;
        current_column = column;
        // then I would go left and down
        while (true)
        {
            current_row += 1;
            current_column -= 1;
            if (CheckIfValidAndUpdate(current_row, current_column, ref moves, player, position) != 1) break;
        }

        current_row = row;
        current_column = column;
        // then I would go up and right
        while (true)
        {
            current_row -= 1;
            current_column += 1;
            if (CheckIfValidAndUpdate(current_row, current_column, ref moves, player, position) != 1) break;
        }

    }

    public void Lshape(int row, int column, ref List<Tuple<int, int>> moves, int player, int position)
    {
        var Captures = new List<int>();
        var noCapture = new List<int>();
        // I would first go downward and right
        // add one to column and one to row

        {
            CheckIfValidAndUpdate(row + 1, column + 2, ref moves, player, position);
        }

        {
            CheckIfValidAndUpdate(row - 1, column + 2, ref moves, player, position);
        }
        {
            CheckIfValidAndUpdate(row - 1, column - 2, ref moves, player, position);

        }
        {
            CheckIfValidAndUpdate(row + 1, column - 2, ref moves, player, position);

        }

        {
            CheckIfValidAndUpdate(row + 2, column + 1, ref moves, player, position);

        }

        {
            CheckIfValidAndUpdate(row + 2, column - 1, ref moves, player, position);

        }

        {
            CheckIfValidAndUpdate(row - 2, column + 1, ref moves, player, position);

        }

        {
            CheckIfValidAndUpdate(row - 2, column - 1, ref moves, player, position);
        }

    }

    public void BoxAround(int row, int column, ref List<Tuple<int, int>> moves, int player, int position)
    {
        CheckIfValidAndUpdate(row + 1, column, ref moves, player, position);
        CheckIfValidAndUpdate(row + 1, column + 1, ref moves, player, position);
        CheckIfValidAndUpdate(row + 1, column - 1, ref moves, player, position);
        CheckIfValidAndUpdate(row - 1, column, ref moves, player, position);
        CheckIfValidAndUpdate(row - 1, column + 1, ref moves, player, position);
        CheckIfValidAndUpdate(row - 1, column - 1, ref moves, player, position);
        CheckIfValidAndUpdate(row, column + 1, ref moves, player, position);
        CheckIfValidAndUpdate(row, column - 1, ref moves, player, position);
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
            if (CheckIfValidAndUpdate(current_row, column, ref moves, player, position) != 1) break;
        }

        current_row = row;


        // then I would go up and left
        while (true)
        {
            // going up
            current_row -= 1;
            if (CheckIfValidAndUpdate(current_row, column, ref moves, player, position) != 1) break;
        }

        current_column = column;
        while (true)
        {
            // going left
            current_column -= 1;
            if (CheckIfValidAndUpdate(row, current_column, ref moves, player, position) != 1) break;
        }

        current_column = column;
        while (true)
        {
            // then go right
            current_column += 1;
            if (CheckIfValidAndUpdate(row, current_column, ref moves, player, position) != 1) break;
        }

    }

    public void PawnMove(int row, int column, ref List<Tuple<int, int>> moves, int player, int position)
    {
        // we can always check if the + 1 is a valid move
        if (BoardRepresentation[((row - player)) * 8 + column] == 0)
        {
            CheckIfValidAndUpdate(row - player, column, ref moves, player, position);
        }
        // remember that white is 1 and black is -1 
        // white is going up

        // we can then check if we are at the starting row
        if ((player == 1 && row == 6 && BoardRepresentation[((4) * 8) + column ] == 0) || (player == -1 && row == 1 && BoardRepresentation[((3) * 8) + column ] == 0))
        {
            CheckIfValidAndUpdate(row - (2 * player), column, ref moves, player, position);
        }
        // covers the case where we are on the starting row

        // if we have someone on the front right or front left, we can check that too

        if (column < 7 && BoardRepresentation[((row - player) * 8) + column + 1] != 0)
        {
            CheckIfValidAndUpdate(row - player, column + 1, ref moves, player, position);
        }

        if (column > 0 && BoardRepresentation[((row - player) * 8) + column - 1] != 0)
        {
            CheckIfValidAndUpdate(row - player, column + 1, ref moves, player, position);
        }
        // no enpassant // It would be a pain to keep track of who just moved twice
    }
    
    public Tuple<int, int> move_to_play {get; set; }

    public List<double> BoardRepresentation { get; set; } = new List<double>([
        -5, -3, -3.25, -9, -15, -3.25, -3, -5,
        -1, -1, -1, -1, -1, -1, -1, -1,
        0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0,
        1, 1, 1, 1, 1, 1, 1, 1,
        5, 3, 3.25, 9, 15, 3.25, 3, 5
    ]);

    public double EvaluatePosition(int player)
    {
        // remember that white is 1
        // black is -1
        Random rnd = new Random();
        // return BoardRepresentation.Sum() > 0 ? (1 * rnd.Next(300)) : (-1 * rnd.Next(300));

        double total = 0;
        int position = -1;
        foreach (var value in BoardRepresentation)
        {
            position += 1;
            switch (Math.Abs(value))
            {
                case 1:
                    if (value >= 0) total += (PawnTable[position] * value) ;
                    else total += (PawnTable[63 -position] * value);
                    break;
                case 3:
                    if (value >= 0) total += (BishopTable[position] * value) ;
                    else total += (BishopTable[63 -position] * (value / 1.5));
                    break;
                case 3.25:
                    if (value >= 0) total += (KnightTable[position] * value) ;
                    else total += (KnightTable[63 -position] * (value / 1.5));
                    break;
                case 5:
                    if (value >= 0) total += (RookTable[position] * value) ;
                    else total += (RookTable[63 - position] * (value / 3.5));
                    break; 
                case 9:
                    if (value >= 0) total += (QueenTable[position] * value) ;
                    else total += (QueenTable[ 63 -position] *( value / 5));
                    break;
                case 15:
                    if (value >= 0) total += (KingTable[position] * value) ;
                    else total += (KingTable[63 -position] * (value / 8));
                    break;
            }
        }
        return total;
        // this is the simplest evaluation function
    }

    public double DepthSearch(int player, int original_player, int depth, double alpha, double beta)
    {
        if (depth == 0)
        {
            return EvaluatePosition(player);
        }
        {
            
            // for alpha beta pruning I want to see
            // I want Alpha to be the max value - - inf
            // Beta should be the min + inf 
            //  in the min part - change Beta Value 
            // we are basically just seeing at a higher step that we do not need to check
            // since all other options would be worse in that branch
            
            // set tbe best move to nothing at the start
            double maxmimizing_score = int.MinValue;
            double minimizing_score = int.MinValue;
            foreach (var move in NextMoves(player))
            {
                var save_space = BoardRepresentation[move.Item1];
                // we save the peice in the starting board
                // then we move the board piece from 2 to 1
                // then we set the second one to empty
                BoardRepresentation[move.Item1] = BoardRepresentation[move.Item2];
                BoardRepresentation[move.Item2] = 0;
                var next_level = new BasicChessEngine(BoardRepresentation);
                var eval = next_level.DepthSearch(player * -1, original_player, depth - 1, alpha, beta);
                BoardRepresentation[move.Item2] = BoardRepresentation[move.Item1];
                BoardRepresentation[move.Item1] = save_space;
                if (player != original_player) // maximizing
                {
                    if (eval >= maxmimizing_score)
                    {
                        move_to_play = move;
                        maxmimizing_score = eval;
                    }
                    if (eval > beta) break;
                    alpha = Math.Max(alpha, eval);

                }

                else if(player == original_player) // minimizing 
                {
                    if (eval <= minimizing_score)
                    {
                        minimizing_score = eval;
                        move_to_play = move;
                    }
                    if (eval <= alpha) break;
                    beta = Math.Min(beta, eval);
                }
            }
            
            if (player == original_player) // minimizing
            {
                
                return minimizing_score;
            }
            return maxmimizing_score;
        }
    }


    public BasicChessEngine(List<double> startingposition)
    {
        if ( startingposition is not null && startingposition.Count != 0)
        {
            BoardRepresentation = startingposition;
        }
    }


}