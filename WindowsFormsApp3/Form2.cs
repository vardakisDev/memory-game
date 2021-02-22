using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp3.lib;

namespace WindowsFormsApp3
{
    public partial class Form2 : Form
    {

        Random rand = new Random();
        public bool allowClick = false;
        PictureBox firstGuess;
        //the timer will count from 60 sec and go down each sec 

        private Game game;
        private IEnumerable<Bitmap> images;
        private  string username ;
        public Form2(string text)
        {
            InitializeComponent();
            game = new Game();
            images = LoadImages.getImages();
            username = text;
            SetUp();
        }

        protected void BindControls()
        {
            label2.DataBindings.Add(new Binding("Text", game, "countClicks", false, DataSourceUpdateMode.OnPropertyChanged));
            label1.DataBindings.Add(new Binding("Text", game, "timeLabel",false,DataSourceUpdateMode.OnPropertyChanged));
        }


        private void SetUp()
        {
            BindControls();
        }

        private void startGame(object sender, EventArgs e)
        {
            allowClick = true;
            //radnomly assing the pics to the picture boxes avaiable
            game.ResetC_T();
            ResetImage();
            game.startGame();
            button1.Enabled = false;
        }



        //return all pictures boxes that exisit in this form
        public  PictureBox[] pictureBoxes
        {
            get
            {
                return Controls.OfType<PictureBox>().ToArray();
            }
        }


        private void ResetImage()
        {
            foreach (var pic in pictureBoxes)
            {
                pic.Tag = null;
                pic.Enabled = true;

            }
            HideImages();
            setRandomImages();

        }
        public void ResetGame()
        {
            ResetImage();
            game.ResetC_T();
        }
        

        //makes all pictures visible as question mark h
        public  void HideImages()
        {
            
            foreach (var pic in pictureBoxes.Where(x => x.Enabled ==true))
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



        private async void clickImage(object sender , EventArgs e)
        {
            //if he has already clicked
            if (!allowClick) { Console.Write("Hey i cant be clicked"); return; }
            var pic = (PictureBox)sender;


            //if the player hasnt open a card yet set the card he clicked as the first guess and retrun
            if(firstGuess == null)
            {
                firstGuess = pic;
                pic.Image = (System.Drawing.Image)pic.Tag;
                return;
            }

            //if the firstguess is not null which means a picture is already clicked set the picture send oover the click event to display
            //set the image he pressed on to display the image contained in tag and not the questiion mark pic
            pic.Image = (System.Drawing.Image)pic.Tag;

            //if the image clicked is the same as the firstguess of the user and the image clicked isnt the samme as the first guessed make both images disapear
            if(pic.Image == firstGuess.Image && pic != firstGuess)
            {   
                //introduce a small delay to show first the image clicked by the user because if the image is matched correctly both of them will go invisible in 0 sec
                await Task.Delay(200);
                pic.Enabled = firstGuess.Enabled = false;
                HideImages();
            }
            //else if the user fails to match the first guess with the current pic he clicked start the timer again reset the opened images and set first guess as null
            else
            {
                allowClick = false;
                game.clickTimer.Start();
            }


            //reset first guess
            firstGuess = null;
            
            if (pictureBoxes.Any(p => p.Enabled==true)) return;

            MessageBox.Show("You win now try again");
            this.saveScore();
            button1.Enabled = true;
            game.timer.Stop();
            game.ResetC_T();

        }
        private void saveScore()
        {
            User user = new User
            {
                name = username,
                timesClicked = Int32.Parse(label2.Text),
                timeToBeat = label1.Text
            };

            Db.SaveToDB(user.name, user.timesClicked, user.timeToBeat);
        }


    }


}
