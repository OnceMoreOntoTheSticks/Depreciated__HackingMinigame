using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FalloutHackingGame
{
    /* -----------------------------------------------------------------------
    The below items are planned enhancements for areas of the application that do not follow best-practice or adhere to proper SOLID principles.

     
    1) Add proper commentation.
    2) Address remaining UserInput edge cases (if still present after refactor)
    3) Refactor code to be more future proof:
	    A) Move 5 default diff levls to DiffLevel class and expose them to the main program.
	    B) Add customizable variable for number of guesses for each difficulty level.
	    C) Turn the password class into more of a service.

        
     ----------------------------------------------------------------------- */
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, and welcome to the hacking minigame. Would you like to use default settings, or would you like to customize your experience?");
            Console.WriteLine("Please know that customizing your experience is not recommended for beginners");
            Console.WriteLine("Please type in either \"default\" or \"customize\" ");
            Console.WriteLine();

            var UserInput = Console.ReadLine();
            Console.WriteLine();
            while (!(String.Equals(UserInput, "default", StringComparison.OrdinalIgnoreCase) || String.Equals(UserInput, "customize", StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("That is not a valid option. Please input only \"default\" or \"customize\".");
                Console.WriteLine();
                UserInput = Console.ReadLine();
                Console.WriteLine();
            }

            //Much of this middle section of the Program will be refactored on the next version. I will be consolidating this middle section of the code into a new Password service to replcae "PasswordGuesses" class and abstract much of the default experience code.
            if (String.Equals(UserInput, "default", StringComparison.OrdinalIgnoreCase))
            {           
                DifficultyLevel One = new DifficultyLevel("Very Easy", 4, 5, 5, 6);
                DifficultyLevel Two = new DifficultyLevel("Easy", 6, 8, 7, 8);
                DifficultyLevel Three = new DifficultyLevel("Average", 9, 10, 9, 10);
                DifficultyLevel Four = new DifficultyLevel("Hard", 11, 13, 11, 13);
                DifficultyLevel Five = new DifficultyLevel("Very Hard", 144, 15, 14, 15);

                List<DifficultyLevel> DifficultyOptionsSet = new List<DifficultyLevel>();
                DifficultyOptionsSet.AddRange(new List<DifficultyLevel>{One, Two, Three, Four, Five});

                Console.WriteLine("Please choose which difficulty level you would prefer: Very Easy, Easy, Average, Hard, or Very Hard");
                Console.WriteLine("If you would like to see more details about each default difficulty level, input \"help\"");
                Console.WriteLine();
                UserInput = Console.ReadLine();
                Console.WriteLine();

                //UserInput validation
                while (!(String.Equals(UserInput, "Very Easy", StringComparison.OrdinalIgnoreCase) || String.Equals(UserInput, "Easy", StringComparison.OrdinalIgnoreCase) || String.Equals(UserInput, "Average", StringComparison.OrdinalIgnoreCase) || String.Equals(UserInput, "Hard", StringComparison.OrdinalIgnoreCase) || String.Equals(UserInput, "Very Hard", StringComparison.OrdinalIgnoreCase)))
                {

                    if (String.Equals(UserInput, "Help", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (DifficultyLevel DiffLvl in DifficultyOptionsSet)
                        {
                            Console.WriteLine($"{DiffLvl.Name} can have between {DiffLvl.MinOptions} and {DiffLvl.MaxOptions} different word options, and {DiffLvl.MinLength} and {DiffLvl.MaxLength} characters per word option.");
                            
                        }

                        Console.WriteLine("Which difficulty level would you like to attempt?");
                        UserInput = Console.ReadLine();

                    }
                    else
                    {
                        Console.WriteLine("That is not a valid option. Please input any of the following default difficulty options: Very Easy, Easy, Average, Hard, or Very Hard or help");
                        UserInput = Console.ReadLine();
                    }

                }

                //This section is also planned to be refactored as well in the next version.
                DifficultyLevel UserChosenDiffLevel = new DifficultyLevel("Temp", 1, 2, 1, 2);

                foreach (DifficultyLevel DiffLvl in DifficultyOptionsSet)
                {

                    if (String.Equals(UserInput, DiffLvl.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        UserChosenDiffLevel = DiffLvl;
                        break;
                    }

                }

                var PossiblePasswords = Password_Guesses.Password_Guesses_Generator(UserChosenDiffLevel);
                var rand = new Random();
                var password = PossiblePasswords[rand.Next(0, PossiblePasswords.Count)];

                Console.WriteLine("Your password is one of the following words listed below. Please type in what you suspect to be the password. The password is case sensitive: ");
                Console.WriteLine();
                foreach (string word in PossiblePasswords)
                {
                    Console.WriteLine(word);
                }
                Console.WriteLine();

                var WasPasswordCorrect = false;

                for (int i = 4; i > 0; i--)
                {
                    Console.WriteLine("Please input one of the above password options: ");
                    Console.WriteLine();
                    var PasswordGuess = Console.ReadLine();

                    if (String.Equals(PasswordGuess, password, StringComparison.OrdinalIgnoreCase))
                    {
                        WasPasswordCorrect = true;
                        break;
                    }

                    int NumCorrectCharacters = 0;
                    char[] PasswordArray = password.ToCharArray();
                    char[] UserGuessArray = PasswordGuess.ToCharArray();
                    for (int j = 0; j < PasswordArray.Length; j++)
                    {
                        if (PasswordArray[j] == UserGuessArray[j])
                        {
                            NumCorrectCharacters++;
                        }
                    }
                    Console.WriteLine($"{NumCorrectCharacters} of the letters matched the correct password in the corresponding spot in the word. Please try again");
                    Console.WriteLine($"That was not the correct password. You have {i - 1} attempts left");
                    Console.WriteLine();

                }

                if (WasPasswordCorrect)
                {
                    //humor
                    Console.WriteLine("You're in! You have hacked this device. The FBI is on the way");
                }
                else
                {
                    //humor
                    Console.WriteLine("You are locked out of this device. Please contact your IT department to have your password reset if you feel this is in error");
                }

            }

            //This is the beginning of the section of code for if the user wanted to customize their experience.
            else
            {
                Console.WriteLine("How many different difficulties would you like? Please enter in a number and note that you will need to enter in five pieces of information for each difficulty. So choosing a large number of difficulties will require lots of input and may take awhile. Also there is no input validation in this section of code, so any typos will error out the program.");
                var NumOfDiff = Int32.Parse(Console.ReadLine());

                var DiffSet = new List<DifficultyLevel>();

                for (int i = 1; i <= NumOfDiff; i++)
                {
                    Console.WriteLine($"What would you like the name of difficulty {i} to be?");
                    string DiffName = Console.ReadLine();

                    Console.WriteLine("What would you like the minimum number of letters in the password to be? Please use a numerical number");
                    int DiffMinLength = Int32.Parse(Console.ReadLine());

                    Console.WriteLine("What would you like the maximum number of letters in the password to be? Please use a numerical number");
                    int DiffMaxLength = Int32.Parse(Console.ReadLine());

                    Console.WriteLine("What would you like the minimum number of possible passwords to be for this difficulty? Please use a numerical number");
                    int DiffMinOptions = Int32.Parse(Console.ReadLine());

                    Console.WriteLine("What would you like the maximum number of possible passwords to be for this difficulty? Please use a numerical number");
                    int DiffMaxOptions = Int32.Parse(Console.ReadLine());

                    DiffSet.Add(new DifficultyLevel(DiffName, DiffMinLength, DiffMaxLength, DiffMinOptions, DiffMaxOptions));
                }

                Console.WriteLine("Please select the difficulty level you would like to attempt: ");
                foreach (DifficultyLevel DiffLvl in DiffSet)
                {
                    Console.WriteLine(DiffLvl.Name);
                }

                UserInput = Console.ReadLine();
                DifficultyLevel UserChosenDiffLevel = new DifficultyLevel("Temp", 1, 2, 1, 2);

                foreach (DifficultyLevel DiffLvl in DiffSet)
                {

                    if (String.Equals(UserInput, DiffLvl.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        UserChosenDiffLevel = DiffLvl;
                        break;
                    }

                }

                var PossiblePasswords = Password_Guesses.Password_Guesses_Generator(UserChosenDiffLevel);
                var rand = new Random();
                var password = PossiblePasswords[rand.Next(0, PossiblePasswords.Count)];


                Console.WriteLine("Your password is one of the following words listed below. Please type in what you suspect to be the password. The password is case sensitive");
                foreach (string word in PossiblePasswords)
                {
                    Console.WriteLine(word);
                }

                var WasPasswordCorrect = false;

                for (int i = 4; i > 0; i--)
                {
                    Console.WriteLine("Please input one of the above password options: ");
                    Console.WriteLine();
                    var PasswordGuess = Console.ReadLine();

                    if (String.Equals(PasswordGuess, password, StringComparison.OrdinalIgnoreCase))
                    {
                        WasPasswordCorrect = true;
                        break;
                    }

                    int NumCorrectCharacters = 0;
                    char[] PasswordArray = password.ToCharArray();
                    char[] UserGuessArray = PasswordGuess.ToCharArray();
                    for (int j = 0; j < PasswordArray.Length; j++)
                    {
                        if (PasswordArray[j] == UserGuessArray[j])
                        {
                            NumCorrectCharacters++;
                        }
                    }

                    Console.WriteLine($"{NumCorrectCharacters} of the letters matched the correct password in the corresponding spot in the word. Please try again");
                    Console.WriteLine($"That was not the correct password. You have {i-1} attempts left");
                    Console.WriteLine();

                }

                if (WasPasswordCorrect)
                {
                    //humor sprinkling
                    Console.WriteLine();
                    Console.WriteLine("You're in! You have hacked this device. The FBI is on the way");
                }
                else
                {
                    //humor sprinkling
                    Console.WriteLine();
                    Console.WriteLine("You are locked out of this device. Please contact your IT department to have your password reset if you feel this is in error");
                }

            }

        }
    }
}
//For the time being I am leaving in some edge cases for inproper UserInput in the customized version of the game, as someone that wants to customize it should not need to have their hand held quite as much.
//One of my ToDo's for one of the next iterations is to address the remaining UserInput edge cases.

/*

-----------------------------------------------------------------------------------------------

**Below is the reference details for the default version of the game listed in the below linked forum post that this application is based on.

Difficult: 1
4-5 letters
5-6 words

Difficult: 2
6-8 letters
7-8 words

Difficult: 3
9-10 letters
9-10 words

Difficult: 4
11-13 letters
11-13 words

Difficult: 5
14-15 letters
14-15 words
*/
//https://www.reddit.com/r/dailyprogrammer/comments/3qjnil/20151028_challenge_238_intermediate_fallout/
