using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FalloutHackingGame
{
    //This class pulls in a file and creates a dictionary of List<string> with each key being a List<string> of words with the same character count.
    public class Dictionary
    {
        //For this example, a default paramater value is provided. If none is provided, then the default is used. If running this on your own PC, that path will need to be updated.
        public Dictionary<int, List<string>> GenerateDictionaryList(string DictionaryLocation = @"C:\Users\{You}\source\repos\FalloutHackingGame\DictionaryFolder\enable1.txt")
        {
            var dict = new Dictionary<int, List<string>>();
            var arg = (string[])File.ReadAllLines(DictionaryLocation);

            foreach (var word in arg)
            {
                int length = word.Length;

                //This loop checks to see if there is already a key in {dict}, and if not adds one before adding the word to the corresponding key in the dictionary {dict}.
                if (!dict.TryGetValue(length, out var list))
                {
                    list = new List<string>();
                    dict[length] = list;
                }

                list.Add(word);
            }

            return dict;
        }
    }
}
