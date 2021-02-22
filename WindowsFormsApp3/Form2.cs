using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form2 : Form
    {

        int time = 60;
        int countTry = 0;
        Random rand = new Random();
        bool allowClick = false;
        PictureBox firstGuess;
        //the timer will count from 60 sec and go down each sec 
        Timer timer = new Timer { Interval = 1000 };
        Timer clickTimer = new Timer();


        public Form2()
        {
            InitializeComponent();
            
            foreach (var item in Form1.selectedImages)
            {
                Console.WriteLine(item);
            }

        }

        //return all pictures boxes that exisit in this form
        private PictureBox[] pictureBoxes
        {
            get 
            {
                return Controls.OfType<PictureBox>().ToArray(); 
            }
        }


        //return all the images thar are in the recourses
        private static IEnumerable<Bitmap> images
        {
            get

            {  
                //if the user has selected images display return those
                if (Form1.selectedImages.Count > 0) {
                    List<Bitmap> images = new List<Bitmap>();
                    foreach (var item in Form1.selectedImages)
                    {
                        images.Add(new Bitmap(item));
                    }
                    return images;
                }
                //return the already loaded pictures from recourses
                return new Bitmap[]
                {
                    Properties.Resources.pic1,
                    Properties.Resources.pic2,
                    Properties.Resources.pic3,
                    Properties.Resources.pic4,
                    Properties.Resources.pic5,
                    Properties.Resources.pic6,
                    Properties.Resources.pic7,
                    Properties.Resources.pic8,
                    Properties.Resources.pic9,
                   /* Properties.Resources.pic10*/

                };
            }
        }

    

        //this timer will start the game and discrement by one each time the time ticks
        private void startGameTimer()
        {
            timer.Start();
            Console.Write("STARTGAMETIMER");
            timer.Tick += delegate
            {
                time--;
                if (time < 0)
                {
                    timer.Stop();
                    MessageBox.Show("Out of time");
                    ResetImage();
                }
                var ssTime = TimeSpan.FromSeconds(time);
                label1.Text = "00:" + time.ToString();
            };
        }
        //reset all images
        private void ResetImage()
        {
            foreach (var pic in pictureBoxes)
            {
                pic.Tag = null;
                pic.Visible = true;

            }
            HideImages();
            setRandomImages();

        }
        private void ResetGame()
        {
            ResetImage();
            time = 60;
            timer.Start();
        }
        

        //makes all pictures visible as question mark h
        private void HideImages()
        {
            
            foreach (var pic in pictureBoxes)
            {
                pic.Image = Properties.Resources.h;
            }
        }

        //returns a random picturebox which hasnt been assigned a picture yet
        private  PictureBox getFreeSlot()
        {
            //search the pictureboxes are array for any picture box that hast been assigned a tag which is an object containing the picture to display
            var box = pictureBoxes.Where(x => x.Tag == null).ToList();
            //select one random of those
            int index = rand.Next(0, box.Count()-1);
            //return it
            return box.ElementAt(index);
            
        }
        //find two free slots and input random image on them both
        private void setRandomImages()
        {
            int count = 0;
            Console.WriteLine("setting Random image");
            foreach (var image in images)
            {
                count++;
                Console.WriteLine(count);
                if (count == 9)
                {
                    Console.WriteLine("exiting");
                    return;
                }
                //after we get the selected pic we choose to random generated slot probided by getfreeslot to assing the picture to them
                getFreeSlot().Tag = image;
                getFreeSlot().Tag = image;

            }
            return;
        }


        //hide all images allow images to be click and 
        private void CLICKTIMER_TICK(object sender ,EventArgs e)
        {
            HideImages();
            allowClick = true;
            countTry++;
            label2.Text = countTry.ToString();
            //we stop the clickTimer because we want the picture first cliked (first guess of the user ) to be displayed
            clickTimer.Stop();
            
        }

        private async void clickImage(object sender , EventArgs e)
        {
            //if he has already clicked
            if (!allowClick) { Console.Write("Hey i cant be clicked"); return; }
            var pic = (PictureBox)sender;
            //if the player hasnt open a card yet set the card he clicked as the first guess and retrun
            if(firstGuess == null)
            {
                firstGuess = pic;
                pic.Image = (Image)pic.Tag;
                return;
            }

            //if the firstguess is not null which means a picture is already clicked set the picture send oover the click event to display
            //set the image he pressed on to display the image contained in tag and not the questiion mark pic
            pic.Image = (Image)pic.Tag;

            //if the image clicked is the same as the firstguess of the user and the image clicked isnt the samme as the first guessed make both images disapear
            if(pic.Image == firstGuess.Image && pic != firstGuess)
            {   
                //introduce a small delay to show first the image clicked by the user because if the image is matched correctly both of them will go invisible in 0 sec
                await Task.Delay(200);
                pic.Visible = firstGuess.Visible = false;
                HideImages();
            }
            //else if the user fails to match the first guess with the current pic he clicked start the timer again reset the opened images and set first guess as null
            else
            {
                allowClick = false;
                clickTimer.Start();
            }
            //reset first guess
            firstGuess = null;
            
            if (pictureBoxes.Any(p => p.Visible)) return;
            MessageBox.Show("You win now try again");
            button1.Enabled = true;
            timer.Stop();
            ResetGame();

        }


        private void startGame(object sender, EventArgs e)
        {
            
            allowClick = true;
            //radnomly assing the pics to the picture boxes avaiable
            setRandomImages();
            //hide all the images
            HideImages();
            //start the game timer 
            startGameTimer();
            //set the interval of the clickTimer to 0.5 sec
            clickTimer.Interval = 500;
            //clicktimer on click
            clickTimer.Tick += CLICKTIMER_TICK;
            button1.Enabled = false;

        }
    }
}
