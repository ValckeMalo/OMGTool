namespace MaloProduction
{
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
    }
}