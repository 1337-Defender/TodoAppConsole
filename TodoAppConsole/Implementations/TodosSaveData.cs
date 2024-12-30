using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoAppConsole.Implementations
{
    public class TodosSaveData
    {
        public int nextId { get; set; } = 1;
        public List<Task> Tasks { get; set; } = new List<Task>();
    }
}
