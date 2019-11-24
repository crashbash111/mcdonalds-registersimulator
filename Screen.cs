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
        public List<RegisterButton> buttons;
        public Object panel;
        public Screen()
        {
            buttons = new List<RegisterButton>();
        }

        public Screen(int number, string title)
        {
            this.number = number;
            this.title = title;
            buttons = new List<RegisterButton>();
        }
    }
}
