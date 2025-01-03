﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TodoAppConsole.Implementations
{
    public class TaskManager
    {
        public TaskManager()
        {
            tasks = new List<Task>();
            nextId = 1;
        }

        public List<Task> tasks;

        public CategoryManager categoryManager;
        //public SaveData saveData;
        public int nextId;

        /// <summary>
        /// @param title 
        /// @param description 
        /// @param dueDate 
        /// @param category 
        /// @param priority 
        /// @return
        /// </summary>
        public void addTask(string title, string description, DateTime dueDate, Category category, string priority, FileSaver fileSaver)
        {  
            var task = new Task();
            task.id = nextId++;
            task.title = title;
            task.description = description;
            task.dueDate = dueDate;
            task.category = category;
            task.priority = priority;
            tasks.Add(task);

            fileSaver.saveToFileTodos(tasks, nextId);
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
        public void editTask(int id, string title, string description, DateTime dueDate, Category category, string priority, FileSaver fileSaver)
        {
            if (id >= 0 && id <= nextId)
            {
                var task = tasks.Find(t => t.id == id);
                task.title = title;
                task.description = description;
                task.dueDate = dueDate;
                task.category = category;
                task.priority = priority;

                //saveData.Tasks = tasks;
                fileSaver.saveToFileTodos(tasks, nextId);
            }

            return;
        }

        /// <summary>
        /// @param id 
        /// @return
        /// </summary>
        public void removeTask(int id, FileSaver fileSaver)
        {
            if (id >= 0 && id <= nextId) 
            {
                var idx = tasks.FindIndex(t => t.id == id);
                tasks.RemoveAt(idx);

                fileSaver.saveToFileTodos(tasks, nextId);
            }
            return;
        }

        public void toggeTaskCompletion(int id, FileSaver fileSaver)
        {
            if (id >= 0 && id <= nextId)
            {
                var task = tasks.Find(t => t.id == id);
                task.toggleCompletion();

                fileSaver.saveToFileTodos(tasks, nextId);
            }
            return;
        }
    }
}
