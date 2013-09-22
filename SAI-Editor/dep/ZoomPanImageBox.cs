/* 
 * Developed by Shannon Young.  http://www.smallwisdom.com
 * Copyright 2005
 * 
 * You are welcome to use, edit, and redistribute this code.
 * 
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using SAI_Editor.dep;

namespace Smallwisdom.Windows.Forms
{
	/// <summary>
	/// ZoomPanImageBox is a specialized ImageBox with Pan and Zoom control.
	/// </summary>
	public class ZoomPanImageBox : System.Windows.Forms.UserControl
	{
		/// <summary>
		/// The zoom factor for this control.  Currently, it is hardcoded, 
		/// but perhaps a nice addition would be to set this?
		/// </summary>
		private double[] zoomFactor = {.25, .33, .50, .66, .80, 1, 1.25, 1.5, 2.0, 2.5, 3.0};
		private System.Windows.Forms.Panel imagePanel;
		private ScrollablePicturebox imgBox;
        private float zoomValue = 2;
        Size origImgSize;

        // zoom controls

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region Construct, Dispose

		public ZoomPanImageBox()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Initialize anything not included in the designer
			init();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.imgBox = new ScrollablePicturebox();
            this.imagePanel = new System.Windows.Forms.Panel();
            this.imagePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgBox
            // 
            this.imgBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imgBox.Location = new System.Drawing.Point(0, 0);
            this.imgBox.Name = "imgBox";
            this.imgBox.Size = new System.Drawing.Size(349, 249);
            this.imgBox.TabIndex = 6;
            this.imgBox.TabStop = false;
            // 
            // imagePanel
            // 
            this.imagePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imagePanel.AutoScroll = true;
            this.imagePanel.Controls.Add(this.imgBox);
            this.imagePanel.Location = new System.Drawing.Point(0, 0);
            this.imagePanel.Name = "imagePanel";
            this.imagePanel.Size = new System.Drawing.Size(352, 252);
            this.imagePanel.TabIndex = 7;
            this.imagePanel.BackColor = Color.White;
            // 
            // ZoomPanImageBox
            // 
            this.Controls.Add(this.imagePanel);
            this.Name = "ZoomPanImageBox";
            this.Size = new System.Drawing.Size(355, 252);
            this.imagePanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Initialization code goes here.
		/// </summary>
		private void init()
		{
			// Add keydown event handler to check if this is a Ctrl+ or Ctrl-
			// If so, then it will change the zoom scroll.
			this.KeyDown += new KeyEventHandler(ImageBoxPanZoom_KeyDown);
            zoomValue = 2.0f;
		}

		/// <summary>
		/// Image loaded into the box.
		/// </summary>
		[Browsable(true),
		Description("Image loaded into the box.")]
		public Image Image
		{
			get
			{
				return imgBox.Image;
			}
			set
			{
				// Set the image value
				imgBox.Image = value;
				// enable the zoom control if this is not a null image
				if (value != null)
				{
                    origImgSize = value.Size;
                    zoomValue = (float)imgBox.Size.Height / value.Size.Height;
                    setZoom();
				}
				else
				{
					// If null image, then reset the imgBox size
					// to the size of the panel so that there are no
					// scroll bars.
					imgBox.Size = imagePanel.Size;
				}
			}
		}

		private void ImageBoxPanZoom_KeyDown(object sender, KeyEventArgs e)
		{
			// Was the key combination that was pressed Ctrl+ or Ctrl-?
			// If so, then change the zoom level
			// Note: The e.KeyData is the combination of all the
			// keys currently pressed down.  To find out if this is
			// the Ctrl key *and* the + key, you "or" the Keys 
			// together. This is a bitwise "or" rather than the 
			// || symbol used for boolean logic.

			if (e.KeyData == (Keys.Oemplus | Keys.Control))
			{
				zoomValue = zoomValue / 0.5f;
				setZoom();
			}
			else if (e.KeyData == (Keys.OemMinus | Keys.Control))
			{
                zoomValue = zoomValue * 0.5f;
				setZoom();
			}
		}

		private void setZoom()
		{
            if (imgBox.Image == null)
                return;
			// The scrollZoom changed so reset the zoom factor
			// based on the scrollZoom TrackBar position.
			// double newZoom = zoomFactor[zoomValue];
            double newZoom = zoomValue;

			// Set the ImageBox width and height to the new zoom
			// factor by multiplying the Image inside the Imagebox
			// by the new zoom factor.
            imgBox.Zoom(Convert.ToInt32(origImgSize.Width * newZoom), Convert.ToInt32(origImgSize.Height * newZoom), ref zoomValue); 
		}

	}// end class
}// end namespace
