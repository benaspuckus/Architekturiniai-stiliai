using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class Category
    {

        protected Category()
        {
        }

        private Category(string name, string description, Guid categoryId, Guid? parentCategoryId)
        {
            CategoryId = categoryId;
            ParentCategoryId = parentCategoryId;
            Name = name;
            Description = description;
        }

        public static Category CreateInitialItem(string name, string description)
        {
            return new Category(name, description, Guid.NewGuid(), null);
        }

        public static Category CreateChildItem(string name, string description, Guid parentItemId)
        {
            return new Category(name, description, Guid.NewGuid(), parentItemId);
        }

        public Guid CategoryId { get; }
        public Guid? ParentCategoryId { get; }
        public string Name { get; }
        public string Description { get; }

        private readonly List<Category> _childCategories = new List<Category>();
        public IReadOnlyList<Category> ChildCategories => _childCategories;

        private readonly List<Item> _childItems = new List<Item>();
        public IReadOnlyList<Item> ChildItems => _childItems;

        public void AddChildCategory(Category category)
        {
            if (ChildItems.Any())
            {
                throw new ArgumentException("Category already has items");
            }

            _childCategories.Add(category);
        }
    }
}
