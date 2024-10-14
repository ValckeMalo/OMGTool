namespace MaloProduction
{
    public static class FilterStringFormatter
    {
        private static string[] filterDescriptions = new string[2] { "Filter Selected", "Filters Selected" };
        private static string[] filterCountDescriptions = new string[5] { "No ", "One ", "Two ", "Three ", "Four " };

        /// <summary>
        /// Returns a formatted string that describes the number of filters selected.
        /// </summary>
        /// <param name="count">The number of filters selected (from 0 to 4).</param>
        /// <returns>A formatted string describing the number of filters selected.</returns>
        public static string GetFilterDescription(int count)
        {
            if (count == 1)
            {
                return filterCountDescriptions[count] + filterDescriptions[0];
            }

            return filterCountDescriptions[count] + filterDescriptions[1];
        }
    }

    public static class ExtensionMethods
    {
        public static bool Invert(this bool value)
        {
            return !value;
        }
    }
}