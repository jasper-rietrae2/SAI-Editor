using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Updater
{
    public partial class Updater : Form
    {
        private const string BaseRemotePath = "http://dl.dropbox.com/u/84527004/SAI-Editor/";
        private const string BaseRemoteDownloadPath = "http://dl.dropbox.com/u/84527004/SAI-Editor/"; // 21676524les
        private readonly string _baseDir;
        readonly List<string> _files = new List<string>();

        public Updater()
        {
            InitializeComponent();
            _baseDir = Directory.GetCurrentDirectory();
        }

        private void UpdaterLoad(object sender, EventArgs e)
        {
            statusLabel.Text = "LOADING";
            var cl = new WebClient();

            // Changelog
            try
            {
                Stream st = cl.OpenRead(BaseRemotePath + "news.txt");
                if (st != null)
                {
                    var rd = new StreamReader(st);

                    string content = rd.ReadToEnd();
                    changelog.Text = content;
                    st.Close();
                    rd.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load news. Error: \r\n\r\n" + ex.Message);
                changelog.Text = "Unable to load news";
                statusLabel.Text = "UNABLE TO LOAD";
                statusLabel.ForeColor = Color.Red;
            }

            // Filelist
            //try
            {
                Stream str = cl.OpenRead(BaseRemotePath + "filelist.txt");
                if (str != null)
                {
                    var rdr = new StreamReader(str);

                    string currentLine;
                    while ((currentLine = rdr.ReadLine()) != null)
                    {
                        // string filename = currentLine;
                        // 
                        string filename = currentLine.Split(',')[0];
                        string md5 = currentLine.Split(',')[1];

                        if (File.Exists(_baseDir.ToString(CultureInfo.InvariantCulture) + @"\" + filename))
                        {
                            if (md5 != GetMD5HashFromFile(_baseDir + @"\" + filename))
                                _files.Add(filename);
                        }
                        else
                        {
                            _files.Add(filename);
                        }
                    }
                    filelist.DataSource = _files;
                    str.Close();
                    rdr.Close();
                }
            }
            //catch (Exception exe)
            //{
            //    MessageBox.Show("Unable to load filelist. Error: \r\n\r\n" + exe.Message);
            //    changelog.Text = "Unable to load filelist";
            //    statusLabel.Text = "UNABLE TO LOAD";
            //    statusLabel.ForeColor = Color.Red;
            //    return;
            //}

            statusLabel.Text = _files.Count > 0 ? "UPDATES AVAILABLE" : "ALREADY UP TO DATE";
        }

        private void Button1Click(object sender, EventArgs e)
        {
            progressBar.Visible = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void BackgroundWorker1DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (string file in _files)
            {
                string[] subfolder = file.Split(Char.Parse(@"\"));
                string currentFolder = _baseDir;

                foreach (string folder in subfolder)
                {
                    if (folder.Contains("."))
                        continue;

                    if (!Directory.Exists(_baseDir.ToString(CultureInfo.InvariantCulture) + @"\" + folder))
                        Directory.CreateDirectory(currentFolder + @"\" + folder);

                    currentFolder = currentFolder + @"\" + folder;
                }

                string remotefile = BaseRemoteDownloadPath + file;
                string destfile = _baseDir + @"\" + file;

                try
                {
                    var client = new WebClient();
                    client.DownloadFile(remotefile, destfile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Update Failed. Error: " + ex.Message);
                }
                progressBar.Value += (100 / _files.Count);
            }
            MessageBox.Show("Updated completed succesfully. \r\nYou can run LES the latest version of LES.");
            filelist.DataSource = null;
            filelist.Items.Clear();
        }

        protected string GetMD5HashFromFile(string fileName)
        {
            var file = new FileStream(fileName, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();

            var sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
