using System;
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
        private static string FilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create), "TodoApp", "todos.json");
        public FileSaver()
        { 
        }

        public string dataDirectory;

        /// <summary>
        /// @param fileName 
        /// @return
        /// </summary>
        public void saveToFile(List<Task> todos)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath));

            var json = JsonSerializer.Serialize(todos, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
            return;
        }

        /// <summary>
        /// @param fileName 
        /// @return
        /// </summary>
        public static List<Task> loadFromFile(string fileName)
        {
            if (!File.Exists(FilePath)) return new List<Task>();  // If no file exists, return an empty list

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Task>>(json);
        }
    }
}
