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
using System.Drawing;

namespace WindowsFormsApp1
{
    static class Configurator
    {
        static string xmlPath = "D:/Personal/Joshua/Google Drive/McDonalds/NP/NP6 24-11-19/PosData/screen.xml";
        static string xmlPathOutages = "D:/Personal/Joshua/Google Drive/McDonalds/NP/NP6 24-11-19/PosData/prodoutage.xml";
        static bool displayException = true;
        public static List<RegisterButton> testImgList = new List<RegisterButton>();
        public static List<Screen> screenList = new List<Screen>();

        static public bool ReadConfigurationFile()
        {
            try
            {
                Screen activeScreen = new Screen(-1, "");
                Button activeButton = new Button();
                XmlTextReader reader = new XmlTextReader(xmlPath);
                while(reader.Read()){
                    
                    switch(reader.NodeType){
                        case XmlNodeType.Element:
                            if(reader.Name == "Screen")
                            {
                                int y = int.Parse(reader.GetAttribute("number"));
                                if (y != activeScreen.number)
                                {
                                    activeScreen = new Screen(y, reader.GetAttribute("title"));
                                    screenList.Add(activeScreen);
                                }
                            }
                            if(reader.Name == "Button"){
                                RegisterButton tmpButton = new RegisterButton();
                                try{
                                    tmpButton.title = reader.GetAttribute("title");
                                    tmpButton.number = int.Parse(reader.GetAttribute("number")) - 1;
                                    tmpButton.screen = activeScreen;//int.Parse(reader.GetAttribute("category"));
                                    tmpButton.imgup = reader.GetAttribute("bitmap");
                                    tmpButton.imgdn = reader.GetAttribute("bitmapdn");
                                    tmpButton.w = int.Parse(reader.GetAttribute("v"));
                                    tmpButton.h = int.Parse(reader.GetAttribute("h"));
                                    tmpButton.textup = reader.GetAttribute("textup");
                                    tmpButton.textdn = reader.GetAttribute("textdn");
                                    tmpButton.bgup = reader.GetAttribute("bgup");
                                    tmpButton.bgdn = reader.GetAttribute("bgdn");
                                    try
                                    {
                                        tmpButton.productCode = int.Parse(reader.GetAttribute("productCode"));
                                    }
                                    catch
                                    {
                                        //not a product
                                    }

                                    reader.Read();
                                    while(reader.Name != "Button" && reader.NodeType != XmlNodeType.EndElement)
                                    {
                                        //MessageBox.Show("loop" + tmpButton.title + " " + reader.NodeType.ToString() + " " + reader.Name);
                                        if (reader.Name == "Action")
                                        {
                                            if (reader.GetAttribute("workflow") == "WF_ShowScreen")
                                            {
                                                reader.Read();
                                                while (reader.Name != "Action" && reader.NodeType != XmlNodeType.EndElement)
                                                {
                                                    if (reader.Name == "Parameter")
                                                    {
                                                        tmpButton.location = int.Parse(reader.GetAttribute("value"));
                                                    }
                                                    reader.Read();
                                                }
                                            }
                                        }
                                        reader.Read();
                                    }
                                    

                                    testImgList.Add(tmpButton);
                                    activeScreen.buttons.Add(tmpButton);
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

        static public void ReadOutages(List<Button> list)
        {
            try
            {
                XmlTextReader reader = new XmlTextReader(xmlPathOutages);
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if(reader.Name == "Product")
                            {
                                int codeToRemove = int.Parse(reader.GetAttribute("code"));
                                foreach(Button x in list)
                                {
                                    RegisterButton y = x.Tag as RegisterButton;
                                    if(y.productCode == codeToRemove)
                                    {
                                        Label t = new Label();
                                        t.Text = "OUTAGE";
                                        t.BackColor = Color.Yellow;
                                        t.AutoSize = true;
                                        
                                        t.TextAlign = ContentAlignment.BottomRight;
                                        x.Controls.Add(t);
                                        //x.TextAlign = System.Drawing.ContentAlignment.BottomRight;
                                    }
                                }
                            }
                            break;
                        case XmlNodeType.Text:
                            break;
                        case XmlNodeType.EndElement:
                            break;
                    }
                    //ConfFactory(reader.Name);
                }
            }
            catch (Exception e)
            {
                if (displayException) MessageBox.Show(e.ToString());
            }
        }

        private static void ConfFactory(string str)
        {
            MessageBox.Show("Processing line: " + str);
        }
    }
}
