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
        public int category;
        public string imgup;
        public string imgdn;
        public int w;
        public int h;
        public string textup;
        public string textdn;
        public string bgup;
        public string bgdn;
        public RegisterButton()
        {

        }
        public RegisterButton(string title, string number, string category, string imgup, string imgdn, string w, string h, string textup, string textdn, string bgup, string bgdn)
        {
            this.title = title;
            this.number = int.Parse(number);
            this.category = int.Parse(category);
            this.imgup = imgup;
            this.imgdn = imgdn;
            this.w = int.Parse(w);
            this.h = int.Parse(h);
            this.textup = textup;
            this.textdn = textdn;
            this.bgup = bgup;
            this.bgdn = bgdn;
        }
    }
}
