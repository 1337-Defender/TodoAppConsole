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

        public DateTime completionTimeStamp;

        public string priority { get; set; }

        public bool isCompleted { get; set; }

        public bool isPinned;

        public Category category { get; set; }

        public string taskGroup;

        /// <summary>
        /// @return
        /// </summary>

        public void toggleCompletion()
        {
            isCompleted = !isCompleted;
            // TODO implement here
            return;
        }

        /// <summary>
        /// @return
        /// </summary>
        public void pinTask()
        {
            // TODO implement here
            return;
        }
    }
}
