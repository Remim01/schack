using Chess;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chess
{
    public partial class MainWindow : Window
    {
        private Board board;
        private Color currentPlayer;

        private Button? selectedButton = null;
        private int startX = -1;
        private int startY = -1;

        public MainWindow()
        {
            InitializeComponent();
            board = new Board();
            currentPlayer = Color.White;
            UpdateTurnLabel();
            DrawBoardUI();
        }

        private void UpdateTurnLabel()
        {
            TurnLabel.Text = currentPlayer == Color.White ? "Vits tur" : "Svarts tur";
        }

        private void DrawBoardUI()
        {
            ChessGrid.Children.Clear();

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Button btn = new Button();
                    btn.Tag = new Point(x, y);

                    // Sätt färg på rutorna
                    if ((x + y) % 2 == 0)
                        btn.Background = Brushes.BlanchedAlmond;
                    else
                        btn.Background = Brushes.SaddleBrown;

                    // Hämta pjäsen
                    Piece? p = board.Grid[x, y];
                    if (p != null)
                    {
                        btn.Content = p.Symbol.ToString();
                        btn.FontSize = 40;
                    }

                    btn.Click += Square_Click;
                    ChessGrid.Children.Add(btn);
                }
            }
        }

        private void Square_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender!;
            Point coords = (Point)clickedButton.Tag!;

            int x = (int)coords.X;
            int y = (int)coords.Y;

            if (selectedButton == null)
            {
                Piece? p = board.Grid[x, y];

                if (p != null && p.PieceColor == currentPlayer)
                {
                    selectedButton = clickedButton;
                    startX = x;
                    startY = y;

                    clickedButton.BorderBrush = Brushes.Yellow;
                    clickedButton.BorderThickness = new Thickness(4);
                }
            }
            else
            {
                int endX = x;
                int endY = y;

                if (board.MovePiece(startX, startY, endX, endY, currentPlayer))
                {
                    currentPlayer = (currentPlayer == Color.White) ? Color.Black : Color.White;
                    UpdateTurnLabel();
                }
                else
                {
                    MessageBox.Show("Ogiltigt drag!", "Fel", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                selectedButton = null;
                DrawBoardUI();
            }
        }
    }

    public enum Color { White, Black }

    public class Board
    {
        public Piece?[,] Grid = new Piece?[8, 8];

        public Board()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // Sätt ut svarta pjäser
            Grid[0, 0] = new Rook(Color.Black);
            Grid[7, 0] = new Rook(Color.Black);
            Grid[1, 0] = new Knight(Color.Black);
            Grid[6, 0] = new Knight(Color.Black);
            Grid[2, 0] = new Bishop(Color.Black);
            Grid[5, 0] = new Bishop(Color.Black);
            Grid[3, 0] = new Queen(Color.Black);
            Grid[4, 0] = new King(Color.Black);
            for (int i = 0; i < 8; i++) Grid[i, 1] = new Pawn(Color.Black);

            // Sätt ut vita pjäser
            Grid[0, 7] = new Rook(Color.White);
            Grid[7, 7] = new Rook(Color.White);
            Grid[1, 7] = new Knight(Color.White);
            Grid[6, 7] = new Knight(Color.White);
            Grid[2, 7] = new Bishop(Color.White);
            Grid[5, 7] = new Bishop(Color.White);
            Grid[3, 7] = new Queen(Color.White);
            Grid[4, 7] = new King(Color.White);
            for (int i = 0; i < 8; i++) Grid[i, 6] = new Pawn(Color.White);
        }

        public bool MovePiece(int startX, int startY, int endX, int endY, Color currentPlayer)
        {
            Piece? p = Grid[startX, startY];

            if (p == null || p.PieceColor != currentPlayer) return false;

            if (Grid[endX, endY] != null && Grid[endX, endY]?.PieceColor == currentPlayer) return false;

            if (p.IsValidMove(startX, startY, endX, endY, Grid))
            {
                Grid[endX, endY] = p;
                Grid[startX, startY] = null;
                return true;
            }
            return false;
        }
    }

    public abstract class Piece
    {
        public Color PieceColor { get; set; }
        public char Symbol { get; set; }

        public Piece(Color color, char symbol)
        {
            PieceColor = color;
            Symbol = symbol;
        }

        public abstract bool IsValidMove(int startX, int startY, int endX, int endY, Piece?[,] board);
    }

    public class Pawn : Piece
    {
        public Pawn(Color color) : base(color, color == Color.White ? '♙' : '♟') { }

        public override bool IsValidMove(int startX, int startY, int endX, int endY, Piece?[,] board)
        {
            int direction = (PieceColor == Color.White) ? -1 : 1;

            if (startX == endX && endY == startY + direction && board[endX, endY] == null)
            {
                return true;
            }
            return false;
        }
    }

    public class Rook : Piece
    {
        public Rook(Color color) : base(color, color == Color.White ? '♖' : '♜') { }

        public override bool IsValidMove(int startX, int startY, int endX, int endY, Piece?[,] board)
        {
            return true; // Tillfälligt true
        }
    }

    public class King : Piece
    {
        public King(Color color) : base(color, color == Color.White ? '♔' : '♚') { }

        public override bool IsValidMove(int startX, int startY, int endX, int endY, Piece?[,] board)
        {
            return true; // Tillfälligt true
        }
    }

    public class Queen : Piece
    {
        public Queen(Color color) : base(color, color == Color.White ? '♕' : '♛') { }

        public override bool IsValidMove(int startX, int startY, int endX, int endY, Piece?[,] board)
        {
            return true; // Tillfälligt true
        }
    }

    public class Bishop : Piece
    {
        public Bishop(Color color) : base(color, color == Color.White ? '♗' : '♝') { }

        public override bool IsValidMove(int startX, int startY, int endX, int endY, Piece?[,] board)
        {
            return true; // Tillfälligt true
        }
    }

    public class Knight : Piece
    {
        public Knight(Color color) : base(color, color == Color.White ? '♘' : '♞') { }

        public override bool IsValidMove(int startX, int startY, int endX, int endY, Piece?[,] board)
        {
            return true; // Tillfälligt true
        }
    }
}