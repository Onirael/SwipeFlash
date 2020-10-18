using System.Collections.Generic;

namespace SwipeFlash.Core
{
    /// <summary>
    /// A container for a flashcard category data
    /// </summary>
    public struct CategoryData
    {
        /// <summary>
        /// The name of the category
        /// </summary>
        public string Name;

        /// <summary>
        /// The category logo
        /// </summary>
        public string Logo;

        /// <summary>
        /// The category articles
        /// </summary>
        public List<string> Articles;

        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="logo"></param>
        public CategoryData(string name, string logo)
        {
            Name = name;
            Logo = logo;
            Articles = new List<string>();
        }

        /// <summary>
        /// Constructor with articles argument
        /// </summary>
        /// <param name="name"></param>
        /// <param name="logo"></param>
        /// <param name="articles"></param>
        public CategoryData(string name, string logo, List<string> articles) : this(name, logo)
        {
            Articles = articles;
        }

        public override bool Equals(object obj)
        {
            // Type checks the object
            if (!(obj is CategoryData categoryData))
                return false;

            bool isEqual = Name == categoryData.Name &&
                           Logo == categoryData.Logo &&
                           Articles == categoryData.Articles;

            return isEqual;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
