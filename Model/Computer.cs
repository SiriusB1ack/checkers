using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace BionicFinalProject.Model
{




    /// <sumary>
    ///   The class responsible for handling
    ///  computer moves.
    /// </sumary>
    internal class Computer
    {
        // Board used for the computer moves
        private Checkers currentBoard;

        // Computer's pieces color
        private int color;

        // Max depth used in the Min-Max algorithm
        private int maxDepth = 1;

        // Weights used for the board
        private int[] tableWeight = { 4, 4, 4, 4, 
                                 4, 3, 3, 3,
                                 3, 2, 2, 4,
                                 4, 2, 1, 3,
                                 3, 1, 2, 4,
                                 4, 2, 2, 3,
                                 3, 3, 3, 4,
                                 4, 4, 4, 4};



        /**
         * Constructor.
         * @param gameBoard Tabuleiro que o computador deve usar para efectuar as jogadas.
         */
        public Computer(Checkers gameBoard)
        {
            currentBoard = gameBoard;
            if (currentBoard.currentPlayer) color = 2;
            else color = 4;
        }

        /// <sumary> 
        ///   Allows the user to change the max depth of
        ///  the min-max tree
        /// </sumary>
        public int depth
        {
            get
            {
                return maxDepth;
            }
            set
            {
                maxDepth = value;
            }
        }

        /// <sumary> 
        ///   Makes the computer play a move in the checkers
        ///  board that it holds.
        /// </sumary>
        public Move play()
        {
            Move moves = null;
            try
            {
                moves = minimax(currentBoard);                
            }
            catch (Exception bad)
            {
                Debug.WriteLine(bad.StackTrace);
                //Application.Exit ();
            }
            return moves;
        }

        /// <sumary> 
        ///   Changes the checkers board that is hold by the computer.
        /// </sumary>
        /// <param name="board">
        ///  The new checkers board
        /// </param>
        public void setBoard(Checkers board)
        {
            currentBoard = board;
        }

        /// <sumary> 
        ///   Says if the game move is valid
        /// </sumary>
        /// <param name="moves">
        ///  The list of piece movements for the game move.
        /// </param>
        /// <value>
        ///  true if the game move is valid, false otherwise.
        /// </value>
        private bool mayPlay(List<Move> moves)
        {
            return moves.Count != 0;
        }


        /// <sumary> 
        ///   Implements the Min-Max algorithm for selecting
        ///  the computer move
        /// </sumary>
        /// <param name="board">
        ///   The board that will be used as a starting point
        ///  for generating the game movements
        /// </param>
        /// <value>
        ///  A list with the computer game movements.
        /// </value>
        private Move minimax(Checkers board)
        {
            Move tmpmove;
            List<Move> sucessors;
            Move move, bestMove = null;
            Checkers nextBoard;
            int value, maxValue = Int32.MinValue;

            sucessors = board.LegalMoves();
            if (sucessors.Count == 1)
            {
                return sucessors[0];
            }
            tmpmove = new Move(sucessors[0].from, sucessors[0].to);
            while (mayPlay(sucessors))
            {
                move = sucessors[0];
                sucessors.RemoveAt(0);
                nextBoard = (Checkers)board.Clone();

                Debug.WriteLine("******************************************************************");
                nextBoard.Move(move);
                value = minMove(nextBoard, 1, maxValue, Int32.MaxValue);

                if (value > maxValue)
                {
                    Debug.WriteLine("Max value : " + value + " at depth : 0");
                    maxValue = value;
                    bestMove = move;
                }
            }

            Debug.WriteLine("Move value selected : " + maxValue + " at depth : 0");
            if (bestMove == null)
                bestMove = tmpmove;

            return bestMove;
        }

        /// <sumary> 
        ///   Implements game move evaluation from the point of view of the
        ///  MAX player.
        /// </sumary>
        /// <param name="board">
        ///   The board that will be used as a starting point
        ///  for generating the game movements
        /// </param>
        /// <param name="depth">
        ///   Current depth in the Min-Max tree
        /// </param>
        /// <param name="alpha">
        ///   Current alpha value for the alpha-beta cutoff
        /// </param>
        /// <param name="beta">
        ///   Current beta value for the alpha-beta cutoff
        /// </param>
        /// <value>
        ///  Move evaluation value
        /// </value>
        private int maxMove(Checkers board, int depth, int alpha, int beta)
        {
            if (cutOffTest(board, depth))
                return eval(board);


            List<Move> sucessors;
            Move move;
            Checkers nextBoard;
            int value;

            Debug.WriteLine("Max node at depth : " + depth + " with alpha : " + alpha +
                                " beta : " + beta);

            sucessors = board.LegalMoves();
            while (mayPlay(sucessors))
            {
                move = sucessors[0];
                sucessors.RemoveAt(0);
                nextBoard = (Checkers)board.Clone();
                nextBoard.Move(move);
                value = minMove(nextBoard, depth + 1, alpha, beta);

                if (value > alpha)
                {
                    alpha = value;
                    Debug.WriteLine("Max value : " + value + " at depth : " + depth);
                }

                if (alpha > beta)
                {
                    Debug.WriteLine("Max value with prunning : " + beta + " at depth : " + depth);
                    Debug.WriteLine(sucessors.Count + " sucessors left");
                    return beta;
                }

            }

            Debug.WriteLine("Max value selected : " + alpha + " at depth : " + depth);
            return alpha;
        }

        /// <sumary> 
        ///   Implements game move evaluation from the point of view of the
        ///  MIN player.
        /// </sumary>
        /// <param name="board">
        ///   The board that will be used as a starting point
        ///  for generating the game movements
        /// </param>
        /// <param name="depth">
        ///   Current depth in the Min-Max tree
        /// </param>
        /// <param name="alpha">
        ///   Current alpha value for the alpha-beta cutoff
        /// </param>
        /// <param name="beta">
        ///   Current beta value for the alpha-beta cutoff
        /// </param>
        /// <value>
        ///  Move evaluation value
        /// </value>
        private int minMove(Checkers board, int depth, int alpha, int beta)
        {
            if (cutOffTest(board, depth))
                return eval(board);


            List<Move> sucessors;
            Move move;
            Checkers nextBoard;
            int value;

            Debug.WriteLine("Min node at depth : " + depth + " with alpha : " + alpha +
                                " beta : " + beta);

            sucessors = board.LegalMoves();
            while (mayPlay(sucessors))
            {
                move = sucessors[0];
                sucessors.RemoveAt(0);
                nextBoard = (Checkers)board.Clone();
                nextBoard.Move(move);
                value = maxMove(nextBoard, depth + 1, alpha, beta);

                if (value < beta)
                {
                    beta = value;
                    Debug.WriteLine("Min value : " + value + " at depth : " + depth);
                }

                if (beta < alpha)
                {
                    Debug.WriteLine("Min value with prunning : " + alpha + " at depth : " + depth);
                    Debug.WriteLine(sucessors.Count + " sucessors left");
                    return alpha;
                }
            }

            Debug.WriteLine("Min value selected : " + beta + " at depth : " + depth);
            return beta;
        }

        /// <sumary> 
        ///   Evaluates the strength of the current player
        /// </sumary>
        /// <param name="board">
        ///   The board where the current player position will be evaluated.
        /// </param>
        /// <value>
        ///  Player strength
        /// </value>
        private int eval(Checkers board)
        {
            int colorKing;
            int colorForce = 0;
            int enemyForce = 0;
            int piece;

            if (color == 2)
                colorKing = 3;
            else
                colorKing = 5;

            try
            {
                for (int i = 0; i < 32; i++)
                {
                    piece = board.GetPiece(i);

                    if (piece != 0)
                        if (piece == color || piece == colorKing)
                            colorForce += calculateValue(piece, i);
                        else
                            enemyForce += calculateValue(piece, i);
                }
            }
            catch (Exception bad)
            {
                Debug.WriteLine(bad.StackTrace);
                //Application.Exit ();
            }

            return colorForce - enemyForce;
        }

        /// <sumary> 
        ///   Evaluates the strength of a piece
        /// </sumary>
        /// <param name="piece">
        ///   The type of piece
        /// </param>
        /// <param name="pos">
        ///   The piece position
        /// </param>
        /// <value>
        ///  Piece value
        /// </value>
        private int calculateValue(int piece, int pos)
        {
            int value;

            if (piece == 2) //Simple piece
                if (pos >= 4 && pos <= 7)
                    value = 7;
                else
                    value = 5;
            else if (piece != 4) //Simple piece
                if (pos >= 24 && pos <= 27)
                    value = 7;
                else
                    value = 5;
            else // king piece
                value = 10;

            return value * tableWeight[pos];
        }


        /// <sumary> 
        ///   Verifies if the game tree can be prunned
        /// </sumary>
        /// <param name="board">
        ///   The board to evaluate
        /// </param>
        /// <param name="depth">
        ///   Current game tree depth
        /// </param>
        /// <value>
        ///  true if the tree can be prunned.
        /// </value>
        private bool cutOffTest(Checkers board, int depth)
        {
            return (depth > maxDepth) || (board.HasEnded());
        }

    }
}
