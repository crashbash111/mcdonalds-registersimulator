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
    public partial class LoadingForm : Form
    {
        private Form1 mainForm;
        public LoadingForm(Form1 x)
        {
            
            InitializeComponent();
            mainForm = x;
            progressBar.Maximum = Configurator.testImgList.Count();
            
        }

        private void UpdateLoader()
        {
            while (mainForm.loadingProgress < progressBar.Maximum-1)
            {
                

                    this.BeginInvoke((System.Windows.Forms.MethodInvoker)delegate ()
                    {
                        progressBar.Value = mainForm.loadingProgress;
                        labScreen.Text = ("Loading " + Configurator.testImgList[mainForm.loadingProgress].screen.title.Replace(@"\n", " "));
                        label.Text = (Configurator.testImgList[mainForm.loadingProgress].title.Replace(@"\n", " "));
                    });
                    Thread.Sleep(100);
            }
            this.BeginInvoke((System.Windows.Forms.MethodInvoker)delegate () { this.Close(); });
        }

        private void LoadingForm_Load(object sender, EventArgs e)
        {
            Thread loader = new Thread(new ThreadStart(UpdateLoader));
            loader.IsBackground = true;
            loader.Start();
            this.Show();
        }
    }
}
