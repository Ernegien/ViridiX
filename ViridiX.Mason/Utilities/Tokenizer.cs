using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViridiX.Mason.Utilities
{
    public static class Tokenizer
    {
        /// <summary>
        /// Tokenizes a response message that's in a key=value format
        /// </summary>
        /// <returns>Dictionary containing key-value pairs</returns>
        public static Dictionary<string, string> TokenizeResponse(string message)
        {
            // Split the string into pieces.
            string[] pieces = message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Loop and build the tokenized dictionary.
            Dictionary<string, string> tokenDictionary = new Dictionary<string, string>();
            for (int i = 0; i < pieces.Length; i++)
            {
                // Check if the token as a value associated with it.
                if (pieces[i].Contains('=') == true)
                {
                    // Split the string into key value and add it to the dictionary.
                    string[] keyValue = pieces[i].Split('=');
                    tokenDictionary.Add(keyValue[0], keyValue[1]);
                }
                else
                {
                    // Add the key with no value.
                    tokenDictionary.Add(pieces[i], "");
                }
            }

            // Return the token dictionary.
            return tokenDictionary;
        }
    }
}
