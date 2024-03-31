using System.Text.RegularExpressions;

namespace AnnikaAvikaBirthday
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StartScreen();

            string name = GetUserInput("Which twin are you? ", true, ["Annika", "Avika"], true).ToLower();

            string encryptedDir = name switch {
                "annika" => @"../../../ForAnnika.aes",
                "avika" => @"../../../ForAvika.aes",
                _ => throw new Exception("Wrong name somehow")
            };

            WrongPassword:

            string password = GetUserInput("Enter your password: ", true);

            string decrypted = Decrypt(encryptedDir, password);

            if (decrypted[..14].Equals("Happy birthday"))
            {
                Console.Clear();
                Console.WriteLine("You might want to go somewhere private for this...");
                PressEnterToContinue();

                Console.Clear();
                Console.WriteLine(decrypted);
                Console.WriteLine();
                PressEnterToContinue();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("That was the wrong password");
                string tryAgainStr = GetUserInput("That was the wrong password\nDo you want to try again? ", true, ["Y", "N"], true).ToLower();
                bool tryAgain = tryAgainStr switch { "y" => true, "n" => false, _ => false };

                if (tryAgain)
                {
                    goto WrongPassword;
                }
            }

            Console.Clear();
            Console.WriteLine("Happy birthday you two");
        }

        static void Encrypt(string inputFile, string outputFile, string password)
        {
            List<int> key = [];
            string unencrypted = "";
            string encrypted = "";

            for (int i = 0; i < password.Length; i++)
            {
                int num = password[i];
                key.Add(num);
            }

            using (StreamReader sr = new(inputFile))
            {
                string? s;
                while (true)
                {
                    s = sr.ReadLine();

                    if (s == null) break;

                    unencrypted += s + "\n";
                }
            }

            unencrypted = unencrypted.Trim('\n');

            char[] unencryptedArray = unencrypted.ToCharArray();
            int[] encryptedArray = new int[unencryptedArray.Length];

            for (int i = 0; i < unencryptedArray.Length; i++)
            {
                int keyNum = i % key.Count;

                int num = unencryptedArray[i] * key[keyNum];

                encryptedArray[i] = num;
            }

            encrypted = string.Join('\n', encryptedArray);

            using (StreamWriter sw = new(outputFile))
            {
                sw.Write(encrypted);
            }
        }

        static string Decrypt(string inputFile, string password)
        {
            List<int> key = [];
            string unencrypted;
            string encrypted = "";

            for (int i = 0; i < password.Length; i++)
            {
                int num = password[i];
                key.Add(num);
            }

            using (StreamReader sr = new(inputFile))
            {
                string? s;
                while (true)
                {
                    s = sr.ReadLine();

                    if (s == null) break;

                    encrypted += s + "\n";
                }
            }

            encrypted = encrypted.Trim('\n');

            string[] encryptedArray = encrypted.Split('\n');
            char[] unencryptedArray = new char[encryptedArray.Length];

            for (int i = 0; i < unencryptedArray.Length; i++)
            {
                int keyNum = i % key.Count;

                int num = int.Parse(encryptedArray[i]) / key[keyNum];

                unencryptedArray[i] = (char)num;
            }

            unencrypted = new(unencryptedArray);

            return unencrypted;
        }

        static void StartScreen()
        {
            // Get window width and height
            int width = -1;
            int height = -1;

            int halfExtraHeight, extraHeight, halfExtrawidth, extraWidth;
            halfExtraHeight = extraHeight = halfExtrawidth = extraWidth = 0;

            // String array holds the acsii art
            string[] title =
            {
                """      ██╗  ██╗ █████╗ ██████╗ ██████╗ ██╗   ██╗    ██████╗ ██╗██████╗ ████████╗██╗  ██╗██████╗  █████╗ ██╗   ██╗     """,
                """      ██║  ██║██╔══██╗██╔══██╗██╔══██╗╚██╗ ██╔╝    ██╔══██╗██║██╔══██╗╚══██╔══╝██║  ██║██╔══██╗██╔══██╗╚██╗ ██╔╝     """,
                """      ███████║███████║██████╔╝██████╔╝ ╚████╔╝     ██████╔╝██║██████╔╝   ██║   ███████║██║  ██║███████║ ╚████╔╝      """,
                """      ██╔══██║██╔══██║██╔═══╝ ██╔═══╝   ╚██╔╝      ██╔══██╗██║██╔══██╗   ██║   ██╔══██║██║  ██║██╔══██║  ╚██╔╝       """,
                """      ██║  ██║██║  ██║██║     ██║        ██║       ██████╔╝██║██║  ██║   ██║   ██║  ██║██████╔╝██║  ██║   ██║        """,
                """      ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝     ╚═╝        ╚═╝       ╚═════╝ ╚═╝╚═╝  ╚═╝   ╚═╝   ╚═╝  ╚═╝╚═════╝ ╚═╝  ╚═╝   ╚═╝        """,
                """                                                                                                                     """,
                """ █████╗ ███╗   ██╗███╗   ██╗██╗██╗  ██╗ █████╗      █████╗ ███╗   ██╗██████╗      █████╗ ██╗   ██╗██╗██╗  ██╗ █████╗ """,
                """██╔══██╗████╗  ██║████╗  ██║██║██║ ██╔╝██╔══██╗    ██╔══██╗████╗  ██║██╔══██╗    ██╔══██╗██║   ██║██║██║ ██╔╝██╔══██╗""",
                """███████║██╔██╗ ██║██╔██╗ ██║██║█████╔╝ ███████║    ███████║██╔██╗ ██║██║  ██║    ███████║██║   ██║██║█████╔╝ ███████║""",
                """██╔══██║██║╚██╗██║██║╚██╗██║██║██╔═██╗ ██╔══██║    ██╔══██║██║╚██╗██║██║  ██║    ██╔══██║╚██╗ ██╔╝██║██╔═██╗ ██╔══██║""",
                """██║  ██║██║ ╚████║██║ ╚████║██║██║  ██╗██║  ██║    ██║  ██║██║ ╚████║██████╔╝    ██║  ██║ ╚████╔╝ ██║██║  ██╗██║  ██║""",
                """╚═╝  ╚═╝╚═╝  ╚═══╝╚═╝  ╚═══╝╚═╝╚═╝  ╚═╝╚═╝  ╚═╝    ╚═╝  ╚═╝╚═╝  ╚═══╝╚═════╝     ╚═╝  ╚═╝  ╚═══╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝""",
                """                                                                                                                     """,
                """                                               <Press ENTER to continue>                                             """
            };

            // Integer form of the colours of the rainbow
            int[] rainbowColours = [12, 6, 10, 3, 9, 13];

            // Make cursor invisible
            Console.CursorVisible = false;

            // Initialize variable for visibility state
            bool isVisible = false;

            // Infinite loop for flashing on start text
            while (true)
            {
                // Check if user has resized screen, and will redraw if so
                if (width != Console.WindowWidth || height != Console.WindowHeight)
                {
                    Console.Clear();

                    width = Console.WindowWidth;
                    height = Console.WindowHeight;

                    // Declare variables to put the text in the middle
                    extraWidth = width - title[0].Length;
                    halfExtrawidth = extraWidth / 2;
                    extraHeight = height - title.Length;
                    halfExtraHeight = extraHeight / 2;
                }

                isVisible = !isVisible;

                if (isVisible)
                {
                    // Iterate through each string in title
                    for (int i = 0; i < title.Length; i++)
                    {
                        // Set colour, cursor position, and print
                        int j = i % rainbowColours.Length;
                        Console.ForegroundColor = (ConsoleColor)rainbowColours[j];
                        Console.SetCursorPosition(halfExtrawidth, halfExtraHeight + i);
                        Console.WriteLine(title[i]);
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;

                    // Iterate through each string in title
                    for (int i = 0; i < title.Length; i++)
                    {
                        // Set colour, cursor position, and print
                        Console.SetCursorPosition(halfExtrawidth, halfExtraHeight + i);
                        Console.WriteLine(title[i]);
                    }
                }

                // Check if the user pressed the Enter key
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    break;
                }

                Thread.Sleep(500);
            }

            // Make cursor visible again
            Console.CursorVisible = true;

            // Set colour back to white
            Console.ForegroundColor = ConsoleColor.White;
        }

        static string GetUserInput(string question, bool inline = false, string[]? wantedVals = null, bool alphanumericOnly = false)
        {
            // Declares string variables
            string outputStr;
            string? inputStr;

            // Converts array of strings into a single string formatted as so
            // " [{0}/{1}/{2}/...]"
            string wantedValsStr = MakeWantedValStr(wantedVals);

            Console.Clear();

            // While looop holding the question and input logic
            while (true)
            {
                // If you want user to input on the same line as the question or not
                if (inline)
                {
                    Console.Write(question);
                }
                else
                {
                    Console.WriteLine(question);
                }
                // Gets user input. Used Trim Method so you cannot put in a lone space
                inputStr = Console.ReadLine()?.Trim();

                // If string is not empty
                if (!string.IsNullOrEmpty(inputStr))
                {
                    // If the input is in the wanted values if the wanted values were provided
                    bool isInWantedVals = (wantedVals != null && wantedVals.Any(wantedVal => wantedVal.Equals(inputStr, StringComparison.OrdinalIgnoreCase))) || (wantedVals == null);

                    bool isAlphabetical = Regex.IsMatch(inputStr, @"^[a-zA-Z]+$") || !alphanumericOnly;

                    if (isInWantedVals)
                    {
                        if (isAlphabetical)
                        {
                            break;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine($"Please enter only alphabetical values.\n");
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"Invalid input.{wantedValsStr}\n");
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"You did not input a value.{wantedValsStr}\n");
                }
            }

            // Sets output string to inpur string after all the logic
            outputStr = inputStr;

            // Returns string
            return outputStr;
        }

        static string MakeWantedValStr(string[]? wantedVals)
        {
            if (wantedVals != null)
            {
                // Converts array of strings into a single string formatted as so
                // " [{0}/{1}/{2}/...]"
                // If the string array is not empty

                string wantedValsStr = " Please enter [";

                for (int i = 0; i < wantedVals.Length; i++)
                {
                    wantedValsStr += wantedVals[i];
                    if (i != wantedVals.Length - 1)
                    {
                        wantedValsStr += "/";
                    }
                }
                wantedValsStr += "]";

                return wantedValsStr;
            }
            else
            {
                return "";
            }
        }

        static void PressEnterToContinue()
        {
            Console.WriteLine("Press enter to continue");

            // Use Console.ReadKey method to make it so whatever they input, they wont see it
            ConsoleKeyInfo consoleKey;
            do
            {
                consoleKey = Console.ReadKey(true);
            } while (consoleKey.Key != ConsoleKey.Enter);
        }
    }
}
