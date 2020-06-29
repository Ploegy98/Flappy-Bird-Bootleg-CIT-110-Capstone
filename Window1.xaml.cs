using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Windows.Threading;

namespace Flappy_Bird_Bootleg
{   // ************************************************************************************
    // Title: Flappy Bird Ripoff
    // Application Type: WPF App(.Net Framework) - C#
    // Description: Flappy Bird application recreation for console, with use of C# and WPF
    // Author: Casey Vanderpleog
    // Date Created: 6/26/2020
    // Last Modified: 6/29/202
    // ************************************************************************************
    /// <summary>
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();

        double score;
        int gravity = 8;
        bool gameOver;
        Rect flappyBirdHitBox;


        public Window1()
        {


            InitializeComponent();

            gameTimer.Tick += MainEventTimer;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20); // how often we want the game to proceed
            StartGame();
        }

        private void MainEventTimer(object sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score; // show's score after each obstacle item crossed

            flappyBirdHitBox = new Rect(Canvas.GetLeft(flappyBird), Canvas.GetTop(flappyBird), flappyBird.Width - 5, flappyBird.Height); //determines the hitbox of Flappy Bird

            Canvas.SetTop(flappyBird, Canvas.GetTop(flappyBird) + gravity); // brings flappybird item along with the gravity definition

            if (Canvas.GetTop(flappyBird) < -10 || Canvas.GetTop(flappyBird) > 458) // endsgame if user goes off screen
            {
                EndGame();
            }

            foreach (var x in MyCanvas.Children.OfType<Image>())
            {
                if ((string)x.Tag == "obs1" || (string)x.Tag == "obs2" || (string)x.Tag == "obs3") //set pipe speed and spacing
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 5);

                    if (Canvas.GetLeft(x) < -100)
                    {
                        Canvas.SetLeft(x, 800);

                        score += .5; // obtain score for each obstacle item passed
                    }

                    Rect pipeHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height); // gives htibox for obstacles

                    if (flappyBirdHitBox.IntersectsWith(pipeHitBox)) // if user intersects item hitbox sends method to call end
                    {
                        EndGame();
                    }
                }

                if ((string)x.Tag == "cloud") //cloud item definition
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 2); // cloud item speed

                    if (Canvas.GetLeft(x) < -250) // resets cloud postion after off screen
                    {
                        Canvas.SetLeft(x, 550);
                    }
                }
            }
        }
        /// <summary>
        /// Key bind for application
        /// Space to Rise
        /// R to restart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                flappyBird.RenderTransform = new RotateTransform(-20, flappyBird.Width / 2, flappyBird.Height / 2); // rotates bird slighlty up to signify it is going up
                gravity = -8; // bird will go up through this definition
            }

            if (e.Key == Key.R && gameOver == true) // resets game
            {
                StartGame();
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            flappyBird.RenderTransform = new RotateTransform(5, flappyBird.Width / 2, flappyBird.Height / 2); // roates bird downward to signify it's declining
            gravity = 8;
        }
        /// <summary>
        /// Starts application
        /// </summary>
        private void StartGame()
        {
            MyCanvas.Focus();

            int temp = 300;

            score = 0;

            gameOver = false;

            Canvas.SetTop(flappyBird, 190); // default position once started

            foreach (var x in MyCanvas.Children.OfType<Image>()) // any item apart of the game will have a loop 
            {
                if ((string)x.Tag == "obs1") // first pipe item obstacle
                {
                    Canvas.SetLeft(x, 500);
                }
                if ((string)x.Tag == "obs2") // second pipe obstacle
                {
                    Canvas.SetLeft(x, 800);
                }
                if ((string)x.Tag == "obs3") // third pipe obstacle
                {
                    Canvas.SetLeft(x, 1100);
                }
                if ((string)x.Tag == "cloud") // background cloud item
                {
                    Canvas.SetLeft(x, 300 + temp);
                    temp = 800;
                }
            }

            gameTimer.Start();

        }
        /// <summary>
        /// End's game run time, prompts user to restart
        /// </summary>
        private void EndGame()
        {
            gameTimer.Stop();
            gameOver = true;
            txtScore.Content += " Game Over, Press R to Restart";
        }


    }
}
