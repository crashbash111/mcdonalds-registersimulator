using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            CurrentlyLoadedScreen = 1;
            //ChangeActiveMenu(1);
        }


        Button[] numButtons = new Button[10];
        List<Button> tmpButtons = new List<Button>();
        List<Panel> screenPanels = new List<Panel>();
        public int loadingProgress;
        private int currentlyLoadedScreen;


        private void ChangeActiveMenu(int screenIndex)
        {
            foreach(Panel pan in screenPanels)
            {
                pan.Visible = false;
            }
            (Configurator.screenList.Find(x => x.number == screenIndex).panel as Panel).Visible = true;
        }

        static string baseDir = "D:/Personal/Joshua/Downloads/repository.1024x768/";

        public int CurrentlyLoadedScreen
        {
            get
            {
                return currentlyLoadedScreen;
            }

            set
            {
                (Configurator.screenList.Find(x => x.number == value).panel as Panel).Visible = true;
                (Configurator.screenList.Find(x => x.number == currentlyLoadedScreen).panel as Panel).Visible = false;
                currentlyLoadedScreen = value;
            }
        }

        private string FetchImgDir(string imgName)
        {
            return baseDir + imgName;
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
                pan.Top = panel2.Top;
                pan.Left = panel2.Left;
                pan.Height = panel2.Height;
                pan.Width = panel2.Width;
                pan.BackColor = panel2.BackColor;
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
                y.Size = new Size(64 * x.h, 64 * x.w); //still needs 5 spacing accounted for
                y.Top = (x.number / 10) * 70;
                y.Left = ((x.number % 10) * (y.Width + 5)) + 5;
                try
                {
                    if (x.location != -1)
                    {
                        y.Click += Y_Click;
                    }
                    y.BackgroundImage = FetchImg(x.imgup);
                    y.BackgroundImageLayout = ImageLayout.Stretch;
                    y.Text = "";
                    y.TabStop = false;
                    y.FlatStyle = FlatStyle.Flat;
                    y.FlatAppearance.BorderSize = 0;
                    
                }
                catch
                {
                    try
                    {
                        y.Text = (x.title.Replace(@"\n", Environment.NewLine));
                    }
                    catch
                    {
                        //null
                    }
                    
                    y.BackColor = Color.FromName(x.bgup);
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

        private void Y_Click(object sender, EventArgs e)
        {
            Button x = sender as Button;
            RegisterButton y = x.Tag as RegisterButton;
            textScreenChange.Text = y.location.ToString();
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
                panel2.Controls.Add(numButtons[i]);
            }
        }

        private void TextScreenChange_TextChanged(object sender, EventArgs e)
        {
            if(textScreenChange.Text != "")
            {
                try
                {
                    CurrentlyLoadedScreen = (int.Parse(textScreenChange.Text));
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
