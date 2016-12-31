using System.Collections.Generic;

namespace IRV
{
    public static class DictionaryExtentions
    {
        public static void IncrementCounter(this Dictionary<string, int> dictionary, string key)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = dictionary[key] + 1;
            }
            else
            {
                dictionary.Add(key, 1);
            }
        }

    }
}