using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoAppConsole.Implementations
{
    public class Category
    {
        public Category(int id, string name, string color)
        {
            this.id = id;
            this.name = name;
            this.color = color;
        }

        public int id { get; set; }

        public string name { get; set; }

        public string color { get; set; }
    }
}
