using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace WindowsFormsApp1
{
    static class Configurator
    {
        static string confPath = "conf.ini";
        static bool displayException = true;

        static public bool ReadConfigurationFile()
        {
            try
            {
                using (Stream stream = Assembly.GetEntryAssembly().GetManifestResourceStream(confPath))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        string str;
                        while ((str = reader.ReadLine()) != null)
                        {
                            ConfFactory(str);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                if(displayException) MessageBox.Show(e.ToString());
                return false;
            }
            return true;
        }

        private static void ConfFactory(string str)
        {
            MessageBox.Show("Processing line: " + str);
        }
    }
}
