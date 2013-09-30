using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAI_Editor.Classes
{
    public class XToolTip : ToolTip
    {
        public void SetToolTipText(Control control, string caption)
        {
            string newCaption = "";

            if (caption.Length > 80)
            {
                string[] splitCaption = caption.Split(Convert.ToChar(" "));

                int totalLength = 0;
                List<string> wordsSoFar = new List<string>();
                List<List<string>> captionChunks = new List<List<string>>();

                for (int i = 0; i < splitCaption.Length; ++i)
                {
                    //! +1 because of the whitespace that was lost in the Split call.
                    totalLength += splitCaption[i].Length + 1;
                    wordsSoFar.Add(splitCaption[i]);

                    if (totalLength > 60 || i == splitCaption.Length - 1)
                    {
                        List<string> wordsSoFarCopy = new List<string>(wordsSoFar);
                        captionChunks.Add(wordsSoFarCopy);
                        wordsSoFar.Clear();
                        totalLength = 0;
                    }
                }

                foreach (List<string> listString in captionChunks)
                {
                    foreach (string strPart in listString)
                        newCaption += strPart + " ";

                    newCaption += "\n";
                }
            }
            else
                newCaption = caption;

            //! We can't use this because it will cut off words, which is exactly what we're trying to avoid.
            //List<string> strParts = new List<string>(Regex.Split(caption, @"(?<=\G.{60})", RegexOptions.Singleline));

            SetToolTip(control, newCaption);
        }
    }
}
