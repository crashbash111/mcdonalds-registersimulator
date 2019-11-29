using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class RegisterButton
    {
        public string title;
        public int number;
        public Screen screen;
        public string imgup;
        public string imgdn;
        public int w;
        public int h;
        public string textup;
        public string textdn;
        private string bgup;
        private string bgdn;
        public List<string> actionType;
        public int location;
        public int productCode;

        public string Bgup
        {
            get
            {
                return bgup;
            }

            set
            {
                //converts colour to accurate register colour
                bgup = Configurator.ConvertColour(value);
            }
        }

        public string Bgdn
        {
            get
            {
                return bgdn;
            }

            set
            {
                //converts colour to accurate register colour
                bgdn = Configurator.ConvertColour(value);
            }
        }

        public RegisterButton()
        {
            location = -1;
            productCode = -1;
            actionType = new List<string>();
        }
        public RegisterButton(string title, string number, Screen screen, string imgup, string imgdn, string w, string h, string textup, string textdn, string bgup, string bgdn)
        {
            this.title = title;
            this.number = int.Parse(number);
            this.screen = screen;
            this.imgup = imgup;
            this.imgdn = imgdn;
            this.w = int.Parse(w);
            this.h = int.Parse(h);
            this.textup = textup;
            this.textdn = textdn;
            this.Bgup = bgup;
            this.Bgdn = bgdn;
            location = -1;
            productCode = -1;
            actionType = new List<string>();
        }
    }
}
