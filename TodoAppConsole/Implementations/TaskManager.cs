using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoAppConsole.Implementations
{
    public class TaskManager
    {
        public TaskManager()
        {
        }

        public List<Task> tasks;

        public CategoryManager categoryManager;

        /// <summary>
        /// @param title 
        /// @param description 
        /// @param dueDate 
        /// @param category 
        /// @param priority 
        /// @return
        /// </summary>
        public void addTask(string title, string description, DateTime dueDate, Category category, string priority)
        {
            // TODO implement here
            return;
        }

        /// <summary>
        /// @param id 
        /// @param title 
        /// @param description 
        /// @param dueDate 
        /// @param category 
        /// @param priority 
        /// @return
        /// </summary>
        public void editTask(int id, string title, string description, DateTime dueDate, Category category, string priority)
        {
            // TODO implement here
            return;
        }

        /// <summary>
        /// @param id 
        /// @return
        /// </summary>
        public void removeTask(int id)
        {
            // TODO implement here
            return;
        }

        /// <summary>
        /// @param taskId 
        /// @return
        /// </summary>
        public Task getTask(int taskId)
        {
            // TODO implement here
            return null;
        }

        /// <summary>
        /// @return
        /// </summary>
        public List<Task> getAllTasks()
        {
            // TODO implement here
            return null;
        }

        /// <summary>
        /// @param attribute 
        /// @return
        /// </summary>
        public List<Task> filterTasks(string attribute)
        {
            // TODO implement here
            return null;
        }

        /// <summary>
        /// @param keyword 
        /// @return
        /// </summary>
        public List<Task> searchTasks(string keyword)
        {
            // TODO implement here
            return null;
        }

        /// <summary>
        /// @param attribute 
        /// @return
        /// </summary>
        public List<Task> sortTasks(string attribute)
        {
            // TODO implement here
            return null;
        }
    }
}
