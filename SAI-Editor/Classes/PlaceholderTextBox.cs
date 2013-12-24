using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAI_Editor.Classes
{
    public class PlaceholderTextBox : TextBox
    {
        string _placeHolderText;

        public string PlaceHolderText
        {
            get { return _placeHolderText; }
            set { _placeHolderText = value; }
        }

        public new string Text
        {
            get
            {
                if (ForeColor == Color.Gray)
                    return String.Empty;

                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (Text.Contains(_placeHolderText) && ForeColor == Color.Gray)
            {
                Text = String.Empty;
                ForeColor = Color.Black;
            }

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (String.IsNullOrEmpty(Text))
            {
                Text = _placeHolderText;
                ForeColor = Color.Gray;
            }

            base.OnLostFocus(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (String.IsNullOrEmpty(Text))
            {
                Text = _placeHolderText;
                ForeColor = Color.Gray;
            }

            base.OnLostFocus(e);
        }
    }
}
