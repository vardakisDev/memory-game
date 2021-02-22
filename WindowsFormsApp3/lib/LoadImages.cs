using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3.lib
{
    public static class LoadImages
    {

        //return all the images thar are in the recourses
        public static IEnumerable<Bitmap> getImages()

        {
            //if the user has selected images display return those
            if (Form1.selectedImages.Count > 0)
            {
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
                    Properties.Resources.pic10

            };
        }

    }
}
