using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        int selectedNumber = 0;

        public Form1()
        {
            InitializeComponent();
            initCountButtons();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is a register.");
        }

        Button[] numButtons = new Button[11];

        static string baseDir = "D:/Personal/Joshua/Downloads/NP6-assets/NP6 Simulator_files/repository.1024x768/";
        private string FetchImgDir(string imgName)
        {
            return baseDir + imgName + ".png";
        }

        private Image FetchImg(string imgName)
        {
            return Image.FromFile(FetchImgDir(imgName));
        }

        private void initCountButtons()
        {
            for(int i = 0; i<11; i++)
            {
                numButtons[i] = new Button();
                numButtons[i].Size = new Size(64, 64);
                numButtons[i].Top = 0;
                numButtons[i].Left = (i * (numButtons[i].Width + 5)) + 5;
                try
                {
                    numButtons[i].Image = FetchImg("0" + i.ToString());
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

        private void Button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
