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
        //declares whether this instance requires the user to specify the simulator files
        //USAGE: true = running in store location with WAY available, false = running on isolated hardware
        static bool physicalStoreEnvironment = false;
        //sets default paths (overridden when running in store environment)
        static string xmlPath = "/screen.xml";
        static string xmlPathOutages = "/prodoutage.xml";
        public static string posDataLocation = "./";
        public static string imgRepositoryPath = "/images/repository.1024x768/";
        public static string imgRepositoryExpectedZipPath = "/images/repository.1024x768.zip";
        static bool displayException = true;
        public static List<RegisterButton> testImgList = new List<RegisterButton>();
        public static List<Screen> screenList = new List<Screen>();

        public static string windowTitle = "NP6 Simulator";

        static public bool ReadConfigurationFile()
        {
            if (!physicalStoreEnvironment)
            {
                DialogResult r = MessageBox.Show("This instance is not running within a store environment. In the following dialog, please provide the location of NP6 register files. This folder is typically called Posdata.", windowTitle, MessageBoxButtons.OKCancel);
                if (r == DialogResult.OK)
                {
                    //loops location prompt until valid path
                    while (true)
                    {
                        using (var folderDialog = new FolderBrowserDialog())
                        {
                            DialogResult result = folderDialog.ShowDialog();
                            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                            {
                                posDataLocation = folderDialog.SelectedPath;
                                SetPathVariables(Directory.GetFiles(folderDialog.SelectedPath));
                                break;
                            }
                            else
                            {
                                if (MessageBox.Show("Please select a valid path.", windowTitle, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                                {
                                    Application.Exit();
                                }
                            }
                        }
                    }
                }
                else
                {
                    Application.Exit();
                }
            }


            try
            {
                Screen activeScreen = new Screen(-1, "", 1000, "");
                Button activeButton = new Button();
                XmlTextReader reader = new XmlTextReader(xmlPath);
                while (reader.Read())
                {

                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == "Screen")
                            {
                                int y = int.Parse(reader.GetAttribute("number"));
                                if (y != activeScreen.number)
                                {
                                    activeScreen = new Screen(y, reader.GetAttribute("title"), int.Parse(reader.GetAttribute("type")), reader.GetAttribute("bgimage"));
                                    screenList.Add(activeScreen);
                                }
                            }
                            if (reader.Name == "Button")
                            {
                                RegisterButton tmpButton = new RegisterButton();
                                try
                                {
                                    tmpButton.title = reader.GetAttribute("title");
                                    tmpButton.number = int.Parse(reader.GetAttribute("number")) - 1;
                                    tmpButton.screen = activeScreen;//int.Parse(reader.GetAttribute("category"));
                                    tmpButton.imgup = reader.GetAttribute("bitmap");
                                    tmpButton.imgdn = reader.GetAttribute("bitmapdn");
                                    tmpButton.w = int.Parse(reader.GetAttribute("v"));
                                    tmpButton.h = int.Parse(reader.GetAttribute("h"));
                                    tmpButton.textup = reader.GetAttribute("textup");
                                    tmpButton.textdn = reader.GetAttribute("textdn");
                                    tmpButton.Bgup = reader.GetAttribute("bgup");
                                    tmpButton.Bgdn = reader.GetAttribute("bgdn");
                                    try
                                    {
                                        tmpButton.productCode = int.Parse(reader.GetAttribute("productCode"));
                                    }
                                    catch
                                    {
                                        //not a product
                                    }

                                    reader.Read();
                                    while (reader.Name != "Button" && reader.NodeType != XmlNodeType.EndElement)
                                    {
                                        //MessageBox.Show("loop" + tmpButton.title + " " + reader.NodeType.ToString() + " " + reader.Name);
                                        if (reader.Name == "Action")
                                        {
                                            tmpButton.actionType.Add(reader.GetAttribute("workflow"));
                                            if (reader.GetAttribute("workflow") == "WF_ShowScreen" || reader.GetAttribute("workflow") == "WF_ShowFloatScreen")
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
                                            else if (reader.GetAttribute("workflow") == "WF_ShowManagerMenu")
                                            {
                                                tmpButton.location = 900;
                                            }
                                        }
                                        reader.Read();
                                    }


                                    testImgList.Add(tmpButton);
                                    activeScreen.buttons.Add(tmpButton);
                                }
                                catch
                                {

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
            catch (Exception e)
            {
                if (displayException) MessageBox.Show(e.ToString());
                return false;
            }
            return true;
        }

        private static void SetPathVariables(string[] strings)
        {
            xmlPath = posDataLocation + xmlPath;
            xmlPathOutages = posDataLocation + xmlPathOutages;
            imgRepositoryPath = posDataLocation + imgRepositoryPath;
            imgRepositoryExpectedZipPath = posDataLocation + imgRepositoryExpectedZipPath;

            //code for dynamically fetching files as it finds them. Have hardcoded for now as it is not working.
            //foreach(string y in strings)
            //{
            //    string x = y.Split('\\').Last();
            //    switch (x)
            //    {
            //        case "screen.xml":
            //            xmlPath = x;
            //            break;
            //        case "prodoutage.xml":
            //            xmlPathOutages = x;
            //            break;
            //    }
            //    if (x == "screen.xml")
            //    {
            //        xmlPath = y;
            //    }
            //}



        }

        static public string ConvertColour(string colorToConvert)
        {
            //checks to ensure colour code is converted to something Visual Studio supports
            if (colorToConvert == "LIGHTRED")
            {
                return "Tomato";
            }
            if (colorToConvert == "BRIGHTWHITE")
            {
                return "FloralWhite";
            }
            //no converting required
            return colorToConvert;
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
                            if (reader.Name == "Product")
                            {
                                int codeToRemove = int.Parse(reader.GetAttribute("code"));
                                foreach (Button x in list)
                                {
                                    RegisterButton y = x.Tag as RegisterButton;
                                    if (y.productCode == codeToRemove)
                                    {
                                        Label t = new Label();
                                        t.Text = "OUTAGE";
                                        t.BackColor = Color.Yellow;
                                        t.AutoSize = true;
                                        t.TextAlign = ContentAlignment.BottomRight;
                                        x.Controls.Add(t);
                                        t.Top = x.Height - t.Height;
                                        t.Left = x.Width - t.Width;
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
