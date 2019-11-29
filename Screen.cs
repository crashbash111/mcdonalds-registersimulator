using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Screen
    {
        public int number;
        public string title;
        public int type;
        public string bg;
        public List<RegisterButton> buttons;
        public Object panel;
        public Screen()
        {
            buttons = new List<RegisterButton>();
        }

        public Screen(int number, string title, int type, string bg)
        {
            this.number = number;
            this.title = title;
            this.type = type;
            this.bg = bg;
            buttons = new List<RegisterButton>();
        }
    }
}
