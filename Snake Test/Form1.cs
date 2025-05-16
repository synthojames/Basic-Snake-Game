using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Snake_Test
{
    public partial class Snake : Form
    {
        //Create Game Timer
        Timer gameTimer = new Timer();
        //Create the player object
        List<Rectangle> snake = new List<Rectangle>();
        //Create the food object
        Rectangle food;
        //define grid
        const int tileSize = 10;
        //create random
        Random rand = new Random();
        //track movement
        enum Direction { Up, Down, Left, Right }
        Direction playerDirection = Direction.Left;


        //scoreboard
        int playerScore = 0;

        //track gamestate
        bool gameOver = false;


        public Snake()
        {
            //start game
            InitializeComponent();

            //initialize timer
            gameTimer.Interval = 100;
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            //detect input
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

            //initialize objects
            snake = new List<Rectangle>();
            snake.Add(new Rectangle (5 * tileSize, 5* tileSize, tileSize, tileSize));
            food = new Rectangle(50, 50, tileSize, tileSize);

            //lock the window
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;


            
        }
        private void GameLoop(object sender, EventArgs e)
        {
            //movement
            //update the body positions
            for (int i = snake.Count - 1; i > 0; i--)
            {
                snake[i] = new Rectangle(snake[i - 1].X, snake[i - 1].Y, snake[i].Width, snake[i].Height);
            }
            //move the head
            Rectangle head = snake[0];

            switch(playerDirection)
            {
                case Direction.Up:
                    if(head.Y > 0)
                    {
                        head.Y -= tileSize;
                    }
                    break;
                case Direction.Down:
                    if(head.Y + head.Height < this.ClientSize.Height)
                    {
                        head.Y += tileSize;
                    }
                    break;
                case Direction.Left:
                    if(head.X > 0)
                    {
                        head.X -= tileSize;
                    }
                    break;
                case Direction.Right:
                    if(head.X + head.Width < this.ClientSize.Width)
                    {
                        head.X += tileSize;
                    }
                    break;
            }
           
            snake[0] = head;

            //Check for body collision
            for (int i = 1; i < snake.Count; i++)
            {
                if (head.IntersectsWith(snake[i]))
                {
                    gameOver = true; ;
                    break;
                }
            }
            if (gameOver == true)
            {
                gameTimer.Stop();
                MessageBox.Show("Game Over!");
                resetGame();
            }

            //Check for eating food


            if (snake[0].IntersectsWith(food))
            {
                //increase score
                playerScore++;



                //generate new food
                int maxX = this.ClientSize.Width / tileSize;
                int maxY = this.ClientSize.Height / tileSize;

                int foodX;
                int foodY;
                bool onSnake;
                do
                {
                    foodX = rand.Next(0, maxX) * tileSize;
                    foodY = rand.Next(0, maxY) * tileSize;

                    onSnake = false;
                    foreach (Rectangle segment in snake)
                    {
                        if (segment.X == foodX && segment.Y == foodY)
                        {
                            onSnake = true;
                            break;
                        }
                    }
                } while (onSnake);
                food = new Rectangle(foodX, foodY, tileSize, tileSize);

                // increase tail
                Rectangle lastSegment = snake[snake.Count - 1];
                snake.Add(new Rectangle(lastSegment.X, lastSegment.Y, lastSegment.Width, lastSegment.Height));
            }


            


            Invalidate();
        }

        //Movement
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.W && playerDirection != Direction.Down)
            { playerDirection = Direction.Up; }
            if(e.KeyCode == Keys.S && playerDirection != Direction.Up)
            { playerDirection = Direction.Down; }
            if(e.KeyCode == Keys.A && playerDirection != Direction.Right)
            { playerDirection = Direction.Left; }
            if(e.KeyCode == Keys.D && playerDirection != Direction.Left)
            { playerDirection = Direction.Right; } 
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            foreach(Rectangle segment in snake)
            {
                e.Graphics.FillRectangle(Brushes.White, segment);
            }
            e.Graphics.FillRectangle(Brushes.White, food);
            e.Graphics.DrawString($"{playerScore}",new Font("Arial", 16), Brushes.White, new PointF(10,10));
        }

        //reset game
        private void resetBody()
        {
            snake = new List<Rectangle>();
            snake.Add(new Rectangle(5 * tileSize, 5 * tileSize, tileSize, tileSize));
            food = new Rectangle(50, 50, tileSize, tileSize);
        }
        private void resetGame()
        {
            playerScore = 0;
            gameTimer.Start();
            resetBody();
            gameOver = false;

        }
    }
}
