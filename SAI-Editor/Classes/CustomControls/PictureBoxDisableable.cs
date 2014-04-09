using SAI_Editor.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SAI_Editor.Classes.CustomControls
{
    public class PictureBoxDisableable : PictureBox
    {
        private string _resourceImageStr;

        [Category("Misc")]
        public string ResourceImageStr
        {
            get
            {
                return _resourceImageStr;
            }
            set
            {
                _resourceImageStr = value;
            }
        }

        public new bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = value;

                try
                {
                    Bitmap originalImage = new Bitmap(Resources.ResourceManager.GetObject(_resourceImageStr) as Bitmap);

                    if (!value)
                    {
                        for (int w = 0; w < originalImage.Width; w++)
                        {
                            for (int h = 0; h < originalImage.Height; h++)
                            {
                                Color pixelColor = originalImage.GetPixel(w, h);

                                //! We only set the color to gray/white ish with an alpha effect on pixels containing an
                                //! actual color. Else it looks really weird and not actually disabled at all.
                                if (pixelColor.A != 0 && pixelColor.B != 0 && pixelColor.G != 0)
                                    originalImage.SetPixel(w, h, Color.FromArgb(70, pixelColor));
                            }
                        }
                    }
                    Image = originalImage;
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
