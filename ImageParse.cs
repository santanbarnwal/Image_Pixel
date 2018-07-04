using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace ImageParseActivity
{  


    public class ImageParse : CodeActivity
    {
        [Category("input"), DisplayName("Path")]
        public InArgument<String> Path { get; set; }
        [Category("Output"), DisplayName("Result")]
        public OutArgument<DataTable> dtResult { get; set; }

   
        protected override void Execute(CodeActivityContext context)
        {
            String path = Path.Get(context);
            dtResult.Set(context, ParseImageToCoOrdinate(path));
        }

        private DataTable ParseImageToCoOrdinate(String imgPath)
        {
            //String pat = imgPath;
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("X");
            dt.Columns.Add("Y");
            Bitmap bmp;
            try
            {
                bmp = new Bitmap(imgPath);
            }
            catch (Exception e)
            {
                throw new System.Exception("Image path is not valid");
            }

            //get image dimension
            int width = bmp.Width;
            int height = bmp.Height;

            //color of pixel
            Color p;

            //Converting to grayscale
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //get pixel value
                    p = bmp.GetPixel(x, y);

                    int a = p.A;
                    int r = p.R;
                    int g = p.G;
                    int b = p.B;

                    //find average of three colors
                    int avg = (r + g + b) / 3;

                    //set new pixel value
                    bmp.SetPixel(x, y, Color.FromArgb(a, avg, avg, avg));
                }
            }

            Color pixel;

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    pixel = bmp.GetPixel(i, j);
                    if (pixel.R < 20 && pixel.G < 20 && pixel.B < 20)
                    {
                        DataRow row = dt.NewRow();
                        row["X"] = i.ToString();
                        row["Y"] = j.ToString();
                        dt.Rows.Add(row);
                    }
                }
            }
            return dt;
        }
    }
}
