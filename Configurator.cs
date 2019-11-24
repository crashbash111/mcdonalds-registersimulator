using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using WindowsFormsApp1.Properties;
using System.Xml;

namespace WindowsFormsApp1
{
    static class Configurator
    {
        static string xmlPath = "D:/Personal/Joshua/Downloads/NP6-assets/NP6 Simulator_files/screen.xml";
        static bool displayException = true;
        public static List<RegisterButton> testImgList = new List<RegisterButton>();

        static public bool ReadConfigurationFile()
        {
            try
            {
                XmlTextReader reader = new XmlTextReader(xmlPath);
                while(reader.Read()){
                    switch(reader.NodeType){
                        case XmlNodeType.Element:
                            if(reader.Name == "Button"){
                                RegisterButton tmpButton = new RegisterButton();
                                try{
                                    tmpButton.title = reader.GetAttribute("title");
                                    tmpButton.category = int.Parse(reader.GetAttribute("category"));
                                    tmpButton.imgup = reader.GetAttribute("bitmap");
                                    tmpButton.imgdn = reader.GetAttribute("bitmapdn");
                                    tmpButton.w = int.Parse(reader.GetAttribute("v"));
                                    tmpButton.h = int.Parse(reader.GetAttribute("h"));
                                    tmpButton.textup = reader.GetAttribute("textup");
                                    tmpButton.textdn = reader.GetAttribute("textdn");
                                    tmpButton.bgup = reader.GetAttribute("bgup");
                                    tmpButton.bgdn = reader.GetAttribute("bgdn");
                                    testImgList.Add(tmpButton);
                                }
                                catch{

                                }
                                
                            }
                            break;
                        case XmlNodeType.Text:
                            
                            //ConfFactory(reader.Value);
                            break;
                        case XmlNodeType.EndElement:
                            break;
                    }
                    //ConfFactory(reader.Name);
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
