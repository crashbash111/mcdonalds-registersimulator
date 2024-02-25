using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
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
            MainScreen = 1;
            this.BringToFront();
            this.DoubleBuffered = true;
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
                    Panel b = (Configurator.screenList.Find(x => x.number == value).panel as Panel);
                    //checks screen type
                    Screen screenToLoad = b.Tag as Screen;
                    this.BackgroundImage = FetchImg(screenToLoad.bg);
                    if (screenToLoad.type != 1000)
                    {
                        FloatMenu = -1;
                    }
                    //hides old screen, makes new screen visible
                    b.Visible = true;
                    (Configurator.screenList.Find(x => x.number == activeMainScreenID).panel as Panel).Visible = false;
                    previousMainScreenID = activeMainScreenID;
                    activeMainScreenID = value;
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
            return Image.FromFile(FetchImgDir(imgName));
        }

        private void OpenLoadingForm()
        {
            LoadingForm z = new LoadingForm(this);
            z.ShowDialog();
        }

        private void TestButtonLoad(){
            Thread loadFormThread = new Thread(new ThreadStart(OpenLoadingForm));
            loadFormThread.IsBackground = true;
            loadFormThread.Start();
            loadingProgress = 0;
            foreach(Screen t in Configurator.screenList)
            {
                Panel pan = new Panel();
                pan.Top = panMainContent.Top;
                pan.Left = panMainContent.Left;
                pan.Height = panMainContent.Height;
                pan.Width = panMainContent.Width;
                pan.BackColor = panMainContent.BackColor;
                pan.Tag = t;
                t.panel = pan;
                pan.Visible = false;
                screenPanels.Add(pan);
                this.Controls.Add(pan);
            }
            foreach(RegisterButton x in Configurator.testImgList)
            {
                Button y = new Button();
                y.Tag = x;
                
                if ((screenPanels.Find(p => p.Tag as Screen == x.screen).Tag as Screen).type == 1002)  // 1202 - floating, 1000 main, 1002 special
                {
                    y.Size = new Size(94 * x.h, 73 * x.w); //still needs 5 spacing accounted for
                    y.Top = ((x.number - 3) / 7) * 78;
                    if (x.h>1)
                    {
                        if(x.number == 3)
                        {
                            //ignore off center if menu title
                            y.Left = ((x.number) % 7) * (94 + 5) + 5;
                        }
                        else
                        {
                            //if big button, adjust offset
                            y.Left = ((x.number - 3) % 7) * (94 + 5) + 5;
                        }
                    }
                    else
                    {
                        y.Left = ((x.number - 3) % 7) * (y.Width + 5) + 5;
                    }
                }
                else
                {
                    y.Size = new Size(64 * x.h, 64 * x.w); //still needs 5 spacing accounted for
                    y.Top = (x.number / 10) * 70;
                    y.Left = ((x.number % 10) * (y.Width + 5)) + 5;
                }
                
                try
                {
                    y.Click += Y_Click;
                    y.MouseDown += Y_MouseDown;
                    y.MouseUp += Y_MouseUp;
                    y.BackgroundImage = FetchImg(x.imgup);
                    y.BackgroundImageLayout = ImageLayout.Stretch;
                    y.Text = "";
                    y.TabStop = false;
                    y.FlatStyle = FlatStyle.Flat;
                    y.FlatAppearance.BorderSize = 0;
                    
                }
                catch
                {
                    if (x.title != null)
                    {
                        y.Text = (x.title.Replace(@"\n", Environment.NewLine));
                    }
                    
                    y.BackColor = Color.FromName(x.Bgup);
                    y.ForeColor = Color.FromName(x.textup);
                }
                screenPanels.Find(p => p.Tag as Screen == x.screen).Controls.Add(y);
                //if (x.screen.number == 1)
                //{
                //    panel2.Controls.Add(y);
                //}
                tmpButtons.Add(y);
                loadingProgress++;
            }
        }

        private void Y_MouseUp(object sender, MouseEventArgs e)
        {
            Button x = sender as Button;
            RegisterButton y = x.Tag as RegisterButton;
            try
            {
                x.BackgroundImage = FetchImg(y.imgup);
            }
            catch
            {
                //could not load normal image
            }
        }

        private void Y_MouseDown(object sender, MouseEventArgs e)
        {
            Button x = sender as Button;
            RegisterButton y = x.Tag as RegisterButton;
            try
            {
                x.BackgroundImage = FetchImg(y.imgdn);
            }
            catch
            {
                //could not load hover image
                try
                {
                    x.BackgroundImage = FetchImg(y.imgup);
                }
                catch
                {
                    //could not load fallback normal image. Ignoring
                }
            }
        }

        private void Y_Click(object sender, EventArgs e)
        {
            Button x = sender as Button;
            RegisterButton y = x.Tag as RegisterButton;
            foreach(string t in y.actionType)
            {
                if (t == "WF_ShowFloatScreen")
                {
                    FloatMenu = y.location;
                }
                else if (t == "WF_ShowScreen" || t == "WF_ShowManagerMenu")
                {
                    textScreenChange.Text = y.location.ToString();
                }
                else if (t == "WF_HideFloatScreen")
                {
                    FloatMenu = -1;
                }
                else if(t == "WF_BackToPreviousScreen")
                {
                    textScreenChange.Text = previousMainScreenID.ToString();
                }
                else
                {
                    MessageBox.Show("Action Not Implemented: " + t);
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

        private void Label1_Click(object sender, EventArgs e)
        {

        }
    }
}
