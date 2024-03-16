using System;

namespace dotnetcore
{
    class Program
    {
        static void Main(string[] args)
        {
            PlayGame();
        }

        static void PlayGame()
        {
            bool playAgain = true;

            while (playAgain) // Repeat game play loop
            {
                // Game variables
                const int minRange = 0; // min range for the location of the Manticore
                const int maxRange = 100; // max range for the location of the Manticore
                int currentRound = 1;
                const int cityHealth = 15; // starting health for the city
                int currentCityHealth = cityHealth; // current health level
                const int manticoreHealth = 10; // starting health for the Manticore
                int currentManticoreHealth = manticoreHealth; // current Manticor health level
                int manticoreDamage = 2; // amount of damage the Manticore does to the city each round
                int cannonDamage; // cannon damage which is a calculated value per round
                string cityWinsText = "\nThe Manticore has been destroyed! The city of Consolas has been saved!";
                string manticoreWinsText = "\nOh No! Consolas has been destroyed! The world is doomed.";
                string mutualDestructionText = "\nOh No! The Manticore and Consolas have destroyed each other. What will happen to us?";

                const string separator = "-------------------------------------------------";

                bool endGame = false; // holds status of whether the game should end or not
                bool cityDestroyed = false;
                bool manticoreDestroyed = false;

                int player1Input = GetUserNumber(minRange, maxRange);
                if (player1Input == -1) // Error occurred
                    Environment.Exit(0);

                Console.WriteLine($"Player 1 stationed the Manticore at {player1Input}.\n");
                //Console.Clear();

                Console.WriteLine("Player 2, it is your turn.");
                Console.WriteLine("You need to guess the Manticore's range.");
                while (!endGame) // Game play loop
                {

                    cannonDamage = MagicCannon(currentRound);
                    Console.WriteLine(separator);
                    Console.WriteLine($"STATUS: Round: {currentRound}  City: {currentCityHealth}/{cityHealth}  Manticore: {currentManticoreHealth}/{manticoreHealth}");
                    Console.WriteLine($"The cannon is expected to deal {cannonDamage} damage this round.");

                    if (GuessNumber(player1Input))
                    {
                        currentManticoreHealth = CalculateHealth(currentManticoreHealth, cannonDamage);
                        manticoreDestroyed = CheckHealth(currentManticoreHealth);
                    }
                    currentCityHealth = CalculateHealth(currentCityHealth, manticoreDamage);
                    cityDestroyed = CheckHealth(currentCityHealth);

                    // check if anyone is dead
                    if (cityDestroyed && manticoreDestroyed)
                    {
                        Console.WriteLine(mutualDestructionText);
                        endGame = true;
                        continue;
                    }
                    else if (manticoreDestroyed)
                    {
                        Console.WriteLine(cityWinsText);
                        endGame = true;
                        continue;
                    }
                    else if (cityDestroyed)
                    {
                        Console.WriteLine(manticoreWinsText);
                        endGame = true;
                        continue;
                    }
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

        /// <summary>
        /// Routine for player 1 to supply the range of the Manticore
        /// </summary>
        /// <param name="minNumber"></param>
        /// <param name="maxNumber"></param>
        /// <returns>(int) Distance to the Manticore</returns>
        static int GetUserNumber(int minNumber, int maxNumber)
        {
            string prompt = "Player 1, how far away from the city do you want to station the Manticore?";
            int userNumber = AskForNumberInRange(prompt, minNumber, maxNumber);

            if (userNumber == -1)
            {
                Console.WriteLine("Next time please follow the instructions. Program is exiting.");
                Environment.Exit(0); // Error condition
            }
            return userNumber;
        }

        /// <summary>
        /// Guessing method for determining if player2 guessed the correct range for the Manticore
        /// </summary>
        /// <param name="player1Input"></param>
        /// <returns>(bool) Correct guess was made for the Manticore's range</returns>
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

        /// <summary>
        /// Returns what the damage level is from the city's cannon based on the supplied round information
        /// </summary>
        /// <param name="round"></param>
        /// <returns>(int) cannon damage</returns>
        static int MagicCannon(int round)
        {
            const int DivideByThree = 3;
            const int DivideByFive = 5;

            if (round % DivideByThree == 0 && round % DivideByFive == 0)
                return 10;
            else if (round % DivideByThree == 0)
                return 3;
            else if (round % DivideByFive == 0)
                return 3;
            else
                return 1;
        }

        // update health value based on supplied damage
        static int CalculateHealth(int currentHealth, int damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0) currentHealth = 0;
            return currentHealth;
        }

        // figure out if alive or not
        static bool CheckHealth(int currentHealth)
        {
            if (currentHealth == 0) return true;
            return false;
        }

        /// <summary>
        /// Asks a user to provide an integer number in the range specified by minNumber and maxNumber.
        /// A text prompt is provided to give context for the number ask. If -1 is returned it indicates
        /// an error occurred and can be dealt with in calling method.
        /// </summary>
        /// <param name="promptText"></param>
        /// <param name="minNumber"></param>
        /// <param name="maxNumber"></param>
        /// <returns>(int) user supplied number in range</returns>
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

        /// <summary>
        /// Asks a user for an integer number. Context for the requested number is given by the promptText.
        /// If -1 is returned it indicates an error occured and it can be dealt with in the calling method
        /// </summary>
        /// <param name="promptText"></param>
        /// <returns>(int) user suppled number</returns>
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
