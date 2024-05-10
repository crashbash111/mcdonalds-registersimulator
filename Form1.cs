using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            if (!Configurator.ReadConfigurationFile())
            {
                MessageBox.Show("There was an error loading the configuration file, or information is missing. Please fill in the conf.ini and try again.");
                Application.Exit();
            }
            //initCountButtons();
            TestButtonLoad();
            Configurator.ReadOutages(tmpButtons);
            if(Configurator.screenList.Count == 0)
            {
                MessageBox.Show("No screens were found. Please check the PosData and try again.");
                Application.Exit();
            }
            BringToFront();
            DoubleBuffered = true;
            MainScreen = 1;
            //ChangeActiveMenu(1);
        }


        Button[] numButtons = new Button[10];
        List<Button> tmpButtons = new List<Button>();
        List<Panel> screenPanels = new List<Panel>();
        public int loadingProgress;
        private int activeMainScreenID;
        private int activeFloatScreenID;
        private int previousMainScreenID;

        /*
        private void ChangeActiveMenu(int screenIndex)
        {
            foreach(Panel pan in screenPanels)
            {
                pan.Visible = false;
            }
            (Configurator.screenList.Find(x => x.number == screenIndex).panel as Panel).Visible = true;
        }
        */

        //static string baseDir = "./repository.1024x768/";

        public int MainScreen
        {
            get
            {
                return activeMainScreenID;
            }

            set
            {
                try
                {
                    Screen screenToLoad = Configurator.screenList.Find(x => x.number == value);
                    if (screenToLoad != null)
                    {
                        Panel b = screenToLoad.panel as Panel;
                        //checks screen type
                        Image img = FetchImg(screenToLoad.bg);
                        if (img != null)
                        {
                            BackgroundImage?.Dispose(); // Dispose the old background image if it exists
                            BackgroundImage = new Bitmap(img); // Create a new Bitmap from the fetched image
                        }
                        if (screenToLoad.type != 1000)
                        {
                            FloatMenu = -1;
                        }
                        //hides old screen, makes new screen visible
                        b.Visible = true;
                        Screen previousScreen = Configurator.screenList.Find(x => x.number == activeMainScreenID);
                        if (previousScreen != null)
                        {
                            (previousScreen.panel as Panel).Visible = false;
                        }
                        previousMainScreenID = activeMainScreenID;
                        activeMainScreenID = value;
                    }
                    else
                    {
                        Debug.Print("Screen with number " + value + " not found.");
                    }
                }
                catch
                {
                    Debug.Print("Error loading panel. You probably should fix this...");
                }
            }
        }

        public int FloatMenu
        {
            get
            {
                return activeFloatScreenID;
            }
            set
            {
                if (activeFloatScreenID != -1)
                {
                    Panel b = (Configurator.screenList.Find(x => x.number == activeFloatScreenID).panel as Panel);
                    b.Visible = false;
                    b.Size = panMainContent.Size;
                    b.Top = panMainContent.Top;
                }
                if(value != -1)
                {
                    Panel c = (Configurator.screenList.Find(x => x.number == value).panel as Panel);
                    c.Size = panFloatScreen.Size;
                    c.Top = panFloatScreen.Top;
                    c.BringToFront();
                    c.Visible = true;
                }
                    
                activeFloatScreenID = value;
            }
        }

        

        private string FetchImgDir(string imgName)
        {
            return Configurator.imgRepositoryPath + imgName;
        }

        private Image FetchImg(string imgName)
        {
            if(ImageExists(FetchImgDir(imgName)))
            {
                return Image.FromFile(FetchImgDir(imgName));
            }
            return null;
        }

        private void OpenLoadingForm()
        {
            LoadingForm z = new LoadingForm(this);
            z.ShowDialog();
        }

        private void TestButtonLoad()
        {
            Thread loadFormThread = new Thread(new ThreadStart(OpenLoadingForm));
            loadFormThread.IsBackground = true;
            loadFormThread.Start();
            loadingProgress = 0;

            foreach (Screen t in Configurator.screenList)
            {
                Panel pan = new Panel
                {
                    Top = panMainContent.Top,
                    Left = panMainContent.Left,
                    Height = panMainContent.Height,
                    Width = panMainContent.Width,
                    BackColor = panMainContent.BackColor,
                    Tag = t,
                    Visible = false
                };
                t.panel = pan;
                screenPanels.Add(pan);
                this.Controls.Add(pan);
            }

            foreach (RegisterButton x in Configurator.testImgList)
            {
                Button y = new Button();
                y.Tag = x;

                var screenPanel = screenPanels.Find(p => p.Tag as Screen == x.screen);
                if ((screenPanel.Tag as Screen).type == 1002)  // 1202 - floating, 1000 main, 1002 special
                {
                    y.Size = new Size(94 * x.h, 73 * x.w); //still needs 5 spacing accounted for
                    y.Top = ((x.number - 3) / 7) * 78;
                    int leftOffset = ((x.number - 3) % 7) * (94 + 5) + 5;
                    y.Left = x.h > 1 && x.number != 3 ? leftOffset : ((x.number - 3) % 7) * (y.Width + 5) + 5;
                }
                else
                {
                    y.Size = new Size(64 * x.h, 64 * x.w); //still needs 5 spacing accounted for
                    y.Top = (x.number / 10) * 70;
                    y.Left = ((x.number % 10) * (y.Width + 5)) + 5;
                }

                y.Click += Y_Click;
                y.MouseDown += Y_MouseDown;
                y.MouseUp += Y_MouseUp;
                Image buttonImage = FetchImg(x.imgup);
                if (buttonImage != null)
                {
                    y.BackgroundImage = buttonImage;
                    y.BackgroundImageLayout = ImageLayout.Stretch;
                    y.Text = "";
                    y.FlatStyle = FlatStyle.Flat;
                    y.FlatAppearance.BorderSize = 0;
                }
                else
                {
                    //no image available, use fallback text
                    Color backColor = Color.FromName(x.Bgup);
                    y.BackColor = backColor.IsKnownColor ? backColor : Color.White;

                    Color foreColor = Color.FromName(x.textup);
                    y.ForeColor = foreColor.IsKnownColor ? foreColor : Color.Black;

                    if (x.title != null)
                    {
                        y.Text = (x.title.Replace(@"\n", Environment.NewLine));
                    }
                }
                y.TabStop = false;
                
                screenPanel.Controls.Add(y);
                tmpButtons.Add(y);
                loadingProgress++;
            }
        }

        private void Y_MouseUp(object sender, MouseEventArgs e)
        {
            Button x = sender as Button;
            RegisterButton y = x?.Tag as RegisterButton;

            if (x != null && y != null && ImageExists(y.imgup))
            {
                x.BackgroundImage = FetchImg(y.imgup);
            }
        }

        private void Y_MouseDown(object sender, MouseEventArgs e)
        {
            Button x = sender as Button;
            RegisterButton y = x?.Tag as RegisterButton;

            if (x != null && y != null)
            {
                if (ImageExists(y.imgdn))
                {
                    x.BackgroundImage = FetchImg(y.imgdn);
                }
                else if (ImageExists(y.imgup))
                {
                    x.BackgroundImage = FetchImg(y.imgup);
                }
            }
        }

        private bool ImageExists(string imagePath)
        {
            // Implement this method to check if the image exists at the given path.
            // This is a placeholder implementation.
            return File.Exists(imagePath);
        }

        private void Y_Click(object sender, EventArgs e)
        {
            Button x = sender as Button;
            RegisterButton y = x.Tag as RegisterButton;
            foreach(string t in y.actionType)
            {
                switch (t)
                {
                    case "WF_ShowFloatScreen":
                        FloatMenu = y.location;
                        break;
                    case "WF_ShowScreen":
                    case "WF_ShowManagerMenu":
                        textScreenChange.Text = y.location.ToString();
                        break;
                    case "WF_HideFloatScreen":
                        FloatMenu = -1;
                        break;
                    case "WF_BackToPreviousScreen":
                        textScreenChange.Text = previousMainScreenID.ToString();
                        break;
                    default:
                        MessageBox.Show("Action Not Implemented: " + t);
                        break;
                }
            } 
        }

        private void initCountButtons()
        {
            for(int i = 0; i<10; i++)
            {
                numButtons[i] = new Button();
                numButtons[i].Size = new Size(64, 64);
                numButtons[i].Top = 0;
                numButtons[i].Left = (i * (numButtons[i].Width + 5)) + 5;
                try
                {
                    numButtons[i].Image = FetchImg("0" + i.ToString() + ".png");
                    numButtons[i].Text = "";
                    numButtons[i].TabStop = false;
                    numButtons[i].FlatStyle = FlatStyle.Flat;
                    numButtons[i].FlatAppearance.BorderSize = 0;
                }
                catch
                {
                    numButtons[i].Text = i.ToString();
                    numButtons[i].BackColor = Color.White;
                }
                panMainContent.Controls.Add(numButtons[i]);
            }
        }

        private void TextScreenChange_TextChanged(object sender, EventArgs e)
        {
            if(textScreenChange.Text != "")
            {
                try
                {
                    MainScreen = (int.Parse(textScreenChange.Text));
                }
                catch
                {
                    //not valid
                }
            }
        }
    }
}
