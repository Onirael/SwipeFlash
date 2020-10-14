using System.Text;

namespace SwipeFlash.Core
{
    public static class DataChecker
    {
        /// <summary>
        /// Checks the validity of family input data
        /// </summary>
        /// <param name="familyData">The family data container</param>
        /// <param name="showErrors">Whether to show error message boxes</param>
        /// <returns></returns>
        public static bool IsFamilyDataValid(FlashcardFamilyData familyData, bool showErrors = true)
        {
            // If any string is null or empty
            if (string.IsNullOrEmpty(familyData.Name) ||
                string.IsNullOrEmpty(familyData.Category1) ||
                string.IsNullOrEmpty(familyData.Category2) ||
                string.IsNullOrEmpty(familyData.Logo1) ||
                string.IsNullOrEmpty(familyData.Logo2))
            {
                IoC.Get<WindowService>().CreateWindow(new WindowArgs()
                {
                    Message = "All fields must be filled in",
                    TargetType = WindowType.Warning,
                });

                return false;
            }

            // If both logos aren't unicode
            if (!IsUnicode(familyData.Logo1) ||
                !IsUnicode(familyData.Logo2))
            {
                IoC.Get<WindowService>().CreateWindow(new WindowArgs()
                {
                    Message = "Logos must be unicode (emoji)",
                    TargetType = WindowType.Warning,
                });

                return false;
            }

            return true;
        }

        private static bool IsUnicode(string input)
        {
            var asciiBytesCount = Encoding.ASCII.GetByteCount(input);
            var unicodBytesCount = Encoding.UTF8.GetByteCount(input);
            return asciiBytesCount != unicodBytesCount;
        }
    }
}
