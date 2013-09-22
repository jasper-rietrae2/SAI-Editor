using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAI_Editor.dep
{
    public partial class ScrollablePicturebox : UserControl
    {
        private Image image;
        private bool centerImage;
        private float curScale;
        private Image origImage = null;

        public Image Image
        {
            get { return image; }
            set 
            { 
                if (origImage == null)
                    origImage = value;
                image = value;
                Invalidate(); 
            }
        }

        public bool CenterImage
        {
            get { return centerImage; }
            set { centerImage = value; Invalidate(); }
        }

        public ScrollablePicturebox()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            Image = null;
            curScale = 1;
            AutoScroll = true;
            AutoScrollMinSize = new Size(0, 0);
        }

        protected Point clickPosition;
        protected Point scrollPosition;
        protected Point lastPosition;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            clickPosition.X = e.X;
            clickPosition.Y = e.Y;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            Cursor = Cursors.Default;
            lastPosition.X = AutoScrollPosition.X;
            lastPosition.Y = AutoScrollPosition.Y;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Cursor = Cursors.Hand;
                scrollPosition.X = clickPosition.X - e.X - lastPosition.X;
                scrollPosition.Y = clickPosition.Y - e.Y - lastPosition.Y;
                AutoScrollPosition = scrollPosition;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.FillRectangle(new Pen(BackColor).Brush, 0, 0, e.ClipRectangle.Width, e.ClipRectangle.Height);

            if (Image == null)
                return;

            int centeredX = AutoScrollPosition.X;
            int centeredY = AutoScrollPosition.Y;

            if (CenterImage)
            {
               //Something not relevant
            }

            AutoScrollMinSize = new Size(Image.Width, Image.Height);
            e.Graphics.DrawImage(Image, new RectangleF(centeredX, centeredY, Image.Width, Image.Height));
        }

        public void Zoom(int width, int height, ref float scale)
        {
            Image tmp = Scale(origImage, width, height, 1);
            if (tmp.Width < this.Width && tmp.Height < this.Height)
            {
                scale = scale / 0.5f;
                return;
            }
            Image = tmp;
            Invalidate();
        }

        public static Image Scale(Image img, int maxWidth, int maxHeight, double scale)
        {
            if (img.Width > maxWidth || img.Height > maxHeight)
            {
                double scaleW, scaleH;

                scaleW = maxWidth / (double)img.Width;
                scaleH = maxHeight / (double)img.Height;

                scale = scaleW < scaleH ? scaleW : scaleH;
            }

            return img.GetThumbnailImage((int)(img.Width * scale), (int)(img.Height * scale), null, IntPtr.Zero);
        }
    }
}
