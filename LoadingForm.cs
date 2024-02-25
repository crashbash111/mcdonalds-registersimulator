using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Compression;
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
            this.BeginInvoke((System.Windows.Forms.MethodInvoker)delegate ()
            {
                labScreen.Text = "Extracting images...";
                //zips no longer used since NPSharp upgrade
                Configurator.imgRepositoryPath = Configurator.posDataLocation + "/images/POS_images/"; //use repository.1024x768 for posdata pre-2023
            });
                
            //loads main assets
            while (mainForm.loadingProgress < progressBar.Maximum-1)
            {
                    this.BeginInvoke((System.Windows.Forms.MethodInvoker)delegate ()
                    {
                        progressBar.Value = mainForm.loadingProgress;
                        labScreen.Text = ("Loading " + Configurator.testImgList[mainForm.loadingProgress].screen.title.Replace(@"\n", " "));
                        try
                        {
                            if(Configurator.testImgList[mainForm.loadingProgress] != null)
                            {
                                label.Text = (Configurator.testImgList[mainForm.loadingProgress].title.Replace(@"\n", " "));
                            }
                        }
                        catch
                        {
                            //null
                        }
                        
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
