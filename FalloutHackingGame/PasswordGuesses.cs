using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutHackingGame
{
    //This class can provide the password and the set of word options to choose from to guess the password.
    //This class and the main program have a good amount of refactoring planned for it. But this was a 2nd pass at the code structure and is functional.
    //Will be updating much of this to more closely align with SOLID design principles and programming best-practices.
    public class Password_Guesses
    {
        public static List<string> Password_Guesses_Generator(DifficultyLevel Diff_Chosen)
        {
            Dictionary Dict_Initiation = new Dictionary();
            var dict = Dict_Initiation.GenerateDictionaryList();
            var rand = new Random();


            int PasswordLength = rand.Next(Diff_Chosen.MinLength, (Diff_Chosen.MaxLength + 1));
            int NumOfWordOptions = rand.Next(Diff_Chosen.MinOptions, (Diff_Chosen.MaxOptions + 1));
            var passwordOptionsSet = new List<string>();

            string WordToAddToGuesses = "temp";

            for (int i = 1; i <= NumOfWordOptions; i++)
            {
                WordToAddToGuesses = dict[PasswordLength].ElementAt(rand.Next(0, (dict[PasswordLength].Count - 1)));

                if (passwordOptionsSet.Contains(WordToAddToGuesses))
                {
                    i -= 1;
                }
                else
                {
                    passwordOptionsSet.Add(WordToAddToGuesses);
                }
            }

            return passwordOptionsSet;
        }

    }
}
