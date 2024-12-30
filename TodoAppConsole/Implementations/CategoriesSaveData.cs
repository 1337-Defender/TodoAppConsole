using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoAppConsole.Implementations
{
    public class CategoriesSaveData
    {
        public int nextId { get; set; } = 1;
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}
