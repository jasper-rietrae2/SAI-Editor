using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Updater
{
    public partial class Updater : Form
    {
        private const string baseRemotePath = "http://dl.dropbox.com/u/84527004/SAI-Editor/";
        private const string baseRemoteDownloadPath = "http://dl.dropbox.com/u/84527004/SAI-Editor/SAI-Editor/";
        private readonly string applicationVersion = String.Empty;
        readonly List<string> _files = new List<string>();

        public Updater()
        {
            InitializeComponent();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            applicationVersion = "v" + version.Major + "." + version.Minor;
            Text = "SAI-Editor " + applicationVersion + ": Updater";
        }

        //! Start the initial search after a second (button is disabled) so the form finishes loading.
        private void timerStartSearchingOnLaunch_Tick(object sender, EventArgs e)
        {
            timerStartSearchingOnLaunch.Enabled = false;
            CheckForUpdates();
        }

        private void buttonCheckForUpdates_Click(object sender, EventArgs e)
        {
            buttonCheckForUpdates.Enabled = false;
            buttonCheckForUpdates.Update();
            buttonUpdateToLatest.Enabled = false;
            buttonUpdateToLatest.Update();
            listBoxFilesToUpdate.DataSource = null;
            listBoxFilesToUpdate.Update();
            _files.Clear();
            changelog.Text = String.Empty;

            CheckForUpdates();
            buttonCheckForUpdates.Enabled = true;
            buttonUpdateToLatest.Enabled = _files.Count > 0;
        }

        private void CheckForUpdates()
        {
            statusLabel.Text = "CHECKING FOR UPDATES...";
            statusLabel.Update();
            progressBar.Maximum = GetFilesCountInFileList();
            progressBar.Value = 0;

            using (WebClient client = new WebClient())
            {
                try
                {
                    Stream streamFileList = client.OpenRead(baseRemotePath + "filelist.txt");

                    if (streamFileList != null)
                    {
                        StreamReader streamReaderFileList = new StreamReader(streamFileList);
                        string currentLine = String.Empty;

                        while ((currentLine = streamReaderFileList.ReadLine()) != null)
                        {
                            string[] splitLine = currentLine.Split(',');
                            string filename = splitLine[0];
                            string md5 = splitLine[1];
                            progressBar.Value++;

                            if (File.Exists(Directory.GetCurrentDirectory().ToString(CultureInfo.InvariantCulture) + @"\" + filename))
                            {
                                if (md5 != GetMD5HashFromFile(Directory.GetCurrentDirectory() + @"\" + filename))
                                    _files.Add(filename);
                            }
                            else
                                _files.Add(filename);
                        }

                        listBoxFilesToUpdate.DataSource = _files;
                        streamFileList.Close();
                        streamReaderFileList.Close();
                    }
                }
                catch (Exception exe)
                {
                    MessageBox.Show("Unable to load filelist. Error: \r\n\r\n" + exe.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    changelog.Text = "Unable to load filelist";
                    statusLabel.Text = "UNABLE TO LOAD";
                    statusLabel.ForeColor = Color.Red;
                    return;
                }

                if (_files.Count > 0)
                {
                    try
                    {
                        Stream streamNews = client.OpenRead(baseRemotePath + "news.txt");

                        if (streamNews != null)
                        {
                            using (StreamReader streamReaderNews = new StreamReader(streamNews))
                            {
                                string content = streamReaderNews.ReadToEnd();
                                changelog.Text = content;
                                streamNews.Close();
                                streamReaderNews.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unable to load news. Error: \r\n\r\n" + ex.Message, "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        changelog.Text = "Unable to load news";
                        statusLabel.Text = "UNABLE TO LOAD";
                        statusLabel.ForeColor = Color.Red;
                    }
                }
            }

            statusLabel.Text = _files.Count > 0 ? "UPDATES AVAILABLE" : "ALREADY UP TO DATE";
            progressBar.Value = 0;
            buttonUpdateToLatest.Enabled = _files.Count > 0;
            buttonCheckForUpdates.Enabled = true;
        }

        private void buttonUpdateToLatest_Click(object sender, EventArgs e)
        {
            progressBar.Value = 0;
            progressBar.Maximum = _files.Count;
            buttonUpdateToLatest.Enabled = false;
            buttonUpdateToLatest.Update();

            buttonCheckForUpdates.Enabled = false;
            buttonCheckForUpdates.Update();

            for (int i = 0; i < _files.Count; ++i)
            {
                string file = _files[i];
                progressBar.Value++;

                if (file.Contains("filelist.txt") || file.Contains("news.txt"))
                    continue;

                string[] subfolder = file.Split(Char.Parse(@"\"));
                string currentFolder = Directory.GetCurrentDirectory();

                foreach (string folder in subfolder)
                {
                    if (Path.HasExtension(folder))
                        continue;

                    if (!Directory.Exists(Directory.GetCurrentDirectory().ToString(CultureInfo.InvariantCulture) + @"\" + folder))
                        Directory.CreateDirectory(currentFolder + @"\" + folder);

                    currentFolder = currentFolder + @"\" + folder;
                }

                string remotefile = baseRemoteDownloadPath + file;
                string destfile = Directory.GetCurrentDirectory() + @"\" + file;

                try
                {
                    using (WebClient client = new WebClient())
                        client.DownloadFile(remotefile, destfile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Update Failed. Error: " + ex.Message, "Someting went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            progressBar.Value = 0;
            listBoxFilesToUpdate.DataSource = null;
            listBoxFilesToUpdate.Items.Clear();
            _files.Clear();
            buttonCheckForUpdates.Enabled = true;
            MessageBox.Show("Updated completed succesfully.\r\nYou can now run the latest version of SAI-Editor. Closing the updater will automatically open the SAI-Editor.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        protected string GetMD5HashFromFile(string fileName)
        {
            byte[] retVal = null;

            try
            {
                using (FileStream file = new FileStream(fileName, FileMode.Open))
                {
                    MD5 md5 = new MD5CryptoServiceProvider();
                    retVal = md5.ComputeHash(file);
                    file.Close();
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "The MD5 hash of the following file could not be read as reading the file threw an exception:\n\n" + fileName + "\n\nError message:\n" + ex.Message;
                MessageBox.Show(errorMessage, "Someting went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (retVal == null)
                return String.Empty;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < retVal.Length; i++)
                sb.Append(retVal[i].ToString("x2"));

            return sb.ToString();
        }

        private void changelog_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true; //! Don't allow writing to the changelog rich textbox
        }

        private void Updater_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Process.Start(Directory.GetCurrentDirectory() + "\\SAI-Editor.exe");
            }
            catch (Exception)
            {
                MessageBox.Show("The SAI-Editor could not be opened. Please do so manually.", "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetFilesCountInFileList()
        {
            int countOfFiles = 0;

            using (WebClient client = new WebClient())
            {
                Stream streamFileList = client.OpenRead(baseRemotePath + "filelist.txt");

                if (streamFileList != null)
                {
                    StreamReader streamReaderFileList = new StreamReader(streamFileList);
                    string currentLine = String.Empty;

                    while ((currentLine = streamReaderFileList.ReadLine()) != null)
                    {
                        string filename = currentLine.Split(',')[0];

                        if (Path.HasExtension(filename))
                            countOfFiles++;
                    }
                }
            }

            return countOfFiles;
        }

        private void timerCheckForSaiEditorRunning_Tick(object sender, EventArgs e)
        {
            if (IsSaiEditorRunning())
            {
                timerCheckForSaiEditorRunning.Enabled = false;
                MessageBox.Show("There is an instance of SAI-Editor running at the moment. In order for the updater to work, this may not be the case. Please close it before continuing.", "SAI-Editor is running!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                timerCheckForSaiEditorRunning.Enabled = true;
            }
        }

        private bool IsSaiEditorRunning()
        {
            return Process.GetProcessesByName("SAI-Editor").Length > 0;
        }
    }
}
