using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoAppConsole.Implementations
{
    public class CategoryManager
    {
        public List<Category> categories;
        public int nextId;

        public CategoryManager()
        {
            categories = new List<Category>();
            nextId = 1;
        }

        /// <summary>
        /// @param id 
        /// @param name 
        /// @param color 
        /// @return
        /// </summary>
        public void createCategory(string name, string color, FileSaver fileSaver)
        {
            var category = new Category(nextId++, name, color);
            categories.Add(category);
            fileSaver.saveToFileCategories(categories, nextId);
            return;
        }

        /// <summary>
        /// @param id 
        /// @param name 
        /// @param color 
        /// @return
        /// </summary>
        public void editCategory(int id, string name, string color, FileSaver fileSaver)
        {
            var category = categories.Find(c => c.id == id);
            category.name = name;
            category.color = color;
            fileSaver.saveToFileCategories(categories, nextId);
            return;
        }

        /// <summary>
        /// @param id
        /// </summary>
        public void deleteCategory(int id, FileSaver fileSaver)
        {
            var category = categories.Find(c => c.id == id);
            categories.Remove(category);
            fileSaver.saveToFileCategories(categories, nextId);
        }
    }
}
