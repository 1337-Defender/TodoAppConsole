using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoAppConsole.Implementations
{
    public class TaskGroup
    {
        public TaskGroup()
        {
        }

        public int id;

        public string title;

        public Category category;

        public List<Task> tasks;

        /// <summary>
        /// @param task 
        /// @return
        /// </summary>
        public void addTaskToGroup(Task task)
        {
            // TODO implement here
            return;
        }

        /// <summary>
        /// @param taskId 
        /// @return
        /// </summary>
        public void removeTaskFromGroup(int taskId)
        {
            // TODO implement here
            return;
        }

        /// <summary>
        /// @return
        /// </summary>
        public List<Task> getTasksInGroup()
        {
            // TODO implement here
            List<Task> l = new List<Task>();
            return l;
        }
    }
}
