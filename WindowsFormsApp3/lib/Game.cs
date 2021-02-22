using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace WindowsFormsApp3.lib
{

    class Game:INotifyPropertyChanged
    {
        public int time = 60;
        public int countTry = 0;

        private string _countClicks = "0";

        public string countClicks {
            get
            {
                return _countClicks;
            }
            set
            {
                if (value == countClicks) return;
                _countClicks = value;
                OnPropretyChanged("countClicks");
            }
                }

        private string _timeLabel;
        public string timeLabel {
            get { return _timeLabel; }
            set
            {
                if (value == timeLabel) return;
                _timeLabel = value;
                OnPropretyChanged("timeLabel");
            }
        } 



        //the timer will count from 60 sec and go down each sec 
        public Timer timer;
        public Timer clickTimer;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropretyChanged(string propertyName)
        {
            if(PropertyChanged !=null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Game()
        {
            timer =  new Timer{ Interval = 1000 };
            clickTimer = new Timer();
            timeLabel = "00:" + time.ToString();
        }

        //this timer will start the game and discrement by one each time the time ticks
        public void startGameTimer()
        {
            timer.Start();
            Console.WriteLine("STARTGAMETIMER");
            timer.Tick += delegate
            {
                time--;
                if (time < 0)
                {
                    timer.Stop();
                    MessageBox.Show("Out of time");
                    ResetC_T();
                    this.enableBtnPlay();
                    
                }
                var ssTime = TimeSpan.FromSeconds(time);
                timeLabel = "00:" + time.ToString();
            };
        }


        public void ResetC_T()
        {
            timer.Stop();
            time = 60;
            timeLabel = "00:60";
            countClicks = "0";

        }

        //hide all images allow images to be click and 
        public void CLICKTIMER_TICK(object sender, EventArgs e)
        {
            
            var x = (Form2)Application.OpenForms["Form2"];
            x.HideImages();
            x.allowClick = true;
            countTry++;
            countClicks = countTry.ToString();
            //we stop the clickTimer because we want the picture first cliked (first guess of the user ) to be displayed
            clickTimer.Stop();
        }

        public void startGame()
        {
            //start the game timer 
            startGameTimer();
            //set the interval of the clickTimer to 0.5 sec
            clickTimer.Interval = 500;
            //clicktimer on click
            clickTimer.Tick += CLICKTIMER_TICK;
            this.enableBtnPlay();
        }

        private void enableBtnPlay()
        {
            var x = (Button)Application.OpenForms["Form2"].Controls.Find("button1", false).FirstOrDefault();
            x.Enabled = true;
        }
    }

}
