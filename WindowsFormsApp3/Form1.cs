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

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        OpenFileDialog dialog = new OpenFileDialog();
        public static List<string> selectedImages = new List<string>();


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
            Form2 form2 = new Form2();
            form2.Show();

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
    }
}
