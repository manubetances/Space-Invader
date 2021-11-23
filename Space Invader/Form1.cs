using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Space_Invader
{
    public partial class Form1 : Form
    {

        bool moveLeft, moveRight;
        int playerSpeed = 17;
        int enemySpeed = 5;
        int score = 0;
        int enemyBulletTimer = 300;
        bool shooting, isGameOver;
        PictureBox[] spaceInvaders;
        public Form1()
        {
            InitializeComponent();
            gameSetup();
        }

        private void keyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)  //If the arrow key left is pressed it will move left 
            {
                moveLeft = true;
            }
            if (e.KeyCode == Keys.Right) //If the arrow key right is pressed it will move right 
            {
                moveRight = true;
            }
        }

        private void keyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)  //Stop movement to the left 
            {
                moveLeft = false;
            }
            if (e.KeyCode == Keys.Right) //Stop movement to the right 
            {
                moveRight = false;
            }
            if (e.KeyCode == Keys.Space && shooting == false)
            {
                shooting = true;
                makeBullet("bullet");
            }
            if (e.KeyCode == Keys.Enter && isGameOver == true)
            {
                cleanScreen();
                gameSetup();
            }
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            textScore.Text = "Score: " + score;
            if (moveLeft) 
            {
                player.Left -= playerSpeed;
            }
            if (moveRight) 
            {
                player.Left += playerSpeed;
            }
            enemyBulletTimer -= 10;

            if(enemyBulletTimer < 1) 
            {
                enemyBulletTimer = 300;
                makeBullet("invaderBullet");
            }

            foreach (Control x in this.Controls) 
            {
                if (x is PictureBox && (string)x.Tag == "spaceInvaders") 
                {
                    x.Left += enemySpeed;
                    if(x.Left > 730) 
                    {
                        x.Top += 65;
                        x.Left = -80;
                    }
                    if (x.Bounds.IntersectsWith(player.Bounds)) 
                    {
                        gameOver("You've been invaded by the aliens");
                    }

                    foreach(Control y in this.Controls) 
                    {
                        if(y is PictureBox && (string)y.Tag == "bullet") 
                        {
                            if (y.Bounds.IntersectsWith(x.Bounds)) 
                            {
                                this.Controls.Remove(x);
                                this.Controls.Remove(y);
                                score += 1;
                                shooting = false;
                            }
                        }
                    }
                }

                if(x is PictureBox && (string)x.Tag == "bullet") 
                {
                    x.Top -= 20;
                    if(x.Top < 15) 
                    {
                        this.Controls.Remove(x);
                        shooting = false;
                    }
                }
                if(x is PictureBox && (string)x.Tag == "spaceBullet") 
                {
                    x.Top += 20;
                    if(x.Top > 620) 
                    {
                        this.Controls.Remove(x);
                    }
                    if (x.Bounds.IntersectsWith(player.Bounds)) 
                    {
                        this.Controls.Remove(x);
                        gameOver("You've been killed!");
                    }
                }
            }
            if(score > 8) 
            {
                enemySpeed = 12;
            }
            if(score == spaceInvaders.Length) 
            {
                gameOver("You've defended Earth");
            }
        }

        private void gameSetup() //Initial game setup
        {
            textScore.Text = "Score: 0";
            score = 0;
            isGameOver = false;
            enemyBulletTimer = 300;
            shooting = false;
            makeInvaders();
            gameTime.Start();
        }

        private void makeInvaders() //Spawns invaders
        {
            spaceInvaders = new PictureBox[15]; //It will make 15 invaders
            int left = 0;
            for (int i = 0; i < spaceInvaders.Length; ++i)
            {
                spaceInvaders[i] = new PictureBox();
                spaceInvaders[i].Size = new Size(60, 50);
                spaceInvaders[i].Image = Properties.Resources.SpaceInvader;
                spaceInvaders[i].Top = 5;
                spaceInvaders[i].Tag = "spaceInvaders";
                spaceInvaders[i].Left = left;
                spaceInvaders[i].SizeMode = PictureBoxSizeMode.StretchImage;
                this.Controls.Add(spaceInvaders[i]); // min 15
                left = left - 50; //Distance the invaders will spawn

            }
        }

        private void makeBullet(string bulletTag) //Makes the bullet the playe shoots
        {
            PictureBox bullet = new PictureBox();
            bullet.Image = Properties.Resources.bullet; //Choosing the bullet's image
            bullet.Size = new Size(5, 20);
            bullet.Tag = bulletTag;
            bullet.Left = player.Left + player.Width / 2; //Putting the bullet in the center of the player

            if((string)bullet.Tag == "bullet") 
            {
                bullet.Top = player.Top - 20;
            }
            else if ((string)bullet.Tag == "invaderBullet")
            {
                bullet.Top = -100;
            }
            this.Controls.Add(bullet);
            bullet.BringToFront();
        }

        private void gameOver(string message) //When you lose the game
        {
            isGameOver = true;
            gameTime.Stop();
            textScore.Text = "Score: " + score + " " + message;
        }

        private void cleanScreen() //Deletes everything in the screen to create a new game
        {
            foreach (PictureBox i in spaceInvaders)
            {
                this.Controls.Remove(i);
            }
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "bullet" || (string)x.Tag == "invaderBullet")
                    {
                        this.Controls.Remove(x);
                    }
                }
            }
        }
    }
}
