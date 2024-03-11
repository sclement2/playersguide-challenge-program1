using System;
using Microsoft.Win32.SafeHandles;

namespace dotnetcore
{
    class Program
    {
        static void Main(string[] args)
        {
            //int range = GetUserNumber(0, 100);
            //Console.WriteLine($"Did I hit the target? {GuessNumber(range)}");

            PlayGame();
        }

        static void PlayGame()
        {
            bool playAgain = true;

            while (playAgain)
            {
                // Game variables
                const int minRange = 0;
                const int maxRange = 100;
                int currentRound = 1;
                const int cityHealth = 15;
                int currentCityHealth = cityHealth;
                const int manticoreHealth = 10;
                int currentManticoreHealth = manticoreHealth;
                int manticoreDamage = 2;
                int cannonDamage;
                string cityWinsText = "\nThe Manticore has been destroyed! The city of Consolas has been saved!";
                string manticoreWinsText = "\nOh No! Consolas has been destroyed! What will happen to us?";

                const string separator = "-------------------------------------------------";

                bool endGame = false;

                int player1Input = GetUserNumber(minRange, maxRange);
                if (player1Input == -1) // Error occurred
                    Environment.Exit(0);

                Console.WriteLine($"Player 1 stationed the Manticore at {player1Input}.\n");
                //Console.Clear();

                Console.WriteLine("Player 2, it is your turn.");
                Console.WriteLine("You need to guess the Manticore's range.");
                while (!endGame)
                {
                    cannonDamage = MagicCannon(currentRound);
                    Console.WriteLine(separator);
                    Console.WriteLine($"STATUS: Round: {currentRound}  City: {currentCityHealth}/{cityHealth}  Manticore: {currentManticoreHealth}/{manticoreHealth}");
                    Console.WriteLine($"The cannon is expected to deal {cannonDamage} damage this round.");

                    if (GuessNumber(player1Input))
                    {
                        currentManticoreHealth = CalculateHealth(currentManticoreHealth, cannonDamage);
                        // Info: Need to figure out how to do mutual destruction. Is that possible?
                        endGame = CheckHealth(currentManticoreHealth, cityWinsText);
                        if (endGame) continue;
                    }
                    currentCityHealth = CalculateHealth(currentCityHealth, manticoreDamage);
                    // Info: Need to figure out how to do mutual destruction. Is that possible?
                    endGame = CheckHealth(currentCityHealth, manticoreWinsText);
                    currentRound++;

                    if (currentRound == 12) endGame = true;
                }

                // Play again?
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\nDo you want to play again? (Y or N) ");
                char response = char.ToUpper(Console.ReadKey().KeyChar);

                Console.WriteLine();
                playAgain = response == 'Y';
            }
        }

        static int GetUserNumber(int minNumber, int maxNumber)
        {
            string prompt = "Player 1, how far away from the city do you want to station the Manticore?";
            int range = AskForNumberInRange(prompt, minNumber, maxNumber);

            if (range == -1)
            {
                Console.WriteLine("Next time please follow the instructions. Program is exiting.");
                Environment.Exit(0); // Error condition
            }
            return range;
        }

        static bool GuessNumber(int player1Input)
        {
            string toShort = "That round FELL SHORT of the target.";
            string toLong = "That round OVERSHOT the target.";
            string directHit = "That round was a DIRECT HIT!";
            string prompt = "Enter desired cannon range";

            int player2Guess = AskForNumber(prompt);

            if (player2Guess == -1)
            {
                Console.WriteLine("Next time please follow the instructions. Program is exiting.");
                Environment.Exit(0); // Error condition
            }

            if (player2Guess < player1Input)
            {
                Console.WriteLine($"{toShort}");
                return false;
            }
            else if (player2Guess > player1Input)
            {
                Console.WriteLine($"{toLong}");
                return false;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{directHit}");
                Console.ForegroundColor = ConsoleColor.White;
                return true; // guessed correctly
            }
        }

        static int MagicCannon(int round)
        {
            const int DivideByThree = 3;
            const int DivideByFive = 5;

            if (round % DivideByThree == 0 && round % DivideByFive == 0)
            {
                return 10;
            }
            else if (round % DivideByThree == 0)
            {
                return 3;
            }
            else if (round % DivideByFive == 0)
            {
                return 3;
            }
            else
            {
                return 1;
            }
        }

        static int CalculateHealth(int currentHealth, int damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0) currentHealth = 0;

            return currentHealth;
        }

        static bool CheckHealth(int currentHealth, string deathText)
        {

            if (currentHealth == 0)
            {
                Console.WriteLine(deathText);
                return true;
            }
            return false;
        }

        static int AskForNumberInRange(string promptText, int minNumber, int maxNumber)
        {
            Console.WriteLine($"{promptText}");
            while (true)
            {
                Console.Write($"Please enter an integer between {minNumber} and {maxNumber}: ");
                if (int.TryParse(Console.ReadLine(), out int result))
                {
                    if (result >= minNumber && result <= maxNumber)
                        return result;
                    else
                    {
                        Console.WriteLine("Sorry, Your response was not valid. Let's try this again.");
                        continue;
                    }
                }
                Console.WriteLine("You did not enter an integer.");
                return -1;
            }
        }

        static int AskForNumber(string promptText)
        {
            Console.Write($"{promptText}: ");
            if (int.TryParse(Console.ReadLine(), out int result))
            {
                return result;
            }
            Console.WriteLine("You did not enter an integer.");
            return -1;
        }
    }
}
