namespace MVProduction
{
    using System.Globalization;
    using System.Text.RegularExpressions;
    using UnityEngine;

    public static class ExtensionMethods
    {
        public static bool Invert(this bool value)
        {
            return !value;
        }

        private static string alphabet = "abcdefghijklmnopqrstuvwxyz";
        public static string GenerateRandomString(int length)
        {
            char[] stringChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                stringChars[i] = alphabet[Random.Range(0, alphabet.Length)];
            }
            return new string(stringChars);
        }

        public static string CamelCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var separated = Regex.Replace(input, "([A-Z])", " $1");

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string result = textInfo.ToTitleCase(separated.ToLower());

            return result.Replace(" ", "");
        }
    }
}