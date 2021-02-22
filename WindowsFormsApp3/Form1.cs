using Microsoft.WindowsAPICodePack.Dialogs;
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
    public partial class Form1 : Form
    {
        private OpenFileDialog dialog = new OpenFileDialog();
        public static List<string> selectedImages = new List<string>();
        private  DataTable dt;
        private  DataView dv;
        private List<User> users;
        private PictureBox[] pictureBoxes
        {
            get
            {
                return Controls.OfType<PictureBox>().ToArray();
            }
        }

        private void populatePictureBoxes()
        {

            for(int i =0; i< selectedImages.Count(); i++)
            {
                pictureBoxes.ElementAt(i).SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBoxes.ElementAt(i).Image = Bitmap.FromFile(selectedImages.ElementAt(i));
            }
        }
        private void clearPictureBoxes()
        {
            foreach (var item in pictureBoxes)
            {
                item.Image = null;
            }
        }


        public Form1()
        {
            InitializeComponent();
            label3.Visible = false;
            label4.Visible = false;
            users = Db.ReadHighScores();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dialog.Multiselect = true;
            dialog.Filter = "Image Files(*.jpg,*.jpeg,*.png)| *.jpg;*.jpeg;*.png;";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if(dialog.FileNames.Length != 8)
                {
                    MessageBox.Show("Please select 8 pictures");
                    return;
                }
                var dirs = dialog.FileNames.ToList();
                foreach (var item in dirs)
                {
                    selectedImages.Add(item);
                }
                SelectImagesLabels();
                populatePictureBoxes();
               
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                Form2 form2 = new Form2(textBox1.Text);
                form2.Show();
            }
            else
            {
                MessageBox.Show("Input a user name");
            }

        }

        private void label4_Click(object sender, EventArgs e)
        {
            selectedImages.Clear();
            clearPictureBoxes();
            SelectImagesLabels();
        }

        private void SelectImagesLabels()
        {
            if(selectedImages.Count > 0)
            {
                label3.Visible = true;
                label4.Visible = true; ;
                return;
            }
            label3.Visible = false;
            label4.Visible = false;
            return;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            iniateListView();
            FillDataTable();
            dv = new DataView(dt);
            PopulateTableView(dv);
        }

        private void iniateListView()
        {
            listView1.View = View.Details;

            listView1.Columns.Add("username", 75);
            listView1.Columns.Add("score", 75);
            listView1.Columns.Add("date", 110);

            dt = new DataTable();
            dt.Columns.Add("Username");
            dt.Columns.Add("Score");
            dt.Columns.Add("Date");
        }

        public void PopulateTableView(DataView dv)
        {
            listView1.Items.Clear();
            foreach (DataRow row in dv.ToTable().Rows)
            {
                listView1.Items.Add(new ListViewItem(new String[] { row[0].ToString(), row[1].ToString(), row[2].ToString() }));
            }



        }

        private void FillDataTable()
        {   
            foreach (var item in users)
            {
                dt.Rows.Add(item.name, item.timesClicked, item.date_time);
            }

        }
    }
}
