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
            (Configurator.screenList.Find(x => x.number == 1).panel as Panel).Visible = true;
        }


        Button[] numButtons = new Button[10];
        List<Button> tmpButtons = new List<Button>();
        List<Panel> screenPanels = new List<Panel>();
        public int loadingProgress;

        static string baseDir = "D:/Personal/Joshua/Downloads/NP6-assets/NP6 Simulator_files/repository.1024x768/";
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
                    y.Image = FetchImg(x.imgup);
                    y.Text = "";
                    y.TabStop = false;
                    y.FlatStyle = FlatStyle.Flat;
                    y.FlatAppearance.BorderSize = 0;
                }
                catch
                {
                    y.Text = (x.title.Replace(@"\n", Environment.NewLine));
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
    }
}
