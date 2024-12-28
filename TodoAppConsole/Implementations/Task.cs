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
        }

        public int id;

        public string title;

        public string description;

        public DateTime dueDate;

        public DateTime completionTimeStamp;

        public string priority;

        public bool isCompleted;

        public bool isPinned;

        public Category category;

        public string taskGroup;

        /// <summary>
        /// @return
        /// </summary>
        public void markAsCompleted()
        {
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
