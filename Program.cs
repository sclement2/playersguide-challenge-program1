using System;

namespace dotnetcore
{
    class Program
    {
        enum RoundResult { Short, Long, Hit }

        static void Main(string[] args)
        {
            PlayGame();
        }

        static void PlayGame()
        {
            // Game variables
            const int minRange = 0; // min range for the location of the Manticore
            const int maxRange = 100; // max range for the location of the Manticore
            int currentRound = 1;
            const int cityStartHealth = 15; // starting health for the city
            const int manticoreStartHealth = 10; // starting health for the Manticore

            int manticoreDamage = 2; // amount of damage the Manticore does to the city each round

            bool playAgain = true;

            while (playAgain) // Repeat game play loop
            {
                int cityCurrentHealth = cityStartHealth; // current health level
                int manticoreCurrentHealth = manticoreStartHealth; // current Manticor health level
                int maxRound = 12; // Highest round the game should play before exiting if no one has won

                int player1Input = GetRange(minRange, maxRange);
                if (player1Input == -1) // Error occurred
                {
                    Console.WriteLine("Next time please follow the instructions. Program is exiting.");
                    Environment.Exit(0);
                }

                Console.WriteLine($"Player 1 stationed the Manticore at {player1Input}.\n");
                //Console.Clear();

                Console.WriteLine("Player 2, it is your turn.");
                Console.WriteLine("You need to guess the Manticore's range.");
                while (manticoreCurrentHealth > 0 && cityCurrentHealth > 0 && currentRound <= maxRound) // Game play loop
                {
                    int cannonDamage = MagicCannon(currentRound);
                    GameDisplay(currentRound, cityCurrentHealth, cityStartHealth, manticoreCurrentHealth, manticoreStartHealth, cannonDamage);

                    RoundResult result = GetRoundResult(player1Input);
                    if (result == RoundResult.Hit)
                        manticoreCurrentHealth -= cannonDamage; // Manticore is damaged only if player 2 guesses the correct target range for the Manticore.
                    cityCurrentHealth -= manticoreDamage; // Manticore always does damage each round as long as it is alive
                    currentRound++;
                }

                EndGame(cityCurrentHealth, manticoreCurrentHealth);

                // Play again?
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\nDo you want to play again? (Y or N) ");
                char response = char.ToUpper(Console.ReadKey().KeyChar);

                Console.WriteLine();
                playAgain = response == 'Y';
            }
        }

        /// <summary>
        /// Determine what the outcome of the game is since an end condition has been reached
        /// </summary>
        /// <param name="cityCurrentHealth"></param>
        /// <param name="manticoreCurrentHealth"></param>
        static void EndGame(int cityCurrentHealth, int manticoreCurrentHealth)
        {
            if (cityCurrentHealth <= 0 && manticoreCurrentHealth <= 0)
                Console.WriteLine("\nOh No! The Manticore and Consolas have destroyed each other. What will happen to us?");
            else if (manticoreCurrentHealth <= 0)
                Console.WriteLine("\nThe Manticore has been destroyed! The city of Consolas has been saved!");
            else if (cityCurrentHealth <= 0)
                Console.WriteLine("\nOh No! Consolas has been destroyed! The world is doomed.");
            else
                Console.WriteLine("\nThe game ended in a stalemate. Both sides need to regroup, repair and prepare for battle again.");
        }

        static void GameDisplay(int currentRound, int cityCurrentHealth, int cityStartHealth, int manticoreCurrentHealth, int manticoreStartHealth, int cannonDamage)
        {
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine($"STATUS: Round: {currentRound}  City: {cityCurrentHealth}/{cityStartHealth}  Manticore: {manticoreCurrentHealth}/{manticoreStartHealth}");
            Console.WriteLine($"The cannon is expected to deal {cannonDamage} damage this round.");
        }

        /// <summary>
        /// Determine if player2 guessed the correct range for the Manticore.
        /// </summary>
        /// <param name="targetRange"></param>
        /// <returns>(RoundResult) enum for the possible results of the cannon shot</returns>
        static RoundResult GetRoundResult(int targetRange)
        {
            string prompt = "Enter desired cannon range";
            string tooShort = "That round FELL SHORT of the target.";
            string tooLong = "That round OVERSHOT the target.";
            string directHit = "That round was a DIRECT HIT!";

            int player2Guess = AskForNumber(prompt);
            if (player2Guess == -1)
            {
                Console.WriteLine("Next time please follow the instructions. Program is exiting.");
                Environment.Exit(0);
            }

            if (player2Guess == targetRange)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{directHit}");
                Console.ForegroundColor = ConsoleColor.White;
                return RoundResult.Hit;
            }
            else if (player2Guess < targetRange)
            {
                Console.WriteLine($"{tooShort}");
                return RoundResult.Short;
            }
            else
            {
                Console.WriteLine($"{tooLong}");
                return RoundResult.Long;
            }
        }

        /// <summary>
        /// Routine for player 1 to supply the range of the Manticore
        /// </summary>
        /// <param name="minNumber"></param>
        /// <param name="maxNumber"></param>
        /// <returns>(int) Distance to the Manticore</returns>
        static int GetRange(int minNumber, int maxNumber)
        {
            string prompt = "Player 1, how far away from the city do you want to station the Manticore?";
            int userNumber = AskForNumberInRange(prompt, minNumber, maxNumber);

            return userNumber;
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
            else if (round % DivideByThree == 0 || round % DivideByFive == 0)
                return 3;
            else
                return 1;
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
