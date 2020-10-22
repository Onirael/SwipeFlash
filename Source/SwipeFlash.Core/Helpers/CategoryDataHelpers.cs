namespace SwipeFlash.Core
{
    public static class CategoryDataHelpers
    {
        public static bool IsCategoryDataValid(this CategoryData categoryData, bool showErrors = true)
        {
            // If the logo isn't unicode or is empty, quit
            if (string.IsNullOrEmpty(categoryData.Logo) ||
                !categoryData.Logo.IsUnicode())
            {
                if (showErrors) IoC.Get<WindowService>().CreateWarning("Logos must be unicode (emoji)");
                return false;
            }

            // If the name is invalid, quit
            if (string.IsNullOrEmpty(categoryData.Name))
            {
                if (showErrors) IoC.Get<WindowService>().CreateWarning("All fields must be filled in");
                return false;
            }

            return true;
        }
    }
}
