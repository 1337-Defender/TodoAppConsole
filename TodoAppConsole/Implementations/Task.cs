using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoAppConsole.Implementations
{
    public class Task
    {
        public Task()
        {
            isCompleted = false;
        }

        public int id { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public DateTime dueDate { get; set; }

        public string priority { get; set; }

        public bool isCompleted { get; set; }

        public Category category { get; set; }

        /// <summary>
        /// @return
        /// </summary>

        public void toggleCompletion()
        {
            isCompleted = !isCompleted;
            // TODO implement here
            return;
        }
    }
}
