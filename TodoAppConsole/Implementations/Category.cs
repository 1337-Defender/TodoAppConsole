using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoAppConsole.Implementations
{
    public class Category
    {
        public Category(string name)
        {
            this.name = name;
        }

        public int id;

        public string name { get; set; }

        public string color;
    }
}
