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
        public void createCategory(int id, string name, string color)
        {
            var category = new Category(id, name, color);
            categories.Add(category);
            return;
        }

        /// <summary>
        /// @param id 
        /// @param name 
        /// @param color 
        /// @return
        /// </summary>
        public void editCategory(int id, string name, string color)
        {
            var category = categories.Find(c => c.id == id);
            category.name = name;
            category.color = color;
            return;
        }

        /// <summary>
        /// @param id
        /// </summary>
        public void deleteCategory(int id)
        {
            var category = categories.Find(c => c.id == id);
            categories.Remove(category);
            
        }
    }
}
