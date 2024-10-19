using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private List<Point> snake = new List<Point>();
        private Point food;
        private int score = 0;
        private int directionX = 0, directionY = 0;
        private int snakeSize = 10;
        private bool isGameOver = false;

        public Form1()
        {
            InitializeComponent();
            gameTimer.Tick += UpdateGame; // Đăng ký sự kiện tick chỉ một lần
            gameTimer.Interval = 200;  // Tốc độ di chuyển của rắn, khởi tạo ở đây
        }

        private void StartGame()
        {
            snake.Clear();
            snake.Add(new Point(100, 100));
            directionX = 0;
            directionY = 0;
            score = 0;
            isGameOver = false;

            gameTimer.Start();  // Bắt đầu lại timer

            GenerateFood();
            lblScore.Text = "Score: " + score;
        }

        private void GenerateFood()
        {
            Random rand = new Random();
            int maxX = this.ClientSize.Width / snakeSize;
            int maxY = this.ClientSize.Height / snakeSize;
            food = new Point(rand.Next(0, maxX) * snakeSize, rand.Next(0, maxY) * snakeSize);
        }

        private void UpdateGame(object sender, EventArgs e)
        {
            if (isGameOver)
                return;

            for (int i = snake.Count - 1; i > 0; i--)
            {
                snake[i] = snake[i - 1];
            }

            snake[0] = new Point(snake[0].X + directionX * snakeSize, snake[0].Y + directionY * snakeSize);

            if (snake[0].X < 0 || snake[0].Y < 0 || snake[0].X >= this.ClientSize.Width || snake[0].Y >= this.ClientSize.Height)
            {
                GameOver();
            }

            for (int i = 1; i < snake.Count; i++)
            {
                if (snake[0] == snake[i])
                {
                    GameOver();
                }
            }

            if (snake[0] == food)
            {
                snake.Add(new Point(snake[snake.Count - 1].X, snake[snake.Count - 1].Y));
                score++;
                lblScore.Text = "Score: " + score;
                GenerateFood();
            }

            this.Invalidate();
        }

        private void GameOver()
        {
            gameTimer.Stop();  // Dừng timer khi game over
            isGameOver = true;
            MessageBox.Show("Game Over! Your score is: " + score);
            StartGame();  // Bắt đầu lại trò chơi
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            for (int i = 0; i < snake.Count; i++)
            {
                canvas.FillRectangle(Brushes.Green, new Rectangle(snake[i].X, snake[i].Y, snakeSize, snakeSize));
            }

            canvas.FillRectangle(Brushes.Red, new Rectangle(food.X, food.Y, snakeSize, snakeSize));
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (directionY != 1)
                    {
                        directionX = 0;
                        directionY = -1;
                    }
                    break;
                case Keys.Down:
                    if (directionY != -1)
                    {
                        directionX = 0;
                        directionY = 1;
                    }
                    break;
                case Keys.Left:
                    if (directionX != 1)
                    {
                        directionX = -1;
                        directionY = 0;
                    }
                    break;
                case Keys.Right:
                    if (directionX != -1)
                    {
                        directionX = 1;
                        directionY = 0;
                    }
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartGame();  // Bắt đầu trò chơi khi form được load
        }
    }
}
