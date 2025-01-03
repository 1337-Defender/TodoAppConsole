﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace TodoAppConsole.Implementations
{
    public class FileSaver
    {
        public TodosSaveData TodosSaveData { get; set; }
        public static string TodosFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create), "TodoApp", "todos.json");

        public CategoriesSaveData CategoriesSaveData { get; set; }
        public static string CategoriesFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create), "TodoApp", "categories.json");

        public FileSaver()
        {
            TodosSaveData = new TodosSaveData();
            TodosSaveData = loadFromFileTodos();

            CategoriesSaveData = new CategoriesSaveData();
            CategoriesSaveData = loadFromFileCategories();
        }

        /// <summary>
        /// @param fileName 
        /// @return
        /// </summary>
        public void saveToFileTodos(List<Task> tasks, int nextId)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(TodosFilePath));

            TodosSaveData.Tasks = tasks;
            TodosSaveData.nextId = nextId;
            var json = JsonSerializer.Serialize(TodosSaveData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(TodosFilePath, json);
            return;
        }

        public void saveToFileCategories(List<Category> categories, int nextId)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(CategoriesFilePath));

            CategoriesSaveData.Categories = categories;
            CategoriesSaveData.nextId = nextId;
            var json = JsonSerializer.Serialize(CategoriesSaveData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(CategoriesFilePath, json);
            return;
        }

        /// <summary>
        /// @param fileName 
        /// @return
        /// </summary>
        public static TodosSaveData loadFromFileTodos()
        {
            if (!File.Exists(TodosFilePath)) return new TodosSaveData();  // If no file exists, return an empty list

            var json = File.ReadAllText(TodosFilePath);
            return JsonSerializer.Deserialize<TodosSaveData>(json) ?? new TodosSaveData();
        }

        public static CategoriesSaveData loadFromFileCategories()
        {
            CategoriesSaveData data = new CategoriesSaveData();
            data.Categories.Add(new Category(data.nextId++, "Default", "white"));
            if (!File.Exists(CategoriesFilePath)) return data;  // If no file exists, return an empty list

            var json = File.ReadAllText(CategoriesFilePath);
            return JsonSerializer.Deserialize<CategoriesSaveData>(json) ?? data;
        }

        public bool ExportSaveData(List<Task> tasks, string filePath, string fileName)
        {
            if (tasks == null || tasks.Count == 0)
            {
                return false;
            }

            try
            {
                // Construct the full path for the file
                string fullPath = Path.Combine(filePath, $"{fileName}.txt");

                // Ensure the directory exists
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                // Write to the full path
                using (StreamWriter writer = new StreamWriter(fullPath))
                {
                    writer.WriteLine("Task Export");
                    writer.WriteLine("============");
                    foreach (var task in tasks)
                    {
                        writer.WriteLine($"ID: {task.id}");
                        writer.WriteLine($"Title: {task.title}");
                        writer.WriteLine($"Description: {task.description}");
                        writer.WriteLine($"Due Date: {task.dueDate:yyyy-MM-dd HH:mm}");
                        writer.WriteLine($"Priority: {task.priority}");
                        writer.WriteLine($"Completed: {task.isCompleted}");
                        writer.WriteLine($"Category: {task.category?.name ?? "None"}");
                        writer.WriteLine(new string('-', 40)); // Separator
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
